using UnityEditor.Experimental.GraphView;

namespace VavilichevGD.Editor.Dialogues {
	public class EditorDialogueNode : Node {
		public string GUID;
		public string author;
		public string text;
		public bool entryPoint = false;
	}
}