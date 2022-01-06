using UnityEditor;
using UnityEngine;

namespace VavilichevGD.Utils.Attributes.GameObjectOfType {
	[CustomPropertyDrawer(typeof(GameObjectOfTypeAttribute))]
	public class GameObjectOfTypeDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			// Show error if property type is not a reference or source object is not a game object
			if (property.propertyType != SerializedPropertyType.ObjectReference || !property.type.Contains($"{nameof(GameObject)}")) {
				DrawErrorOfType(position);
				return;
			}
			
			var gootAttribute = attribute as GameObjectOfTypeAttribute;

			// Handle the dragging objects
			var draggedObjectsCount = DragAndDrop.objectReferences.Length;
			if (draggedObjectsCount > 0) {
				
				for (int i = 0; i < draggedObjectsCount; i++) {
					var draggedObject = DragAndDrop.objectReferences[i] as GameObject;
					
					if (draggedObject == null || (draggedObject != null && draggedObject.GetComponent(gootAttribute.type) == null)) {
						DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
					}
				}
			}

			// If wrong type was putted into the property field it should be cleaned
			if (property.objectReferenceValue != null) {
				var go = property.objectReferenceValue as GameObject;
				
				if (go != null && go.GetComponent(gootAttribute.type) == null) {
					property.objectReferenceValue = null;
				}
			}

			var modifiedLabel = $"{label.text} ({gootAttribute.type.Name})";
			property.objectReferenceValue = EditorGUI.ObjectField(position, modifiedLabel, property.objectReferenceValue, typeof(GameObject), true);
		}
		
		private void DrawErrorOfType(Rect position)
		{
			EditorGUI.HelpBox(position, $"{nameof(GameObjectOfTypeAttribute)} works only with {nameof(GameObject)} type", MessageType.Error);
		}
	}
}
