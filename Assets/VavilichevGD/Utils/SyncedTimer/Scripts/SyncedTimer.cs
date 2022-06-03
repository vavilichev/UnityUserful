using System;
using UnityEngine;

namespace VavilichevGD.Utils.Timing
{
	public delegate void TimerValueChangedHandler(float remainingSeconds, TimeChangingSource changingSource);

	public class SyncedTimer
	{
		public event TimerValueChangedHandler TimerValueChanged;
		public event Action TimerFinished;

		public TimerType type { get; }
		public bool isActive { get; private set; }
		public bool isPaused { get; private set; }
		public float remainingSeconds { get; private set; }

		public SyncedTimer(TimerType type)
		{
			this.type = type;
		}

		public SyncedTimer(TimerType type, float seconds)
		{
			this.type = type;

			SetTime(seconds);
		}

		public void SetTime(float seconds)
		{
			remainingSeconds = seconds;
			TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimeForceChanged);
		}

		public void Start()
		{
			if (isActive)
				return;

			if (System.Math.Abs(remainingSeconds) < Mathf.Epsilon)
			{
#if DEBUG
				Debug.LogError("TIMER: You are trying start timer with remaining seconds equal 0.");
#endif
				TimerFinished?.Invoke();
			}

			isActive = true;
			isPaused = false;
			SubscribeOnTimeInvokerEvents();

			TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimerStarted);
		}

		public void Start(float seconds)
		{
			if (isActive)
				return;

			SetTime(seconds);
			Start();
		}

		public void Pause()
		{
			if (isPaused || !isActive)
				return;

			isPaused = true;
			UnsubscribeFromTimeInvokerEvents();

			TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimerPaused);
		}

		public void Unpause()
		{
			if (!isPaused || !isActive)
				return;

			isPaused = false;
			SubscribeOnTimeInvokerEvents();

			TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimerUnpaused);
		}

		public void Stop()
		{
			if (isActive)
			{
				UnsubscribeFromTimeInvokerEvents();
				
				remainingSeconds = 0f;
				isActive = false;
				isPaused = false;

				TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimerFinished);
				TimerFinished?.Invoke();
			}
		}

		private void SubscribeOnTimeInvokerEvents()
		{
			switch (type)
			{
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

		private void UnsubscribeFromTimeInvokerEvents()
		{
			switch (type)
			{
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

		private void CheckFinish()
		{
			if (remainingSeconds <= 0f)
			{
				Stop();
			}
		}

		private void NotifyAboutTimePassed()
		{
			if (remainingSeconds >= 0f)
			{
				TimerValueChanged?.Invoke(remainingSeconds, TimeChangingSource.TimePassed);
			}
		}

		private void OnTicked(float deltaTime)
		{
			remainingSeconds -= deltaTime;
			
			NotifyAboutTimePassed();
			CheckFinish();
		}

		private void OnSyncedSecondTicked()
		{
			remainingSeconds -= 1;
			
			NotifyAboutTimePassed();
			CheckFinish();
		}
	}
}