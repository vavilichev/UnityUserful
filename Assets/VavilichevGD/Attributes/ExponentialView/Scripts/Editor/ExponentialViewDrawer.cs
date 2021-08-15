using System;
using UnityEditor;
using UnityEngine;

namespace VavilichevGD.Attributes {
	[CustomPropertyDrawer(typeof(ExponentialViewAttribute))]
	public class ExponentialViewDrawer : PropertyDrawer {
		
		#region CONSTANTS

		private const string DOUBLE_TYPE = "double";

		#endregion
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			ValidateProperty(property);

			var doubleValue = property.doubleValue;
			var mantissa = doubleValue.GetMantissa(2);
			var exponent = doubleValue.GetExponent();
			
			EditorGUI.BeginProperty(position, label, property);

			// Draw label
			position = EditorGUI.PrefixLabel(position, label);

			// Calculate rects
			var mantissaRect = new Rect(position.x, position.y, position.width - 95, position.height);
			position.x = mantissaRect.position.x + mantissaRect.width + 5f;
			
			var tenRect = new Rect(position.x, position.y, 15, position.height);
			position.x = tenRect.position.x + tenRect.width + 5f;

			var exponentRect = new Rect(position.x, position.y, position.width - mantissaRect.width - tenRect.width - 10, position.height);

			// Draw fields - pass GUIContent.none to each so they are drawn without labels
			mantissa = EditorGUI.DoubleField(mantissaRect, mantissa);
			mantissa = ClampMantissa(mantissa, ExponentialViewUtility.MANTISSA_MIN, ExponentialViewUtility.MANTISSA_MAX);

			EditorGUI.LabelField(tenRect, "+E");
			exponent = EditorGUI.IntField(exponentRect, exponent);
			exponent = Mathf.Clamp(exponent, ExponentialViewUtility.EXPONENT_MIN, ExponentialViewUtility.EXPONENT_MAX);


			EditorGUI.EndProperty();
			
			property.doubleValue = mantissa * Math.Pow(10, exponent);
			property.serializedObject.ApplyModifiedProperties();
		}

		private void ValidateProperty(SerializedProperty property) {
			var type = property.type;
			if (type != DOUBLE_TYPE)
				throw new Exception($"ExponentialView attribute works only with double value ({property.displayName})");
		}
		
		private double ClampMantissa(double value, double min, double max) {
			if (value == 0d)
				return value;
			
			var clampedValue = value;
			if (clampedValue < min)
				clampedValue = min;
			else if (clampedValue > max) 
				clampedValue = max;
			return clampedValue;
		}
		
	}
}