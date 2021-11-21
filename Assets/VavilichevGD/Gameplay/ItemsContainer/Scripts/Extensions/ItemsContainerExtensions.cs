using System.Collections.Generic;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay.Extensions {
	public static class ItemsContainerExtensions {
		public static ItemsContainerData ToData(this IItemsContainer itemsContainer) {
			var itemCellDatas = new List<ItemCellData>();
			var itemsCount = itemsContainer.itemCells.Length;
			
			for (int i = 0; i < itemsCount; i++) {
				var cell = itemsContainer.itemCells[i];

				itemCellDatas.Add(cell.ToData());
			}

			var itemsContainerData = new ItemsContainerData {
				id = itemsContainer.id,
				itemCellDatas = itemCellDatas 
			};

			return itemsContainerData;
		}

		public static ItemsContainer FromData(this ItemsContainerData itemsContainerData) {
			var containerId = itemsContainerData.id;
			var itemsAmount = itemsContainerData.itemCellDatas.Count;
			var items = new ItemCell[itemsAmount];

			for (int i = 0; i < itemsAmount; i++) {
				items[i] = itemsContainerData.itemCellDatas[i].FromData();
			}

			return new ItemsContainer(containerId, items);
		}
	}
}