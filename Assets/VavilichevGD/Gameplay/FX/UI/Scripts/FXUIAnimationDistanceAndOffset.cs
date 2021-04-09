using System.Collections;
using UnityEngine;

namespace VavilichevGD.FXs {
	public class FXUIAnimationDistanceAndOffset : FXUIAnimation {

		[SerializeField] private AnimationCurve offsetOverLifeTimeY = AnimationCurve.Constant(0f, 1f, 0f);
		[SerializeField] private AnimationCurve offsetOverLifeTimeX = AnimationCurve.Constant(0f, 1f, 0f);
		[SerializeField] private AnimationCurve distanceLerp = AnimationCurve.Constant(0f, 1f, 0f);

		protected override IEnumerator AnimationRoutine(Vector3 startPosition, Transform targetPoint) {
			var timer = 0f;
			var distanceCurve = 0f;

			while (timer < 1f) {
				timer = Mathf.Min(timer + Time.deltaTime / this.duration, 1f);
				distanceCurve = this.distanceLerp.Evaluate(timer);
				var nextPosition = Vector3.Lerp(startPosition, targetPoint.position, distanceCurve);
				nextPosition.x += this.offsetOverLifeTimeX.Evaluate(timer);
				nextPosition.y += this.offsetOverLifeTimeY.Evaluate(timer);

				this.myTransform.position = nextPosition;
				yield return null;
			}
		}
	}
}