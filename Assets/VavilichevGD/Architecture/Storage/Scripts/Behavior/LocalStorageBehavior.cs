using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class LocalStorageBehavior : IStorageBehavior {

		#region CONSTANTS

		private readonly string savesDirectory = Application.persistentDataPath + "/saves";
		private const string SAVE_FILE_NAME = "GameSave";

		#endregion

		private string filePath { get; }

		public LocalStorageBehavior() {
			if (!Directory.Exists(savesDirectory)) 
				Directory.CreateDirectory(savesDirectory);
			this.filePath =  $"{savesDirectory}/{SAVE_FILE_NAME}";
		}


		
		
		#region SAVE

		public void Save(object saveData) {
			var file = File.Create(filePath);
			Storage.formatter.Serialize(file, saveData);
			file.Close();
		}

		public void SaveAsync(object saveData, Action callback) {
			var thread = new Thread(() => this.SaveDataTaskThreaded(saveData, callback));
			thread.Start();
		}

		private void SaveDataTaskThreaded(object saveData, Action callback) {
			this.Save(saveData);
			callback?.Invoke();
		}

		#endregion



		#region LOAD

		public object Load(object saveDataByDefault) {
			if (!File.Exists(filePath)) {
				if (saveDataByDefault != null)
					this.Save(saveDataByDefault);
				return saveDataByDefault;
			}

			var file = File.Open(filePath, FileMode.Open);
			var saveData = Storage.formatter.Deserialize(file);
			file.Close();
			return saveData;
		}
		
		public void LoadAsync(Action<object> callback, object saveDataByDefault) {
			var thread = new Thread(() => LoadDataTaskThreaded(callback, saveDataByDefault));
			thread.Start();
		}

		private void LoadDataTaskThreaded(Action<object> callback, object saveDataByDefault) {
			var saveData = this.Load(saveDataByDefault);
			callback?.Invoke(saveData);
		}

		#endregion

	}
}