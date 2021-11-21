using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Gameplay.Data;

namespace VavilichevGD.Gameplay.Examples {
	public class UIItemsContainerLogger : MonoBehaviour {
		[SerializeField] private UIItemsContainerExample _uiItemsContainer;
		[SerializeField] private Text _textLogLine;

		private void OnEnable() {
			_uiItemsContainer.itemsContainer.OnValueChangedEvent += OnItemsContainerChanged;
		}

		private void OnDisable() {
			_uiItemsContainer.itemsContainer.OnValueChangedEvent -= OnItemsContainerChanged;
		}

		private void OnItemsContainerChanged(ItemsContainer oldContainer, ItemsContainer newContainer) {
			if (oldContainer != null) {
				oldContainer.OnItemsContainerStateChangedEvent -= OnItemsContainerStateChanged;
			}

			newContainer.OnItemsContainerStateChangedEvent += OnItemsContainerStateChanged;
		}

		private void OnItemsContainerStateChanged(ItemCellStateChangeArgs e) {
			if (!e.success) {
				_textLogLine.text = $"{e.errorText}";
			}
			else {
				_textLogLine.text =
					$"Changed: item {e.itemId} into cell {e.cellId}. Items in cell before: {e.itemsAmountOld}, and new {e.itemsAmountNew}";
			}
		}
	}
}