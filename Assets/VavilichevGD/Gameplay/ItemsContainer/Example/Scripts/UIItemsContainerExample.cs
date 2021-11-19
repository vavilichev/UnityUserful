using System;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Utils.Observable;

namespace VavilichevGD.Gameplay.Examples {
   public class UIItemsContainerExample : MonoBehaviour {
      [SerializeField] private Button _buttonRefresh;
      [SerializeField] private Button _buttonSave;
      [SerializeField] private Button _buttonAddItem1;
      [SerializeField] private Button _buttonAddItem2;
      [SerializeField] private Button _buttonAddItem1_ToRandomCell;
      [SerializeField] private Button _buttonAddItem2_ToRandomCell;
      [SerializeField] private Button _buttonRemoveItem1;
      [SerializeField] private Button _buttonRemoveItem2;
      [SerializeField] private Button _buttonRemoveItem1_FromRandomCell;
      [SerializeField] private Button _buttonRemoveItem2_FromRandomCell;
      
      [Space]
      [SerializeField] private ItemsContainerExample _itemsContainerExample;
      
      public ObservableVariable<ItemsContainer> itemsContainer { get; } = new ObservableVariable<ItemsContainer>();
      public UIItemCell[] uiItemCells { get; private set; }
      
      private void Awake() {
         uiItemCells = GetComponentsInChildren<UIItemCell>();
      }

      private void OnEnable() {
         if (itemsContainer.value != null) {
            Refresh();
         }

         Subscribe();
      }

      private void OnDisable() {
         Unsubscribe();
      }

      public void SetItemsService(ItemsService itemsService) {
         foreach (var cell in uiItemCells) {
            cell.SetItemsService(itemsService);
         }
      }

      public void Setup(ItemsContainer itemsContainer) {
         this.itemsContainer.value = itemsContainer;

         Refresh();
      }

      private void Subscribe() {
         _buttonRefresh.onClick.AddListener(OnRefreshButtonClick);
         _buttonSave.onClick.AddListener(OnSaveButtonClick);
         _buttonAddItem1.onClick.AddListener(OnAddItem1ButtonClick);
         _buttonAddItem2.onClick.AddListener(OnAddItem2ButtonClick);
         _buttonAddItem1_ToRandomCell.onClick.AddListener(OnAddItem1_ToRandomCellButtonClick);
         _buttonAddItem2_ToRandomCell.onClick.AddListener(OnAddItem2_ToRandomCellButtonClick);
         _buttonRemoveItem1.onClick.AddListener(OnRemoveItem1ButtonClick);
         _buttonRemoveItem2.onClick.AddListener(OnRemoveItem2ButtonClick);
         _buttonRemoveItem1_FromRandomCell.onClick.AddListener(OnRemoveItem1_FromRandomCellButtonClick);
         _buttonRemoveItem2_FromRandomCell.onClick.AddListener(OnRemoveItem2_FromRandomCellButtonClick);
      }

      private void Unsubscribe() {
         _buttonRefresh.onClick.RemoveListener(OnRefreshButtonClick);
         _buttonSave.onClick.RemoveListener(OnSaveButtonClick);
         _buttonAddItem1.onClick.RemoveListener(OnAddItem1ButtonClick);
         _buttonAddItem2.onClick.RemoveListener(OnAddItem2ButtonClick);
         _buttonAddItem1_ToRandomCell.onClick.RemoveListener(OnAddItem1_ToRandomCellButtonClick);
         _buttonAddItem2_ToRandomCell.onClick.RemoveListener(OnAddItem2_ToRandomCellButtonClick);
         _buttonRemoveItem1.onClick.RemoveListener(OnRemoveItem1ButtonClick);
         _buttonRemoveItem2.onClick.RemoveListener(OnRemoveItem2ButtonClick);
         _buttonRemoveItem1_FromRandomCell.onClick.RemoveListener(OnRemoveItem1_FromRandomCellButtonClick);
         _buttonRemoveItem2_FromRandomCell.onClick.RemoveListener(OnRemoveItem2_FromRandomCellButtonClick);
      }

      private void Refresh() {
         var allCells = itemsContainer.value.itemCells;

         if (uiItemCells.Length != allCells.Length)
            throw new Exception("Amount of UI cells must equals amount of cells");

         var amount = uiItemCells.Length;
         for (int i = 0; i < amount; i++) {
            var cell = allCells[i];
            var uiCell = uiItemCells[i];
            uiCell.SetItemCel(cell);
            uiCell.Refresh();
         }
      }
      
      private void OnRemoveItem2_FromRandomCellButtonClick() {
         _itemsContainerExample.RemoveItem2_FromRandomCell();
      }

      private void OnRemoveItem1_FromRandomCellButtonClick() {
         _itemsContainerExample.RemoveItem1_FromRandomCell();
      }

      private void OnRemoveItem2ButtonClick() {
         _itemsContainerExample.RemoveItem2();  
      }

      private void OnRemoveItem1ButtonClick() {
         _itemsContainerExample.RemoveItem1();  
      }
      
      private void OnAddItem2_ToRandomCellButtonClick() {
         _itemsContainerExample.AddItem2_ToRandomCell();
      }

      private void OnAddItem1_ToRandomCellButtonClick() {
         _itemsContainerExample.AddItem1_ToRandomCell();
      }

      private void OnAddItem2ButtonClick() {
         _itemsContainerExample.AddItem2();
      }

      private void OnAddItem1ButtonClick() {
         _itemsContainerExample.AddItem1();
      }

      private void OnSaveButtonClick() {
         _itemsContainerExample.Save();
      }

      private void OnRefreshButtonClick() {
         _itemsContainerExample.RegenerateItemsContainer();
      }
   }
}