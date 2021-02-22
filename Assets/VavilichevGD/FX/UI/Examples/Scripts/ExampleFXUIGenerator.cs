using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.FXs.Example {
	public class ExampleFXUIGenerator : FXUIGenerator {

		[Space]
		[SerializeField] private Button button;
		[SerializeField] private bool manyFXs;

		private void OnEnable() {
			this.button.onClick.AddListener(this.OnClick);
		}

		private void OnDisable() {
			this.button.onClick.RemoveListener(this.OnClick);
		}


		#region CALLBACKS

		private void OnClick() {
			var startPosition = this.transform.position;
			if (this.manyFXs)
				this.MakeFXMany(startPosition);
			else
				this.MakeFX(startPosition);
		}

		#endregion
	}
}
