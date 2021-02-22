using System;
using System.Collections;
using UnityEngine;

namespace VavilichevGD.FXs {
	public abstract class FXUIAnimation : MonoBehaviour{
		
		#region EVENTS

		public event Action<FXUIAnimation> OnAnimationFinishedEvent;

		#endregion

		[SerializeField] protected float duration = 1f;
		[SerializeField] protected float animationStartDelay = 0.25f;

		public bool isPlaying { get; private set; }

		protected Transform myTransform;

		protected virtual void Awake() {
			this.myTransform = this.transform;
		}

		public void Play(Vector3 startPosition, Transform targetPoint) {
			if (this.isPlaying)
				throw new Exception("Could not start animation while another one is running");

			this.StartCoroutine(this.AnimationRoutineBase(startPosition, targetPoint));
		}

		private IEnumerator AnimationRoutineBase(Vector3 startPosition, Transform targetPoint) {
			this.isPlaying = true;
            
			yield return new WaitForSeconds(this.animationStartDelay);
			yield return this.StartCoroutine(this.AnimationRoutine(startPosition, targetPoint));
            
			this.isPlaying = false;
			this.OnAnimationFinishedEvent?.Invoke(this);
		}

		protected abstract IEnumerator AnimationRoutine(Vector3 startPosition, Transform targetPoint);
	}
}