using System;
using System.Collections.Generic;
using System.Linq;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay {
	public class ItemsContainer : IItemsContainer {
		public event Action<ItemCellStateChangeArgs> OnItemsContainerStateChangedEvent;
		
		public string id { get; }
		public IItemCell[] itemCells => _itemCellsMap.Values.ToArray();
		
		protected Dictionary<string, IItemCell> _itemCellsMap { get; }

		public ItemsContainer(string id, IItemCell[] itemCells) {
			this.id = id;

			_itemCellsMap = new Dictionary<string, IItemCell>();
			
			foreach (var itemCell in itemCells) {
				_itemCellsMap[itemCell.id] = itemCell;
			}

			Subscribe();
		}

		public IItemCell GetCellData(string cellId) {
			_itemCellsMap.TryGetValue(cellId, out var result);
			
			return result;
		}

		public virtual bool HasEnoughItems(string itemId, int requiredItemsCount) {
			var itemsCount = GetItemAmount(itemId);
			return itemsCount >= requiredItemsCount;
		}

		public int GetItemAmount(string itemId) {
			var sum = 0;
			var cells = itemCells;

			foreach (var itemCell in cells) {
				if (itemCell.itemId == itemId) {
					sum += itemCell.itemsAmount;
				}
			}

			return sum;
		}
		
		public virtual void AddItems(Item item, int amount, Action<ItemCellStateChangeArgs> callback) {
			AddItemsToAnyCellWithSameItem(item, amount);
		}

		protected virtual void AddItemsToAnyCellWithSameItem(Item item, int amount) {
			var amountToAdd = amount;
			var cells = itemCells;

			foreach (var itemCell in cells) {
				if (itemCell.itemId == item.id) {
					itemCell.AddItems(item, amountToAdd, out var remainder);
					amountToAdd = remainder;
					
					if (remainder == 0) {
						break;
					}
				}
			}

			if (amountToAdd > 0) {
				AddToAnyEmptyCell(item, amountToAdd);				
			}
		}

		protected virtual void AddToAnyEmptyCell(Item item, int amount) {
			var amountToAdd = amount;
			var cells = itemCells;
			
			foreach (var itemCell in cells) {
				if (itemCell.isEmpty) {
					itemCell.AddItems(item, amountToAdd, out var remainder);
					amountToAdd = remainder;
					
					if (remainder == 0) {
						break;
					}
				}
			}
			
			if (amountToAdd > 0) {
				var result = new ItemCellStateChangeArgs {
					errorText = $"Not all of items can be placed in container. Extra items amount: {amountToAdd}",
					itemId = item.id,
					itemsRemainder = amountToAdd
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}
		
		public virtual void AddItemsToCell(Item item, int amount, string cellId) {
			var cellData = GetCellData(cellId);
			var result = new ItemCellStateChangeArgs {
				cellId = cellId,
				itemId = item.id,
			};
			
			if (cellData != null) {

				if (!cellData.isEmpty) {
					if (cellData.itemId != item.id) {
						result.itemsRemainder = amount;
						result.errorText = $"Cell contains another item: Cell contains {cellData.itemId}, you trying to add: {item.id}. Cell: {cellData.id}";
					
						OnItemsContainerStateChangedEvent?.Invoke(result);
						return;
					}

					if (cellData.isFull) {
						result.itemsRemainder = amount;
						result.errorText = $"Cell is full {cellData.id}";

						OnItemsContainerStateChangedEvent?.Invoke(result);
					}
				}
				
				cellData.AddItems(item, amount, out var remainder);
				result.itemsRemainder = remainder;
			}
			else {
				result.itemsRemainder = amount;
				result.errorText = $"There is no cell with id: {cellId}";
				
				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}

		public virtual void RemoveItems(Item item, int amount, out int remainder) {
			remainder = amount;
			
			if (!HasEnoughItems(item.id, amount)) {
				var result = new ItemCellStateChangeArgs {
					itemId = item.id,
					itemsRemainder = amount,
					errorText = $"You do not have enough items. Required: {amount}"
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
				return;
			}
			
			var amountToRemove = amount;
			var cells = itemCells;

			foreach (var itemCell in cells) {
				if (itemCell.itemId == item.id) {
					itemCell.RemoveItems(item, amountToRemove, out remainder);
					amountToRemove = remainder;
				}

				if (amountToRemove <= 0) {
					var result = new ItemCellStateChangeArgs {
						itemId = item.id
					};
					
					OnItemsContainerStateChangedEvent?.Invoke(result);
					return;
				}
			}

			if (amountToRemove == amount) {
				var result = new ItemCellStateChangeArgs {
					errorText = $"There is no cells with item: {item.id}",
					itemId = item.id,
					itemsRemainder = amount
				};

				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}

		public virtual void RemoveItemsFromCell(Item item, int amount, string cellId, out int remainder) {
			var cellData = GetCellData(cellId);
			remainder = amount;
			
			if (cellData != null) {
				cellData.RemoveItems(item, amount, out remainder);
			}
			else {
				var result = new ItemCellStateChangeArgs {
					cellId = cellId,
					itemId = item.id,
					itemsRemainder = amount,
					errorText = $"There is no cell with id: {cellId}"
				};
				
				OnItemsContainerStateChangedEvent?.Invoke(result);
			}
		}

		protected virtual void Subscribe() {
			var cells = itemCells;
			
			foreach (var itemCell in cells) {
				itemCell.OnItemCellStateChangedEvent += OnItemCellStateChanged;
			}
		}

		protected virtual void OnItemCellStateChanged(ItemCellStateChangeArgs e) {
			OnItemsContainerStateChangedEvent?.Invoke(e);
		}
	}
}