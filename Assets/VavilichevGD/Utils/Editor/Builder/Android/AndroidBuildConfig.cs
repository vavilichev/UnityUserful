using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VavilichevGD.Utils.Editor.Builder {
	public class AndroidBuildConfig : ScriptableObject {

		[SerializeField] private string m_keyStorePassword = "KeyStorePassword";
		[SerializeField] private string m_keyAliasPassword = "KeyAliasPassword";
		[SerializeField] private List<Object> m_scenes;

		public string keyStorePassword => this.m_keyStorePassword;
		public string keyAliasPassword => this.m_keyAliasPassword;
		public List<Object> scenes => this.m_scenes;

		

		public string[] GetScenesPaths() {
			var scenePaths = new List<string>();
			foreach (var scene in scenes) {
				var path = AssetDatabase.GetAssetPath(scene);
				scenePaths.Add(path);
			}

			return scenePaths.ToArray();
		}
		
		
		private void OnValidate() {
			foreach (var scene in this.m_scenes) {
				if (scene != null && !(scene is SceneAsset)) {
					this.m_scenes.Remove(scene);
					Debug.Log($"You cannot add {scene.GetType().Name}, because it is not a scene.");
					break;
				}
			}
		}
	}
}