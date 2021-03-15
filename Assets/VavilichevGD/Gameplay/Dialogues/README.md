# Dialogue System with Graph View
<br>

[Load DialogueSystem.unitypackage](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Gameplay/Dialogues/DialogueSystem.unitypackage)

<img src="https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Gameplay/Dialogues/DialogueGraph_3.png" data-canonical-src="https://gyazo.com/eb5c5741b6a9a16c692170a41a49c858.png" width="650" height="367" />

<br>

To create new Dialogue - just go to **Window/Dialogues/Dialogue Graph**
<img src="https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Gameplay/Dialogues/DialogueGraph_4.png" data-canonical-src="https://gyazo.com/eb5c5741b6a9a16c692170a41a49c858.png" width="457" height="159" />

<br><br>
Simple system that allows you to write your game dialogues with convenient graph tool!
<br><br>
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Gameplay/Dialogues/DialogueGraph_1.png?raw=true)

## Features
- Simple Save & Load (files stored like a scriptable objects);
- Zoom in and out (you can write really big dialogue);
- Flexible code allows you to add your own data to the sistem;
- Easy to use. 

<br><br>
---

#### The scriptable object can be look a little bit weird. But it is okay, the Dialogue system uses GUID as identifier of each dialogue node. No need to read scriptable object in the inspector window. Just click "Open Graph" button.
![](https://github.com/vavilichev/UnityUserful/blob/main/Assets/VavilichevGD/Gameplay/Dialogues/DialogueGraph_2.png?raw=true)

<br><br>
---
#### Watch the example to learn more. Here is the code that used in that example - looks pretty easy, isn't it?

```csharp
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
```
