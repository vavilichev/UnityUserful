using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace VavilichevGD.Architecture.StorageSystem.Example {
	public class StorageExample : MonoBehaviour {

		[SerializeField] private Button buttonSaveAsync;
		[SerializeField] private Button buttonLoadInstantly;
		[SerializeField] private Button buttonLoadWithRoutine;
		[SerializeField] private Text textLog;

		private bool savingComplete;


		#region LIFECYCLE

		private void Start() {
			Storage.instance.Load();

			var data = Storage.instance.data;
			this.Log($"GameData loaded at start:\n" +
			         $"int = {data.valueInt},\n" +
			         $"speed = {data.speed},\n" +
			         $"float = {data.valueFloat},\n" +
			         $"vector3 = {data.valueVector3},\n" +
			         $"vector2 = {data.valueVector2},\n" +
			         $"version = {data.version}", false);

			if (data.version < 2) {
				data.version = 2;
				data.speed = 100;
				Debug.Log("New version. Speed changed tp 100");
			}
		}

		private void OnEnable() {
			this.buttonSaveAsync.onClick.AddListener(this.OnSaveAsyncButtonClick);
			this.buttonLoadInstantly.onClick.AddListener(this.OnLoadInstantlyButtonClick);
			this.buttonLoadWithRoutine.onClick.AddListener(this.OnLoadWithRoutineButtonClick);
		}

		private void OnDisable() {
			this.buttonSaveAsync.onClick.RemoveListener(this.OnSaveAsyncButtonClick);
			this.buttonLoadInstantly.onClick.RemoveListener(this.OnLoadInstantlyButtonClick);
			this.buttonLoadWithRoutine.onClick.RemoveListener(this.OnLoadWithRoutineButtonClick);
		}

		#endregion
		
	
		
		private void Update() {
			
			if (this.savingComplete) {
				var data = Storage.instance.data;
				this.Log($"GameData saved asynchronous:\n" +
				         $"int = {data.valueInt},\n" +
				         $"speed = {data.speed},\n" +
				         $"float = {data.valueFloat},\n" +
				         $"vector3 = {data.valueVector3},\n" +
				         $"vector2 = {data.valueVector2},\n" + 
						 $"version = {data.version}", true);

				this.Log($"Saving complete at: {Time.time}", true);
				this.savingComplete = false;
			}
			
		}

		#region CALLBACKS

		private void OnSaveAsyncButtonClick() {
			this.Log($"Saving started: {Time.time}", false);
				
			var data = Storage.instance.data;
			data.valueInt = Random.Range(0, 10);
			data.valueFloat = Random.Range(0f, 10f);
			data.valueVector3 = Vector3.left;;
			data.valueVector2 = Vector2.up;
			data.version = 2;
				
			Storage.instance.SaveAsync(() => {
				// We cannot get Time.time because Unity doesnot support to do this in the side thread. That
				// is why we use a simple flag for logging time.
				this.savingComplete = true;
			});
		}

		private void OnLoadInstantlyButtonClick() {
			Storage.instance.Load();
			var data = Storage.instance.data;
			this.Log($"GameData loaded instantly:\n" +
			         $"int = {data.valueInt},\n" +
			         $"speed = {data.speed},\n" +
			         $"float = {data.valueFloat},\n" +
			         $"vector3 = {data.valueVector3},\n" +
			         $"vector2 = {data.valueVector2},\n" + 
					 $"version = {data.version}", false);
		}
		
		private void OnLoadWithRoutineButtonClick() {
			Storage.instance.LoadWithRoutine(loadedData => {
				var data = loadedData;
				this.Log($"GameData loaded with routine:\n" +
				         $"int = {data.valueInt},\n" +
				         $"speed = {data.speed},\n" +
				         $"float = {data.valueFloat},\n" +
				         $"vector3 = {data.valueVector3},\n" +
				         $"vector2 = {data.valueVector2},\n" +
				         $"version = {data.version}", false);
			});
		}

		#endregion

		private void Log(string text, bool append) {
			var logText = append ? this.textLog.text + "\n" + text : text;
			this.textLog.text = logText;
		}
		
	}
}