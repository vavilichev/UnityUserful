namespace VavilichevGD.Gameplay.Data {
	public class ItemCellStateChangeArgs {
		public string cellId;
		public string itemId;
		public int itemsAmountOld;
		public int itemsAmountNew;
		public int itemsRemainder;
		public string error;

		public bool success => string.IsNullOrWhiteSpace(error);
	}
}