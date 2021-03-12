using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Gameplay.Dialogues.Example {
	public class DialogueExample : MonoBehaviour {
		
		[SerializeField] private DialogueInfo dialogueInfo;

		[SerializeField] private Text textFieldAuthor;
		[SerializeField] private Text textFieldSpeech;
		[SerializeField] private UIWidgetDialogueOptionsExample widgetDialogueOptionsExample;

		private Dialogue dialogue;

		private void Start() {
			this.dialogue = new Dialogue(dialogueInfo);

			var firstDialogueNodeId = this.dialogue.state.guidCurrent;
			var firstDialogueNode = this.dialogue.dialogueTree.GetNode(firstDialogueNodeId);
			this.Setup(firstDialogueNode);
			
			this.widgetDialogueOptionsExample.OnOptionClickedEvent += OnOptionExampleClicked;
		}

		private void OnOptionExampleClicked(DialogueOption option) {
			var nextDialogueNodeId = option.targetNodeGuid;
			this.dialogue.state.guidCurrent = nextDialogueNodeId;
			var nextDialogueNode = this.dialogue.dialogueTree.GetNode(nextDialogueNodeId);
			this.Setup(nextDialogueNode);
		}

		private void Setup(DialogueNode dialogueNode) {
			this.textFieldAuthor.text = dialogueNode.author;
			this.textFieldSpeech.text = dialogueNode.text;
			this.widgetDialogueOptionsExample.Setup(dialogueNode.options);
		}

	}
}