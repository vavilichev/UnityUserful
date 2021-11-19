using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay.Examples {
	public class UIItemCell : MonoBehaviour {
		[SerializeField] private Image imgIcon;
		[SerializeField] private Text textAmount;

		public IItemCell itemCell { get; private set; }

		private ItemsService _itemsService;

		private void OnDisable() {
			Unsubscribe();
		}

		public void SetItemsService(ItemsService itemsService) {
			_itemsService = itemsService;
		}
		
		public void SetItemCel(IItemCell itemCell) {
			Unsubscribe();
			
			this.itemCell = itemCell;
			this.itemCell.OnItemCellStateChangedEvent += OnItemCellStateChanged;
			
			Refresh();
		}

		public void Refresh() {
			if (!itemCell.isEmpty) {
				var item = _itemsService.GetItem<ItemExample>(itemCell.itemId);
				imgIcon.color = item.color;
				textAmount.text = itemCell.itemsAmount.ToString();
			}
			else {
				Clear();
			}
		}

		public void Clear() {
			imgIcon.color = Color.white;
			textAmount.text = "";
		}

		private void Unsubscribe() {
			if (itemCell != null)
				itemCell.OnItemCellStateChangedEvent -= OnItemCellStateChanged;
		}
		
		private void OnItemCellStateChanged(ItemCellStateChangeArgs e) {
			Refresh();
		}
	}
}