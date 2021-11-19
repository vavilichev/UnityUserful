using System;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay {
	public interface IItemsContainer {
		event Action<ItemCellStateChangeArgs> OnItemsContainerStateChangedEvent;

		string id { get; }
		IItemCell[] itemCells { get; }

		IItemCell GetCellData(string cellId);
		
		bool HasEnoughItems(string itemId, int requiredItemsCount);
		
		int GetItemAmount(string itemId);
		
		void AddItems(Item item, int amount, Action<ItemCellStateChangeArgs> callback);
		
		void AddItemsToCell(Item item, int amount, string cellId);
		
		void RemoveItems(Item item, int amount, out int remainder);
		
		void RemoveItemsFromCell(Item item, int amount, string cellId, out int remainder);
	}
}