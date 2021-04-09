using System.Collections;
using UnityEngine;

namespace VavilichevGD.FXs {
	public class FXUIAnimationOffset : FXUIAnimation {

		[SerializeField] private AnimationCurve offsetOverLifeTimeY = AnimationCurve.Constant(0f, 1f, 0f);
		[SerializeField] private AnimationCurve offsetOverLifeTimeX = AnimationCurve.Constant(0f, 1f, 0f);

		protected override IEnumerator AnimationRoutine(Vector3 startPosition, Transform targetPoint) {
            
			float timer = 0f;
			while (timer < 1f) {
				timer = Mathf.Min(timer + Time.deltaTime / this.duration, 1f);
				var nextPosition = Vector3.Lerp(startPosition, targetPoint.position, timer);
				nextPosition.x += this.offsetOverLifeTimeX.Evaluate(timer);
				nextPosition.y += this.offsetOverLifeTimeY.Evaluate(timer);

				this.myTransform.position = nextPosition;
				yield return null;
			}
		}
	}
}