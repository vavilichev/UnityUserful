using System.Collections;
using UnityEngine;

namespace VavilichevGD.FXs {
	public class FXUIAnimationDistance : FXUIAnimation {

		[SerializeField] private AnimationCurve distanceLerp = AnimationCurve.Constant(0f, 1f, 0f);

		protected override IEnumerator AnimationRoutine(Vector3 startPosition, Transform targetPoint) {
			var timer = 0f;
			var distanceCurve = 0f;

			while (timer < 1f) {
				timer = Mathf.Min(timer + Time.deltaTime / this.duration, 1f);
				distanceCurve = this.distanceLerp.Evaluate(timer);
				var nextPosition = Vector3.Lerp(startPosition, targetPoint.position, distanceCurve);

				this.myTransform.position = nextPosition;
				yield return null;
			}
		}
	}
}