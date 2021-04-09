using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Gameplay.Dialogues.Example {
	public class UIWidgetDialogueOptionsExample : MonoBehaviour {

		#region EVENTS

		public event Action<DialogueOption> OnOptionClickedEvent;

		#endregion

		[SerializeField] private List<UIWidgetDialogueOptionExample> widgetsOptions;
		[SerializeField] private Button buttonCancel;
		[SerializeField] private Button buttonSubscribe;

		
		private void OnEnable() {
			foreach (var uiOption in this.widgetsOptions) 
				uiOption.OnClickedEvent += this.OnOptionClicked;
			
			this.buttonCancel.onClick.AddListener(this.OnCancelButtonClicked);
			this.buttonSubscribe.onClick.AddListener(this.OnSubscribeOnTwitterButtonClicked);
		}
		
		private void OnDisable() {
			foreach (var uiOption in this.widgetsOptions) 
				uiOption.OnClickedEvent -= this.OnOptionClicked;
			
			this.buttonCancel.onClick.RemoveListener(this.OnCancelButtonClicked);
			this.buttonSubscribe.onClick.RemoveListener(this.OnSubscribeOnTwitterButtonClicked);
		}


		public void Setup(List<DialogueOption> options) {
			this.DeactivateAll();

			for (int i = 0; i < options.Count; i++) {
				widgetsOptions[i].gameObject.SetActive(true);
				widgetsOptions[i].Setup(options[i]);
			}
			
			this.buttonCancel.gameObject.SetActive(options.Count == 0);
			this.buttonSubscribe.gameObject.SetActive(options.Count == 0);
		}

		private void DeactivateAll() {
			foreach (var uiOption in widgetsOptions) 
				uiOption.gameObject.SetActive(false);
			this.buttonCancel.gameObject.SetActive(false);
			this.buttonSubscribe.gameObject.SetActive(false);
		}

		
		#region CALLBACKS

		private void OnOptionClicked(DialogueOption option) {
			this.OnOptionClickedEvent?.Invoke(option);
		}

		private void OnCancelButtonClicked() {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
		}

		private void OnSubscribeOnTwitterButtonClicked() {
			Application.OpenURL("https://twitter.com/vavilichevgd");
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
		}

		#endregion
		
	}
}