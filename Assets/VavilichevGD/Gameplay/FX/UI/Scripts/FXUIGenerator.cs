using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.FXs {
	public abstract class FXUIGenerator : FXGenerator<FXUIObject> {
		#region EVENTS

		public event Action<FXUIGenerator> OnFXReachedTargetEvent;

		#endregion

		[SerializeField] protected Transform transformTarget;
		[Space]
		[SerializeField] protected int packCountMin = 3;
		[SerializeField] protected int packCountMax = 5;
		[SerializeField] protected float timeBetweenParts = 0.1f;


		protected void MakeFX(Vector3 validPosition) {
			this.MakeFXMany(validPosition, 1);
		}

		protected void MakeFXMany(Vector3 validPosition) {
			var rCount = UnityEngine.Random.Range(this.packCountMin, this.packCountMax + 1);
			this.MakeFXMany(validPosition, rCount);
		}

		protected void MakeFXMany(Vector3 validPosition, int count) {
			this.StartCoroutine(this.FXWorkRoutine(validPosition, count));
		}

		protected virtual IEnumerator FXWorkRoutine(Vector3 validPosition, int count) {
			var finishedParts = 0;

			void OnParticleReachedTarget(FXUIAnimation fxuiAnimation) {
				fxuiAnimation.OnAnimationFinishedEvent -= OnParticleReachedTarget;
				fxuiAnimation.gameObject.SetActive(false);
				finishedParts++;
			}
			
			for (int i = 0; i < count; i++) {
				var fx = this.fxPool.GetFreeElement();
				fx.Go(validPosition, this.transformTarget);
				fx.animation.OnAnimationFinishedEvent += OnParticleReachedTarget;
				yield return new WaitForSeconds(this.timeBetweenParts);
			}

			while (finishedParts < count)
				yield return null;
			
			this.OnFXReachedTargetEvent?.Invoke(this);
		}
		
	}
}