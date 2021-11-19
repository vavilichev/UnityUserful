namespace VavilichevGD.Gameplay {
	public class Item : IItem {
		public string id { get; }
		public int maxItemsInCell { get; }

		public Item(string id, int maxItemsInCell) {
			this.id = id;
			this.maxItemsInCell = maxItemsInCell;
		}
	}
}