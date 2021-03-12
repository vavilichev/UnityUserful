using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VavilichevGD.Gameplay.Dialogues;
using Button = UnityEngine.UIElements.Button;

namespace VavilichevGD.Editor.Dialogues {
	public class DialogueGraphView : GraphView {

		#region CONSTANTS

		public readonly Vector2 defaultNodeSize = new Vector2(150, 200);

		#endregion

		private NodeSearchWindow searchWindow;

		public DialogueGraphView(EditorWindow editorWindow) {

			this.styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
			this.SetupZoom(0.05f, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());


			var grid = new GridBackground();
			this.Insert(0, grid);
			grid.StretchToParentSize();

			var entryPointNode = this.GenerateEntryPointNode();
			this.AddElement(entryPointNode);
			this.AddSearchWindow(editorWindow);
		}
		
		
		private EditorDialogueNode GenerateEntryPointNode() {
			var node = new EditorDialogueNode {
				title = "Start",
				GUID = Guid.NewGuid().ToString(),
				text = "ENTRYPOINT",
				entryPoint = true
			};

			var generatedPort = this.CreatePort(node, Direction.Output);
			generatedPort.portName = "Begin";
			node.outputContainer.Add(generatedPort);

			node.capabilities &= ~Capabilities.Movable;
			node.capabilities &= ~Capabilities.Deletable;

			node.RefreshExpandedState();
			node.RefreshPorts();

			node.SetPosition(new Rect(100, 200, 100, 150));
			return node;
		}
		
		
		private void AddSearchWindow(EditorWindow editorWindow) {
			this.searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
			this.searchWindow.Initialize(editorWindow, this);
			this.nodeCreationRequest = context =>
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
			var compatiblePorts = new List<Port>();

			ports.ForEach(port => {
				if (startPort != port && startPort.node != port.node)
					compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}

		

		#region CREATE DIALOGUE NODE

		public void CreateNode(string nodeName, string dialogueAuthor, Vector2 position) {
			this.AddElement(this.CreateDialogueNode(nodeName, dialogueAuthor, position));
		}
		

		public EditorDialogueNode CreateDialogueNode(string nodeName, string dialogueAuthor, Vector2 position) {
			var dialogueNode = new EditorDialogueNode {
				title = nodeName,
				author = dialogueAuthor,
				text = nodeName,
				GUID = Guid.NewGuid().ToString()
			};

			dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
			
			this.CreateInputPort(dialogueNode);
			this.CreateAddChoiceButton(dialogueNode);
			this.CreateAuthorField(dialogueNode);
			this.CreateDialogueTextField(dialogueNode);
			
			dialogueNode.RefreshExpandedState();
			dialogueNode.RefreshPorts();
			dialogueNode.SetPosition(new Rect(position, defaultNodeSize));
			
			return dialogueNode;
		}

		private void CreateInputPort(EditorDialogueNode editorDialogueNode) {
			var inputPort = this.CreatePort(editorDialogueNode, Direction.Input, Port.Capacity.Multi);
			inputPort.portName = "Input";
			editorDialogueNode.inputContainer.Add(inputPort);
		}

		private void CreateAddChoiceButton(EditorDialogueNode editorDialogueNode) {
			var button = new Button(() => { this.AddChoiсePort(editorDialogueNode); });
			button.text = "Add Choice";
			editorDialogueNode.titleContainer.Add(button);
		}

		private void CreateAuthorField(EditorDialogueNode editorDialogueNode) {
			var dialogueAuthorField = new TextField("Author");
			dialogueAuthorField.RegisterValueChangedCallback(evt => {
				editorDialogueNode.author = evt.newValue;
			});
			dialogueAuthorField.SetValueWithoutNotify(editorDialogueNode.author);
			editorDialogueNode.mainContainer.Add(dialogueAuthorField);
		}

		private void CreateDialogueTextField(EditorDialogueNode editorDialogueNode) {
			var dialogueTextField = new TextField("DialogueText");
			dialogueTextField.RegisterValueChangedCallback(evt => {
				editorDialogueNode.text = evt.newValue;
				editorDialogueNode.title = evt.newValue;
			});
			dialogueTextField.SetValueWithoutNotify(editorDialogueNode.title);
			editorDialogueNode.mainContainer.Add(dialogueTextField);
		}

		#endregion


		#region CREATE PORT

		private Port CreatePort(EditorDialogueNode node, Direction portDirection,
			Port.Capacity capacity = Port.Capacity.Single) {
			return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
		}

		public void AddChoiсePort(EditorDialogueNode editorDialogueNode, string overridedPortName = "") {
			var generatedPort = this.CreatePort(editorDialogueNode, Direction.Output);
			var oldLabel = generatedPort.contentContainer.Q<Label>("type");
			generatedPort.contentContainer.Remove(oldLabel);

			var outputPortCount = editorDialogueNode.outputContainer.Query("connector").ToList().Count;
			var choicePortName = string.IsNullOrEmpty(overridedPortName)
				? $"Choice {outputPortCount}"
				: overridedPortName;
			generatedPort.portName = choicePortName;

			var textField = new TextField {
				name = string.Empty,
				value = choicePortName
			};
			textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
			generatedPort.contentContainer.Add(new Label(" "));
			generatedPort.contentContainer.Add(textField);

			var deleteButton = new Button(() => this.RemovePort(editorDialogueNode, generatedPort)) {
				text = "X"
			};
			generatedPort.contentContainer.Add(deleteButton);


			editorDialogueNode.outputContainer.Add(generatedPort);
			editorDialogueNode.RefreshPorts();
			editorDialogueNode.RefreshExpandedState();
		}

		private void RemovePort(EditorDialogueNode editorDialogueNode, Port generatedPort) {
			var targetEdge = edges.ToList().Where(x =>
				x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);


			if (!targetEdge.Any()) {
				editorDialogueNode.outputContainer.Remove(generatedPort);
				editorDialogueNode.RefreshPorts();
				editorDialogueNode.RefreshExpandedState();
				return;
			}

			var edge = targetEdge.First();
			edge.input.Disconnect(edge);
			RemoveElement(targetEdge.First());

			editorDialogueNode.outputContainer.Remove(generatedPort);
			editorDialogueNode.RefreshPorts();
			editorDialogueNode.RefreshExpandedState();
		}

		#endregion
		
	}
}