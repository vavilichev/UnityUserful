namespace VavilichevGD.Gameplay.Data {
	public struct ItemCellStateChangeArgs {
		public string cellId;
		public string itemId;
		public int itemsAmountOld;
		public int itemsAmountNew;
		public int itemsRemainder;
		public string errorText;
		public ItemsContainerErrorCode errorCode;

		public bool success => string.IsNullOrWhiteSpace(errorText);
	}
}