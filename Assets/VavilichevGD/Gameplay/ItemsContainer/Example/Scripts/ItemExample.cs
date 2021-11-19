using UnityEngine;

namespace VavilichevGD.Gameplay.Examples {
	public class ItemExample : Item {
		public Color color { get;}

		public ItemExample(string id, int maxItemsInCell, Color color) : base(id, maxItemsInCell) {
			this.color = color;
		}
	}
}