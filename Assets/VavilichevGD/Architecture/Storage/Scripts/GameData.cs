using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VavilichevGD.Architecture.StorageSystem {
	[Serializable]
	public sealed class GameData {
		
		// ======== EXAMPLE REPLACE YOUR OWN DATA PLEASE

		public int version;
		public int speed;
		public int valueInt;
		public float valueFloat;
		public Vector3 valueVector3;
		public Vector2 valueVector2;

		public GameData() {
			// It is necessary to create values by default in constructor.
			this.valueInt = Random.Range(0, 10);
			this.valueFloat = Random.Range(0f, 10f);
			this.valueVector3 = Vector3.down;
			this.valueVector2 = Vector2.up;
		}
		
		// ======= END OF EXAMPLE

	}
}