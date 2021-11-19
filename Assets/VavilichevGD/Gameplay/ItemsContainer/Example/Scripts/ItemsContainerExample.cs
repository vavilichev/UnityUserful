using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Gameplay.Data;
using VavilichevGD.Gameplay.Extensions;

namespace VavilichevGD.Gameplay.Examples {
	public class ItemsContainerExample : MonoBehaviour {

		private const string KEY_ITEMS_CONTAINER = "ItemsContainerKey";
		private const string ITEM_ID_1 = "item_id_example_1";
		private const string ITEM_ID_2 = "item_id_example_2";
		private const int ITEMS_MAX_AMOUNT_IN_CELL = 10;

		[SerializeField] private UIItemsContainerExample _uiItemsContainer;

		private readonly Color colorItem1 = Color.red;
		private readonly Color colorItem2 = Color.blue;
		
		private ItemsContainer _itemsContainer;
		private ItemsService _itemsService;
		
		private void Start() {
			InitItemsService();

			_uiItemsContainer.SetItemsService(_itemsService);

			if (!TryToLoadItemsContainer()) {
				GenerateItemsContainer();
			}

			RefreshUI();
		}
		
		public void Save() {
			var itemsContainerData = _itemsContainer.ToData();
			var itemsContainerDataJson = JsonUtility.ToJson(itemsContainerData);
			
			PlayerPrefs.SetString(KEY_ITEMS_CONTAINER, itemsContainerDataJson);
			Debug.Log($"Items container state saved");
		}

		public void RegenerateItemsContainer() {
			GenerateItemsContainer();		
			RefreshUI();
		}

		public void AddItem1() {
			var item1 = _itemsService.GetItem<ItemExample>(ITEM_ID_1);
			
			_itemsContainer.AddItems(item1, 2, null);
		}

		public void AddItem2() {
			var item2 = _itemsService.GetItem<ItemExample>(ITEM_ID_2);
			
			_itemsContainer.AddItems(item2, 2, null);
		}

		public void AddItem1_ToRandomCell() {
			var item1 = _itemsService.GetItem<ItemExample>(ITEM_ID_1);
			var allCells = _itemsContainer.itemCells;
			var rIndex = Random.Range(0, allCells.Length);
			var rCell = allCells[rIndex];

			_itemsContainer.AddItemsToCell(item1, 1, rCell.id);
		}

		public void AddItem2_ToRandomCell() {
			var item2 = _itemsService.GetItem<ItemExample>(ITEM_ID_2);
			var allCells = _itemsContainer.itemCells;
			var rIndex = Random.Range(0, allCells.Length);
			var rCell = allCells[rIndex];

			_itemsContainer.AddItemsToCell(item2, 1, rCell.id);
		}

		public void RemoveItem1() {
			var item1 = _itemsService.GetItem<ItemExample>(ITEM_ID_1);
			_itemsContainer.RemoveItems(item1, 2, out var remainder);
		}

		public void RemoveItem2() {
			var item2 = _itemsService.GetItem<ItemExample>(ITEM_ID_2);
			_itemsContainer.RemoveItems(item2, 2, out var remainder);
		}

		public void RemoveItem1_FromRandomCell() {
			var item1 = _itemsService.GetItem<ItemExample>(ITEM_ID_1);
			var allCells = _itemsContainer.itemCells;
			var rIndex = Random.Range(0, allCells.Length);
			var rCell = allCells[rIndex];

			_itemsContainer.RemoveItemsFromCell(item1, 1, rCell.id, out var remainder);
		}

		public void RemoveItem2_FromRandomCell() {
			var item2 = _itemsService.GetItem<ItemExample>(ITEM_ID_2);
			var allCells = _itemsContainer.itemCells;
			var rIndex = Random.Range(0, allCells.Length);
			var rCell = allCells[rIndex];

			_itemsContainer.RemoveItemsFromCell(item2, 1, rCell.id, out var remainder);
		}
		
		private void InitItemsService() {
			_itemsService = new ItemsService();
			
			var item1 = new ItemExample(ITEM_ID_1, ITEMS_MAX_AMOUNT_IN_CELL, colorItem1);
			var item2 = new ItemExample(ITEM_ID_2, ITEMS_MAX_AMOUNT_IN_CELL, colorItem2);
			
			_itemsService.Add(item1);
			_itemsService.Add(item2);
		}

		private bool TryToLoadItemsContainer() {
			if (!PlayerPrefs.HasKey(KEY_ITEMS_CONTAINER))
				return false;
			
			var containerDataJson = PlayerPrefs.GetString(KEY_ITEMS_CONTAINER);
			var containerData = JsonUtility.FromJson<ItemsContainerData>(containerDataJson);
			
			_itemsContainer = containerData.FromData();
			
			Debug.Log("ItemsContainer loaded from the prefs.");
			return true;
		}

		private void GenerateItemsContainer() {
			var cells = new List<IItemCell>();
			var uiCellsAmount = _uiItemsContainer.uiItemCells.Length;

			for (int i = 0; i < uiCellsAmount; i++) {
				var isCellEmpty = Random.Range(0, 2) == 0;
				var createdCell = new ItemCell($"cell_id_example_{i}");

				if (!isCellEmpty) {
					var randomItemsAmount = Random.Range(1, 10);
					var itemsIds = new[] {ITEM_ID_1, ITEM_ID_2};
					var randomItemIdIndex = Random.Range(0, itemsIds.Length);
					var randomItemId = itemsIds[randomItemIdIndex];
					var itemData = new Item(randomItemId, ITEMS_MAX_AMOUNT_IN_CELL);
					
					createdCell.itemId = itemData.id;
					createdCell.capacity = itemData.maxItemsInCell;
					createdCell.itemsAmount = randomItemsAmount;
				}
				
				cells.Add(createdCell);
			}
				
			_itemsContainer = new ItemsContainer($"items_container", cells.ToArray());
			Debug.Log("ItemsContainer created.");
		}

		private void RefreshUI() {
			_uiItemsContainer.Setup(_itemsContainer);
		}
	}
}