using System;
using UnityEngine;

namespace VavilichevGD.Utils.Timing {
	public class SyncedTimer {

		#region EVENTS

		public event Action<float> OnTimerValueChangedEvent;
		public event Action OnTimerFinishedEvent; 

		#endregion
		
		
		public TimerType type { get; }
		public bool isActive { get; private set; }
		public bool isPaused { get; private set; }
		public float remainingSeconds { get; private set; }
		

		public SyncedTimer(TimerType type) {
			this.type = type;
		}

		public SyncedTimer(TimerType type, float seconds) {
			this.type = type;
			
			SetTime(seconds);
		}

		
		public void SetTime(float seconds) {
			remainingSeconds = seconds;
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Start() {
			if (isActive)
				return;
			
			if (System.Math.Abs(remainingSeconds) < Mathf.Epsilon) {
#if DEBUG
				Debug.LogError("TIMER: You are trying start timer with remaining seconds equal 0.");
#endif
				OnTimerFinishedEvent?.Invoke();
			}

			isActive = true;
			isPaused = false;
			SubscribeOnTimeInvokerEvents();
			
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Start(float seconds) {
			if (isActive)
				return;
			
			SetTime(seconds);
			Start();
		}

		public void Pause() {
			if (isPaused || !isActive)
				return;
			
			isPaused = true;
			UnsubscribeFromTimeInvokerEvents();
			
			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Unpause() {
			if (!isPaused || !isActive)
				return;
			
			isPaused = false;
			SubscribeOnTimeInvokerEvents();

			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
		}

		public void Stop() {
			if (!isActive)
				return;
			
			UnsubscribeFromTimeInvokerEvents();
			remainingSeconds = 0f;
			isActive = false;
			isPaused = false;

			OnTimerValueChangedEvent?.Invoke(remainingSeconds);
			OnTimerFinishedEvent?.Invoke();
		}

		
		private void SubscribeOnTimeInvokerEvents() {
			switch (type) {
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent += OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent += OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSyncedSecondTickedEvent += OnSyncedSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSyncedSecondUnscaledTickedEvent += OnSyncedSecondTicked;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		private void UnsubscribeFromTimeInvokerEvents() {
			switch (type) {
				case TimerType.UpdateTick:
					TimeInvoker.instance.OnUpdateTimeTickedEvent -= OnTicked;
					break;
				case TimerType.UpdateTickUnscaled:
					TimeInvoker.instance.OnUpdateTimeUnscaledTickedEvent -= OnTicked;
					break;
				case TimerType.OneSecTick:
					TimeInvoker.instance.OnOneSyncedSecondTickedEvent -= OnSyncedSecondTicked;
					break;
				case TimerType.OneSecTickUnscaled:
					TimeInvoker.instance.OnOneSyncedSecondUnscaledTickedEvent -= OnSyncedSecondTicked;
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
			remainingSeconds -= deltaTime;
			CheckFinish();
		}
		
		private void OnSyncedSecondTicked() {
			remainingSeconds -= 1;
			CheckFinish();
		}

		#endregion
		
	}
}