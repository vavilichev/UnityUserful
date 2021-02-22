using UnityEngine;

namespace VavilichevGD.Utils.Math {
	public static class Math {

		public static int RandomSign() {
			return Random.Range(0, 2) * 2 - 1;
		}
		
	}
}