using System;

namespace VavilichevGD.Gameplay.Data {
	[Serializable]
	public struct ItemCellData {
		public string cellId;
		public string itemId;
		public int itemsAmount;
		public int capacity;
	}
}