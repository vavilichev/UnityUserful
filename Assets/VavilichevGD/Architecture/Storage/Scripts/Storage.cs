using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace VavilichevGD.Architecture.StorageSystem {
	public sealed class Storage {

		#region EVENTS

		public event Action<GameData> OnStorageLoadedEvent;
		public event Action OnStorageSaveStartedEvent;
		public event Action OnStorageSaveCompleteEvent;

		#endregion


		#region STATIC VARIABLES

		public static BinaryFormatter formatter {
			get {
				if (_formatter == null)
					_formatter = CreateBinaryFormatter();
				return _formatter;
			}
		}
		private static BinaryFormatter _formatter;
		
		public static Storage instance {
			get {
				if (_instance == null)
					_instance = new Storage();
				return _instance;
			}
		}
		private static Storage _instance;
		
		public static bool isInitialized => instance.data != null;

		#endregion
		
		
		public GameData data { get; private set; }
		
		
		private IStorageBehavior storageBehavior;

		
		private Storage() {
			this.storageBehavior = new LocalStorageBehavior();
		}
		
		private static BinaryFormatter CreateBinaryFormatter() {
			var createdFormatter = new BinaryFormatter();
			var selector = new SurrogateSelector();
			
			var vector3Surrogate = new Vector3SerializationSurrogate();
			var vector2Surrogate = new Vector2SerializationSurrogate();
			var quaternionSurrogate = new QuaternionSerializationSurrogate();
			
			selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
			selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2Surrogate);
			selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
			
			createdFormatter.SurrogateSelector = selector;

			return createdFormatter;
		}

		
		
		public void Save() {
			this.OnStorageSaveStartedEvent?.Invoke();
			this.storageBehavior.Save(data);
			this.OnStorageSaveCompleteEvent?.Invoke();
		}

		public void SaveAsync(Action callback = null) {
			this.OnStorageSaveStartedEvent?.Invoke();
			this.storageBehavior.SaveAsync(this.data, callback);
			this.OnStorageSaveCompleteEvent?.Invoke();
		}
		
		public void Load() {
			this.data = (GameData) storageBehavior.Load(new GameData());
			this.OnStorageLoadedEvent?.Invoke(this.data);
		}

		public void LoadAsync(Action<GameData> callback) {
			this.storageBehavior.LoadAsync(gameData => {
					this.data = (GameData) gameData;
					callback?.Invoke(this.data);
					this.OnStorageLoadedEvent?.Invoke(this.data);
				},
				new GameData());
		}
	}
}