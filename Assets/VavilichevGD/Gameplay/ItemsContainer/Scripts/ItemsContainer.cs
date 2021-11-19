using System;
using System.Linq;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay {
	public class ItemsContainer : IItemsContainer {
		public event Action<ItemCellStateChangeArgs> OnItemsContainerStateChangedEvent;
		
		public string id { get; }
		public IItemCell[] itemCells { get; }

		public ItemsContainer(string id, IItemCell[] itemCells) {
			this.id = id;
			this.itemCells = itemCells;

			Subscribe();
		}

		public IItemCell GetCellData(string cellId) {
			return itemCells.FirstOrDefault(c => c.id == cellId);
		}

		public bool HasEnoughItems(string itemId, int requiredItemsCount) {
			var itemsCount = GetItemAmount(itemId);
			return itemsCount >= requiredItemsCount;
		}

		public int GetItemAmount(string itemId) {
			var sum = 0;
			var allCellsWithSameItem = itemCells.Where(c => c.itemId == itemId);

			foreach (var cellData in allCellsWithSameItem) {
				sum += cellData.itemsAmount;
			}

			return sum;
		}
		
		public void AddItems(Item item, int amount, Action<ItemCellStateChangeArgs> callback) {
			AddItemsToAnyCellWithSameItem(item, amount);
		}

		private void AddItemsToAnyCellWithSameItem(Item item, int amount) {
			var cellsWithTheSameItem = itemCells.Where(cellData => !cellData.isFull && cellData.itemId == item.id).ToArray();
			var cellsCount = cellsWithTheSameItem.Length;
			var amountToAdd = amount;

			if (cellsCount == 0) {
				AddToAnyEmptyCell(item, amount);
				return;
			}

			for (int i = 0; i < cellsCount; i++) {
				var cellData = cellsWithTheSameItem[i];

				cellData.AddItems(item, amountToAdd, out var remainder);

				amountToAdd = remainder;
				if (remainder == 0) {
					break;
				}
			}

			if (amountToAdd > 0) {
				AddToAnyEmptyCell(item, amountToAdd);				
			}
		}

		private void AddToAnyEmptyCell(Item item, int amount) {
			var emptyCells = itemCells.Where(cellData => cellData.isEmpty).ToArray();
			var cellsCount = emptyCells.Length;
			var amountToAdd = amount;
			
			if (cellsCount == 0) {
				var result = new ItemCellStateChangeArgs {
					error = $"There is no empty cells",
					itemId = item.id,
					itemsRemainder = amount
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
				return;
			}
			
			for (int i = 0; i < cellsCount; i++) {
				var cellData = emptyCells[i];

				cellData.AddItems(item, amountToAdd, out var remainder);
				
				amountToAdd = remainder;
				if (remainder == 0) {
					break;
				}
			}
			
			if (amountToAdd > 0) {
				var result = new ItemCellStateChangeArgs {
					error = $"Not all of items can be placed in container. Extra items amount: {amountToAdd}",
					itemId = item.id,
					itemsRemainder = amountToAdd
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}
		
		public void AddItemsToCell(Item item, int amount, string cellId) {
			var cellData = itemCells.FirstOrDefault(c => c.id == cellId);
			var result = new ItemCellStateChangeArgs {
				cellId = cellId,
				itemId = item.id,
			};
			
			if (cellData != null) {

				if (!cellData.isEmpty) {
					if (cellData.itemId != item.id) {
						result.itemsRemainder = amount;
						result.error = $"Cell contains another item: Cell contains {cellData.itemId}, you trying to add: {item.id}. Cell: {cellData.id}";
					
						OnItemsContainerStateChangedEvent?.Invoke(result);
						return;
					}

					if (cellData.isFull) {
						result.itemsRemainder = amount;
						result.error = $"Cell is full {cellData.id}";

						OnItemsContainerStateChangedEvent?.Invoke(result);
					}
				}
				
				cellData.AddItems(item, amount, out var remainder);
				result.itemsRemainder = remainder;
			}
			else {
				result.itemsRemainder = amount;
				result.error = $"There is no cell with id: {cellId}";
				
				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}

		public void RemoveItems(Item item, int amount, out int remainder) {
			remainder = amount;
			
			if (!HasEnoughItems(item.id, amount)) {
				var result = new ItemCellStateChangeArgs {
					itemId = item.id,
					itemsRemainder = amount,
					error = $"You do not have enough items. Required: {amount}"
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
				return;
			}
			
			var cellsWithTheSameItem = itemCells.Where(cellData => cellData.itemId == item.id).ToArray();
			var cellsCount = cellsWithTheSameItem.Length;
			var amountToRemove = amount;

			if (cellsCount == 0) {
				var result = new ItemCellStateChangeArgs {
					error = $"There is no cells with item: {item.id}",
					itemId = item.id,
					itemsRemainder = amount
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
				return;
			}

			for (int i = 0; i < cellsCount; i++) {
				
				if (amountToRemove == 0) {
					return;
				}

				var cellData = cellsWithTheSameItem[i];

				cellData.RemoveItems(item, amountToRemove, out remainder);
				amountToRemove = remainder;
				
				if (amountToRemove <= 0)
					return;
			}
		}

		public void RemoveItemsFromCell(Item item, int amount, string cellId, out int remainder) {
			var cellData = itemCells.FirstOrDefault(c => c.id == cellId);
			remainder = amount;
			
			if (cellData != null) {
				cellData.RemoveItems(item, amount, out remainder);
			}
			else {
				var result = new ItemCellStateChangeArgs {
					cellId = cellId,
					itemId = item.id,
					itemsRemainder = amount,
					error = $"There is no cell with id: {cellId}"
				};
				
				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}

		private void Subscribe() {
			foreach (var itemCell in itemCells) {
				itemCell.OnItemCellStateChangedEvent += OnItemCellStateChanged;
			}
		}

		private void OnItemCellStateChanged(ItemCellStateChangeArgs e) {
			OnItemsContainerStateChangedEvent?.Invoke(e);
		}
	}
}