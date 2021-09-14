using System;
using UnityEngine;

namespace VavilichevGD.Utils.Timing {
	public class TimeInvoker : MonoBehaviour {

		#region EVENTS

		public event Action<float> OnUpdateTimeTickedEvent;
		public event Action<float> OnUpdateTimeUnscaledTickedEvent; 
		public event Action OnOneSecondTickedEvent;
		public event Action OnOneSecondUnscaledTickedEvent;

		#endregion

		public static TimeInvoker instance {
			get {
				if (_instance == null) {
					var go = new GameObject("[TIME INVOKER]");
					_instance = go.AddComponent<TimeInvoker>();
				}

				return _instance;
			}
		}
		private static TimeInvoker _instance;
		
		private float _oneSecTimer;
		private float _oneSecUnscaledTimer;
		
		private void Update() {
			var deltaTimer = Time.deltaTime;
			OnUpdateTimeTickedEvent?.Invoke(deltaTimer);
			
			_oneSecTimer += deltaTimer;
			if (_oneSecTimer >= 1f) {
				_oneSecTimer -= 1f;
				OnOneSecondTickedEvent?.Invoke();
			}

			var unscaledDeltaTimer = Time.unscaledDeltaTime;
			OnUpdateTimeUnscaledTickedEvent?.Invoke(Time.unscaledDeltaTime);
			_oneSecUnscaledTimer += unscaledDeltaTimer;
			if (_oneSecUnscaledTimer >= 1f) {
				_oneSecUnscaledTimer -= 1f;
				OnOneSecondUnscaledTickedEvent?.Invoke();
			}
		}
	}
}