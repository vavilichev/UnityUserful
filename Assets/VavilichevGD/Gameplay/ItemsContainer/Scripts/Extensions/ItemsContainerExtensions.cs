using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay.Extensions {
	public static class ItemsContainerExtensions {
		public static ItemsContainerData ToData(this IItemsContainer itemsContainer) {
			var itemsContainerData = new ItemsContainerData();

			itemsContainerData.id = itemsContainer.id;

			var itemsCount = itemsContainer.itemCells.Length;
			for (int i = 0; i < itemsCount; i++) {
				var cell = itemsContainer.itemCells[i];

				itemsContainerData.itemCellDatas.Add(cell.ToData());
			}

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