using System;
using System.Collections.Generic;

namespace VavilichevGD.Gameplay.Data {
	[Serializable]
	public class ItemsContainerData {
		public string id;
		public List<ItemCellData> itemCellDatas;

		public ItemsContainerData() {
			itemCellDatas = new List<ItemCellData>();
		}
	}
}