using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Attributes;

public class ExponentialViewExample : MonoBehaviour {

	#region CONSTANTS

	private const string PREF_KEY = "BIG_NUMBER_VALUE";

	#endregion
	
		[SerializeField, ExponentialView] private double minValueForRandom;
		[SerializeField, ExponentialView] private double maxValueForRandom;
		[Space]
		[SerializeField] private Button btnSave;
		[SerializeField] private Button btnLoad;
		[SerializeField] private Button btnRandomize;
		[Space] 
		[SerializeField] private Text textLog;
		

		private double _number;

		#region MONO

		private void OnEnable() {
			btnSave.onClick.AddListener(OnSaveButtonClick);
			btnLoad.onClick.AddListener(OnLoadButtonClick);
			btnRandomize.onClick.AddListener(OnRandomizeButtonClick);
			UpdateTextField();
		}

		private void OnDisable() {
			btnSave.onClick.RemoveListener(OnSaveButtonClick);
			btnLoad.onClick.RemoveListener(OnLoadButtonClick);
			btnRandomize.onClick.RemoveListener(OnRandomizeButtonClick);
		}

		#endregion
		

		private void UpdateTextField() {
			textLog.text = _number.ToStringFormatted();
		}

		private void Save() {
			PlayerPrefs.SetString(PREF_KEY, _number.ToString("r"));
		}

		private void Load() {
			var loadedString = PlayerPrefs.GetString(PREF_KEY, _number.ToString("r"));
			_number = double.Parse(loadedString);
		}

		private void RandomizeNumber() {
			_number = ExponentialViewUtility.GetRandomBigNumber(minValueForRandom, maxValueForRandom);
		}


		#region CALLBACKS

		private void OnSaveButtonClick() {
			Save();
		}
		
		private void OnLoadButtonClick() {
			Load();
			UpdateTextField();
		}
		
		private void OnRandomizeButtonClick() {
			RandomizeNumber();
			UpdateTextField();
		}

		#endregion
		
}