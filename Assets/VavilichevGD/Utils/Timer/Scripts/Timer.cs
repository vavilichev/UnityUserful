using System;
using UnityEngine;

namespace VavilichevGD.Utils.Timing {
	public class Timer {

		#region EVENTS

		public event Action<float> OnTimerValueChangedEvent;
		public event Action OnTimerFinishedEvent; 

		#endregion
		
		
		public TimerType type { get; }
		public bool isPaused { get; private set; }
		public float remainingSeconds { get; private set; }
		

		public Timer(TimerType type) {
			this.type = type;
		}

		public Timer(TimerType type, float seconds) {
			this.type = type;
			
			SetTime(seconds);
		}

		
		
		public void SetTime(float seconds) {
			remainingSeconds = seconds;
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Start() {
			if (System.Math.Abs(remainingSeconds) < Mathf.Epsilon) {
#if DEBUG
				Debug.LogError("TIMER: You are trying start timer with remaining seconds equal 0.");
#endif
				OnTimerFinishedEvent?.Invoke();
			}

			isPaused = false;
			Subscribe();
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Start(float seconds) {
			SetTime(seconds);
			Start();
		}

		public void Pause() {
			isPaused = true;
			
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Unpause() {
			isPaused = false;

			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Stop() {
			Unsubscribe();
			remainingSeconds = 0f;

			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
			OnTimerFinishedEvent?.Invoke();
		}

		
		private void Subscribe() {
			switch (type) {
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent += OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent += OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSecondTickedEvent += OnSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSecondUnscaledTickedEvent += OnSecondTicked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		private void Unsubscribe() {
			switch (type) {
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent -= OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent -= OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSecondTickedEvent -= OnSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSecondUnscaledTickedEvent -= OnSecondTicked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}


		private void CheckFinish() {
			if (remainingSeconds <= 0f) 
				Stop();
			else
				OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}


		#region CALLBACKS

		private void OnTicked(float deltaTime) {
			if (isPaused)
				return;
			
			remainingSeconds -= deltaTime;
			CheckFinish();
		}
		
		private void OnSecondTicked() {
			if (isPaused)
				return;

			remainingSeconds -= 1;
			CheckFinish();
		}

		#endregion
		
	}
}