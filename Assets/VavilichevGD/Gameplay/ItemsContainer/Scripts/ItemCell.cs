using System;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay {
	public class ItemCell : IItemCell{
		public event Action<ItemCellStateChangeArgs> OnItemCellStateChangedEvent;
		
		public string id { get; }
		public string itemId { get; set; }
		public int itemsAmount { get; set; }
		public int capacity { get; set; }
		
		public bool isFull => !string.IsNullOrWhiteSpace(itemId) && itemsAmount >= capacity;
		public bool isEmpty => string.IsNullOrWhiteSpace(itemId) || itemsAmount == 0;

		public ItemCell(string id) {
			this.id = id;
		}

		public virtual void Clear() {
			itemId = null;
			itemsAmount = 0;
			capacity = 0;
			
			var result = new ItemCellStateChangeArgs();
			result.cellId = id;
			
			OnItemCellStateChangedEvent?.Invoke(result);
		}
		
		public virtual void AddItems(IItem item, int amount, out int remainder) {
			var result = new ItemCellStateChangeArgs();

			result.cellId = id;
			result.itemId = item.id;
			result.itemsAmountOld = itemsAmount;
			result.itemsAmountNew = itemsAmount;

			if (isFull) {
				result.errorText = $"Cell is full";
				result.errorCode = ItemsContainerErrorCode.CellIsFull;
				result.itemsRemainder = amount;
				remainder = amount;
				
				OnItemCellStateChangedEvent?.Invoke(result);
				return;
			}

			if (isEmpty) {
				itemId = item.id;
				capacity = item.maxItemsInCell;
			}
			
			if (itemId != item.id) {
				result.errorText =
					$"You are trying to add item with different id. Item in cel is: {itemId} and you are adding: {item.id}";
				result.errorCode = ItemsContainerErrorCode.CompareDifferentItems;
				result.itemsRemainder = amount;
				remainder = amount;

				OnItemCellStateChangedEvent?.Invoke(result);
				return;
			}

			var sum = itemsAmount + amount;
		
			if (sum > capacity) {
				result.itemsRemainder = sum - capacity;
				itemsAmount = capacity;
			}
			else {
				itemsAmount = sum;
			}
				
			result.itemsAmountNew = itemsAmount;
			remainder = result.itemsRemainder;

			OnItemCellStateChangedEvent?.Invoke(result);
		}

		public virtual void RemoveItems(IItem item, int amount, out int remainder) {
			var result = new ItemCellStateChangeArgs();

			result.cellId = id;
			result.itemId = item.id;
			result.itemsAmountOld = itemsAmount;
			result.itemsAmountNew = itemsAmount;
			result.itemsRemainder = amount;
			remainder = amount;

			if (isEmpty) {
				result.errorText = $"Cell is empty";
				result.errorCode = ItemsContainerErrorCode.CellIsEmpty;
				
				OnItemCellStateChangedEvent?.Invoke(result);
				return;
			}

			if (itemId != item.id) {
				result.errorText =
					$"You are trying to remove item with different id. Item in cel is: {itemId} and you are removing: {item.id}";
				result.errorCode = ItemsContainerErrorCode.CompareDifferentItems;
				
				OnItemCellStateChangedEvent?.Invoke(result);
				return;
			}

			if (itemsAmount < amount) {
				result.errorText =
					$"Not enough items in the cell. Cell has {itemsAmount} items, and you are trying to remove {amount}";
				result.errorCode = ItemsContainerErrorCode.NotEnoughItems;
				result.itemsAmountNew = 0;
				result.itemsRemainder = amount - itemsAmount;
				remainder = result.itemsRemainder;
				
				Clear();
				
				OnItemCellStateChangedEvent?.Invoke(result);
				return;
			}
			
			itemsAmount -= amount;
			
			if (itemsAmount == 0)
				Clear();
				
			result.itemsAmountNew = itemsAmount;
			result.itemsRemainder = 0;
			remainder = 0;

			OnItemCellStateChangedEvent?.Invoke(result);
		}
	}
}