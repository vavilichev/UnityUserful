using UnityEditor;
using UnityEngine;

namespace VavilichevGD.Utils.Editor {
	public static class EditorHelper {
		
		public static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject {
			var loadedAsset = AssetDatabase.LoadAssetAtPath<T>(path);
			if (loadedAsset)
			{
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = loadedAsset;
				return loadedAsset;
			}

			var createdAsset = ScriptableObject.CreateInstance<T>();

			AssetDatabase.CreateAsset(createdAsset, path);
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = createdAsset;
			return createdAsset;
		}
		
	}
}