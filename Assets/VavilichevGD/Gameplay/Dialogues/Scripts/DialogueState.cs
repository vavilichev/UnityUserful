namespace VavilichevGD.Gameplay.Dialogues {
	public class DialogueState {
		public string guidCurrent;
		public bool isViewed;

		public DialogueState(string firstGuid) {
			this.guidCurrent = firstGuid;
			this.isViewed = false;
		}
	}
}