using System.Collections.Generic;

namespace VavilichevGD.Gameplay.Dialogues {
	public class DialogueNode {
		public string guid { get; set; }
		public string author { get; set; }
		public string text { get; set; }
		public List<DialogueOption> options { get; set; } = new List<DialogueOption>();
	}
}