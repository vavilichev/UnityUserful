namespace VavilichevGD.Gameplay.Dialogues {
	public class Dialogue {
		
		public DialogueInfo info { get; }
		public DialogueState state { get; }
		public DialogueTree dialogueTree { get; }

		public Dialogue(DialogueInfo info) {
			this.info = info;
			this.state = new DialogueState(info.nodeLinks[0].targetNodeGUID);
			this.dialogueTree = new DialogueTree(info);
		}
		
	}
}