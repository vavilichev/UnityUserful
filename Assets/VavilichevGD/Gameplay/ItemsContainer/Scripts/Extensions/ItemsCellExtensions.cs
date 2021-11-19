using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay.Extensions {
	public static class ItemsCellExtensions {
		public static ItemCellData ToData(this IItemCell itemCell) {
			var itemsCellData = new ItemCellData();

			itemsCellData.cellId = itemCell.id;
			itemsCellData.itemId = itemCell.itemId;
			itemsCellData.itemsAmount = itemCell.itemsAmount;
			itemsCellData.capacity = itemCell.capacity;

			return itemsCellData;
		}

		public static ItemCell FromData(this ItemCellData data) {
			var itemCell = new ItemCell(data.cellId);

			itemCell.itemId = data.itemId;
			itemCell.itemsAmount = data.itemsAmount;
			itemCell.capacity = data.capacity;

			return itemCell;
		}
	}
}