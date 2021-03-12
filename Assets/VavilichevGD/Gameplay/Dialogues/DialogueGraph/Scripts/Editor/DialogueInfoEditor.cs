using UnityEditor;
using UnityEngine;
using VavilichevGD.Gameplay.Dialogues;

namespace VavilichevGD.Editor.Dialogues {
	[CustomEditor(typeof(DialogueInfo))]
	public class DialogueInfoEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			EditorGUILayout.Space();
			if (GUILayout.Button("Open Graph"))
				this.OpenGraph();
		}

		private void OpenGraph() {
			var dialogueInfo = target as DialogueInfo;
			DialogueGraph.OpenDialogueGraphWindow(dialogueInfo);
		}
	}
}