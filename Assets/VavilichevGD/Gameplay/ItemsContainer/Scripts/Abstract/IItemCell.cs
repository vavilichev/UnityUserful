using System;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay {
	public interface IItemCell {
		event Action<ItemCellStateChangeArgs> OnItemCellStateChangedEvent;
		
		string id { get; }
		string itemId { get; set; }
		int itemsAmount { get; set; }
		int capacity { get; set; }
		bool isFull { get; }
		bool isEmpty { get; }

		void Clear();

		void AddItems(IItem item, int amount, out int remainder);

		void RemoveItems(IItem item, int amount, out int remainder);
	}
}