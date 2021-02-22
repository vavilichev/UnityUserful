using UnityEngine;
using VavilichevGD.Utils;

namespace VavilichevGD.FXs {
	public abstract class FXGenerator<T> : MonoBehaviour where T : FXObject {
		
		[SerializeField] protected T prefab;
		[SerializeField] protected int poolCount = 3;
		[SerializeField] protected bool autoExpand = false;
        
		protected PoolMono<T> fxPool;


		protected virtual void Awake() {
			this.InitFXPool();
		}

		protected virtual void InitFXPool() {
			var myTransform = this.transform;
			this.fxPool = new PoolMono<T>(this.prefab, this.poolCount, myTransform);
			this.fxPool.autoExpand = this.autoExpand;
		}
        
	}
}