using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VavilichevGD.Gameplay.Dialogues;

namespace VavilichevGD.Editor.Dialogues {
	public class GraphSaveUtility {

		#region CONSTANTS

		private const string DIRRECTORY_DEFAULT = "Assets/Resources/Dialogues/";
		private const string FILE_NAME_DEFAULT = "DialogueGraph";
		private const string EXTENSION = "asset";

		#endregion

		#region EVENTS

		public event Action<string> OnFileNameChangedEvent;

		#endregion

		private DialogueGraphView targetGraphView;
		private DialogueInfo cachedInfo;
		
		private List<Edge> edges => this.targetGraphView.edges.ToList();
		private List<EditorDialogueNode> nodes => this.targetGraphView.nodes.ToList().Cast<EditorDialogueNode>().ToList();

		public GraphSaveUtility(DialogueGraphView graphView) {
			this.targetGraphView = graphView;
		}


		#region SAVING

		public void SaveBeforeClose() {
			// If we worked with empty graph.
			if (this.cachedInfo == null) {
				
				// If new graph stay empty - we don't have to do anything.
				var possibleNewInfo = ScriptableObject.CreateInstance<DialogueInfo>();
				if (!this.WriteNodesToInfo(possibleNewInfo))
					return;
				
				// If we created something - we must to warn user that he can lost all unsaved data.
				var saveNewGraph = EditorUtility.DisplayDialog("Exit without saving",
					"You did not saved the graph. Do you want to save it?", "Save", "Exit");
				
				// If he agreed to save - start saving new file method.
				if (saveNewGraph)
					this.SaveGraphAs();
				
				// Otherwise - do nothing.
				return;
			}
			
			// If we worked with opened graph and it becomes empty - we dont save it.
			var newDialogueInfo = ScriptableObject.CreateInstance<DialogueInfo>();
			if (!this.WriteNodesToInfo(newDialogueInfo))
				return;

			// if we worked with open graph and it hasn't changed - we do nothing. Because there is no changes.
			if (this.cachedInfo.Equals(newDialogueInfo))
				return;

			// Otherwise - warn user about possibility to lose that changes.
			var needToSave = EditorUtility.DisplayDialog("Exit without saving",
				"You did not saved the graph. Do you want to save it?", "Save", "Exit");
			
			// And if user agrees to save - we run SaveGraph procedure.
			if (needToSave) 
				this.SaveGraph();
		}


		public void SaveGraph() {
			// If we worked with empty graph - we try to save it as new file.
			if (this.cachedInfo == null) {
				this.SaveGraphAs();
				return;
			}

			// Otherwise - we just overwrite existing file. 
			var path = AssetDatabase.GetAssetPath(this.cachedInfo);
			if (this.WriteNodesToInfo(this.cachedInfo)) {
				AssetDatabase.SaveAssets();
				Debug.Log($"Dialogue Graph overwritten. Path: {path}");
			}
		}

		public void SaveGraphAs() {
			// Get full path from SaveFile popup
			var fullPath = EditorUtility.SaveFilePanel(
				"Save Graph",
				DIRRECTORY_DEFAULT,
				FILE_NAME_DEFAULT,
				EXTENSION);

			// If user clicked cancel - do nothing.
			if (string.IsNullOrEmpty(fullPath))
				return;

			// Get local project path. Because AssetDataBase doestn work with full path.
			var localPath = this.ConvertFullPathToLocal(fullPath);
			
			// If file already exists - we just overwrite it (if user agrees with that).
			var loadedAssetAtPath = AssetDatabase.LoadAssetAtPath<DialogueInfo>(localPath);
			if (loadedAssetAtPath != null) {
				if (this.WriteNodesToInfo(loadedAssetAtPath)){
					AssetDatabase.SaveAssets();
					this.cachedInfo = loadedAssetAtPath;
					this.OnFileNameChangedEvent?.Invoke(this.cachedInfo.name);
					Debug.Log($"Dialogue Graph overwritten. Path: {localPath}");
					return;
				}
			}
			
			// Otherwise - create and save new file.
			var newDialogueInfo = ScriptableObject.CreateInstance<DialogueInfo>();
			if (this.WriteNodesToInfo(newDialogueInfo)){
				AssetDatabase.CreateAsset(newDialogueInfo, localPath);	
				AssetDatabase.SaveAssets();
				this.cachedInfo = newDialogueInfo;
				this.OnFileNameChangedEvent?.Invoke(this.cachedInfo.name);
				Debug.Log($"Dialogue Graph Saved. Path: {localPath}");
			}
		}

		private string ConvertFullPathToLocal(string fullPath) {
			var subString = "Assets/";
			var tempLocalPath = fullPath.Substring(fullPath.IndexOf(subString) + subString.Length);
			return $"Assets/{tempLocalPath}";
		}
		
		private bool WriteNodesToInfo(DialogueInfo dialogueInfo) {
			if (!this.edges.Any())
				return false;

			dialogueInfo.Clear();
			var connectedPorts = this.edges.Where(x => x.input.node != null).ToArray();
			for (int i = 0; i < connectedPorts.Length; i++) {
				var outputNode = connectedPorts[i].output.node as EditorDialogueNode;
				var inputNode = connectedPorts[i].input.node as EditorDialogueNode;
				
				dialogueInfo.nodeLinks.Add(new NodeLinkData {
					baseNodeGUID = outputNode.GUID,
					portName = connectedPorts[i].output.portName,
					targetNodeGUID = inputNode.GUID
				});
			}

			foreach (var dialogueNode in nodes.Where(node => !node.entryPoint)) {
				dialogueInfo.dialogueNodeData.Add(new DialogueNodeData {
					guid = dialogueNode.GUID,
					author = dialogueNode.author,
					dialogueText = dialogueNode.text,
					position = dialogueNode.GetPosition().position
				});
				
			}

			return true;
		}

		#endregion



		#region LOADING

		public void LoadGraph(DialogueInfo dialogueInfo) {
			this.cachedInfo = dialogueInfo;
			this.OnFileNameChangedEvent?.Invoke(this.cachedInfo.name);
			
			this.ClearGraph();
			this.CreateNodes();
			this.ConnectNodes();
		}

		public void LoadGraph() {
			var fullPath = EditorUtility.OpenFilePanel(
				"Load Graph",
				DIRRECTORY_DEFAULT,
				EXTENSION);
			
			if (string.IsNullOrEmpty(fullPath))
				return;
			
			var localPath = this.ConvertFullPathToLocal(fullPath);
			var loadedDialogueGraph = AssetDatabase.LoadAssetAtPath<DialogueInfo>(localPath);
			if (loadedDialogueGraph == null) {
				EditorUtility.DisplayDialog("Error",
					$"File {localPath} is not a DialogueInfo scriptable object.",
					"Ok");
				return;
			}
			
			this.LoadGraph(loadedDialogueGraph);
		}

		private void ClearGraph() {
			nodes.Find(x => x.entryPoint).GUID = cachedInfo.nodeLinks[0].baseNodeGUID;

			foreach (var node in nodes) {
				if (node.entryPoint) 
					continue;

				// remove edges that connected to this node
				edges.Where(x => x.input.node == node).ToList()
					.ForEach(edge => targetGraphView.RemoveElement(edge));
				
				// then remove the node 
				targetGraphView.RemoveElement(node);
			}
		}
		
		private void CreateNodes() {
			foreach (var nodeData in cachedInfo.dialogueNodeData) {

				var tempNode = targetGraphView.CreateDialogueNode(nodeData.dialogueText, nodeData.author, Vector2.zero);
				tempNode.GUID = nodeData.guid;
				targetGraphView.AddElement(tempNode);

				var nodePorts = cachedInfo.nodeLinks.Where(x => x.baseNodeGUID == nodeData.guid).ToList();
				nodePorts.ForEach(x=> targetGraphView.AddChoiсePort(tempNode, x.portName));
			}
		}
		
		private void ConnectNodes() {
			for (int i = 0; i < nodes.Count; i++) {
				var connections = cachedInfo.nodeLinks.Where(x => x.baseNodeGUID == nodes[i].GUID).ToList();
				for (int j = 0; j < connections.Count; j++) {
					var targetNodeGuid = connections[j].targetNodeGUID;
					var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
					LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
					
					targetNode.SetPosition(new Rect(cachedInfo.dialogueNodeData.First(x => x.guid == targetNodeGuid).position,
						targetGraphView.defaultNodeSize));
				}
			}
		}

		private void LinkNodes(Port output, Port input) {
			var tempEdge = new Edge {
				output = output,
				input = input
			};

			tempEdge?.input.Connect(tempEdge);
			tempEdge?.output.Connect(tempEdge);
			targetGraphView.Add(tempEdge);
		}

		#endregion
		
	}
}