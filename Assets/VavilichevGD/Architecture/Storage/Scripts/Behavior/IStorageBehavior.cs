using System;

namespace VavilichevGD.Architecture.StorageSystem {
	public interface IStorageBehavior {
		void Save(object saveData);
		void SaveAsync(object saveData, Action callback);
		object Load(object saveDataByDefault);
		void LoadAsync(Action<object> callback, object saveDataByDefault);
	}
}