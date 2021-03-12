using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace VavilichevGD.Editor.Dialogues {
	public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider {

		private DialogueGraphView graphView;
		private EditorWindow window;
		private Texture2D indentationIcon;

		public void Initialize(EditorWindow window, DialogueGraphView graphView) {
			this.window = window;
			this.graphView = graphView;
			
			indentationIcon = new Texture2D(1, 1);
			indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
			indentationIcon.Apply();
		}
		
		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) {
			var tree = new List<SearchTreeEntry> {
				new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
				new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon)) {
					userData = new EditorDialogueNode(),
					level = 1
				}
			};
			
			return tree;
		}

		public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context) {

			var worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent,
				context.screenMousePosition - window.position.position);
			var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);
			
			switch (SearchTreeEntry.userData) {
				case EditorDialogueNode dialogueNode:
					graphView.CreateNode("Dialogue Text", "Author", localMousePosition);
					return true;
				default:
					return false;
			}
		}
	}
}