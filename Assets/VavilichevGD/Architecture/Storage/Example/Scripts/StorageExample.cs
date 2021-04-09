using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace VavilichevGD.Architecture.StorageSystem.Example {
	public class StorageExample : MonoBehaviour {

		[SerializeField] private Button buttonSave;
		[SerializeField] private Button buttonLoad;
		[SerializeField] private Text textLog;

		private bool savingComplete;


		#region LIFECYCLE

		private void Start() {
			Storage.instance.Load();

			var data = Storage.instance.data;
			this.Log($"GameData loaded at start:\n" +
			         $"int = {data.valueInt},\n" +
			         $"float = {data.valueFloat},\n" +
			         $"vector3 = {data.valueVector3},\n" +
			         $"vector2 = {data.valueVector2}", false);
		}

		private void OnEnable() {
			this.buttonSave.onClick.AddListener(this.OnSaveButtonClick);
			this.buttonLoad.onClick.AddListener(this.OnLoadButtonClick);
		}

		private void OnDisable() {
			this.buttonSave.onClick.RemoveListener(this.OnSaveButtonClick);
			this.buttonLoad.onClick.RemoveListener(this.OnLoadButtonClick);
		}

		#endregion
		
	
		
		private void Update() {
			
			if (this.savingComplete) {
				var data = Storage.instance.data;
				this.Log($"GameData saved:\n" +
				         $"int = {data.valueInt},\n" +
				         $"float = {data.valueFloat},\n" +
				         $"vector3 = {data.valueVector3},\n" +
				         $"vector2 = {data.valueVector2}", true);
				this.Log($"Saving complete at: {Time.time}", true);
				this.savingComplete = false;
			}
			
		}

		#region CALLBACKS

		private void OnSaveButtonClick() {
			this.Log($"Saving started: {Time.time}", false);
				
			var data = Storage.instance.data;
			data.valueInt = Random.Range(0, 10);
			data.valueFloat = Random.Range(0f, 10f);
			data.valueVector3 = Vector3.left;;
			data.valueVector2 = Vector2.up;
				
			Storage.instance.SaveAsync(() => {
				// We cannot get Time.time because Unity doesnot support to do this in the side thread. That
				// is why we use a simple flag for logging time.
				this.savingComplete = true;
			});
		}

		private void OnLoadButtonClick() {
			Storage.instance.Load();
			var data = Storage.instance.data;
			this.Log($"GameData loaded:\n" +
			         $"int = {data.valueInt},\n" +
			         $"float = {data.valueFloat},\n" +
			         $"vector3 = {data.valueVector3},\n" +
			         $"vector2 = {data.valueVector2}", false);
		}

		#endregion

		private void Log(string text, bool append) {
			var logText = append ? this.textLog.text + "\n" + text : text;
			this.textLog.text = logText;
		}
		
	}
}