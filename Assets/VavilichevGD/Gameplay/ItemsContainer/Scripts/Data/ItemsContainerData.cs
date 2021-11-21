using System;
using System.Collections.Generic;

namespace VavilichevGD.Gameplay.Data {
	[Serializable]
	public struct ItemsContainerData {
		public string id;
		public List<ItemCellData> itemCellDatas;
	}
}