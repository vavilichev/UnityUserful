using System.Collections.Generic;

namespace VavilichevGD.Gameplay.Examples {
	public class ItemsService {
		private Dictionary<string, Item> _itemsMap;

		public ItemsService() {
			_itemsMap = new Dictionary<string, Item>();
		}

		public void Add(Item item) {
			_itemsMap[item.id] = item;
		}

		public void Remove(Item item) {
			if (_itemsMap.ContainsKey(item.id))
				_itemsMap.Remove(item.id);
		}

		public T GetItem<T>(string itemId) where T : Item {
			_itemsMap.TryGetValue(itemId, out var result);

			return result as T;
		}
	}
}