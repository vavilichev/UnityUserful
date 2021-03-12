using System;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Gameplay.Dialogues.Example {
	public class UIWidgetDialogueOptionExample : MonoBehaviour {

		#region EVENTS

		public event Action<DialogueOption> OnClickedEvent;

		#endregion
		
		[SerializeField] private Button button;
		[SerializeField] private Text textOption;
		
		public DialogueOption option { get; private set; }

		public void Setup(DialogueOption option) {
			this.option = option;
			this.textOption.text = option.text;
		}

		private void OnEnable() {
			this.button.onClick.AddListener(this.OnClicked);
		}

		private void OnDisable() {
			this.button.onClick.RemoveListener(this.OnClicked);
		}

		#region CALLBACKS

		private void OnClicked() {
			this.OnClickedEvent?.Invoke(this.option);
		}

		#endregion
		
	}
}