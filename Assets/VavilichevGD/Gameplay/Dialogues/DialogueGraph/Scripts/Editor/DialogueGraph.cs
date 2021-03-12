using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VavilichevGD.Gameplay.Dialogues;

namespace VavilichevGD.Editor.Dialogues {
	public class DialogueGraph : EditorWindow {

		#region CONSTANTS

		private const string TITLE_DIALOGUE_GRAPH = "Dialogue Graph";

		#endregion
		
		private DialogueGraphView graphView;
		private GraphSaveUtility saveUtility;

		#region EDITOR

		[MenuItem("Window/Dialogues/Dialogue Graph")]
		public static void OpenDialogueGraphWindow() {
			var window = GetWindow<DialogueGraph>();
			window.Initialize();
		}

		public static void OpenDialogueGraphWindow(DialogueInfo dialogueInfo) {
			var window = GetWindow<DialogueGraph>();
			window.Initialize();
			window.saveUtility.LoadGraph(dialogueInfo);
		}

		#endregion


		#region LIFECYCLE

		public void Initialize() {
			this.titleContent = new GUIContent(TITLE_DIALOGUE_GRAPH);
			
			var gv = this.CreateGraphView();
			this.saveUtility = new GraphSaveUtility(gv);
			this.saveUtility.OnFileNameChangedEvent += this.OnFileNameChanged;

			this.CreateToolbar();
			this.CreateMiniMap();
		}

		private void OnDisable() {
			if (this.saveUtility == null)
				return;
			 
			this.saveUtility.SaveBeforeClose();
			this.saveUtility.OnFileNameChangedEvent -= this.OnFileNameChanged;
			this.rootVisualElement.Remove(this.graphView);
		}

		#endregion

		private DialogueGraphView CreateGraphView() {
			this.graphView = new DialogueGraphView(this) {
				name = TITLE_DIALOGUE_GRAPH
			};

			this.graphView.StretchToParentSize();
			this.rootVisualElement.Add(this.graphView);
			return this.graphView;
		}

		private void CreateToolbar() {
			var toolbar = new Toolbar();
			toolbar.Add(new Button(this.OnSaveButtonClicked) {text = "Save"});
			toolbar.Add(new Button(this.OnSaveAsButtonClicked) {text = "Save As.."});
			toolbar.Add(new Button(this.OnLoadButtonClicked) {text = "Load"});

			rootVisualElement.Add(toolbar);
		}

		private void CreateMiniMap() {
			var miniMap = new MiniMap {anchored = true};
			this.UpdateMiniMapPosition(miniMap);
			this.graphView.Add(miniMap);

			rootVisualElement.RegisterCallback<GeometryChangedEvent>(visualElement => {
				this.UpdateMiniMapPosition(miniMap);
			});
		}

		private void UpdateMiniMapPosition(MiniMap miniMap) {
			var coords = this.graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
			miniMap.SetPosition(new Rect(coords.x, coords.y, 200, 140));
		}

		#region CALLBACKS

		private void OnSaveButtonClicked() {
			this.saveUtility.SaveGraph();
		}

		private void OnSaveAsButtonClicked() {
			this.saveUtility.SaveGraphAs();
		}

		private void OnLoadButtonClicked() {
			this.saveUtility.LoadGraph();
		}
		
		private void OnFileNameChanged(string newFileName) {
			this.titleContent.text = $"{TITLE_DIALOGUE_GRAPH} - {newFileName}";
		}

		#endregion

	}
}
