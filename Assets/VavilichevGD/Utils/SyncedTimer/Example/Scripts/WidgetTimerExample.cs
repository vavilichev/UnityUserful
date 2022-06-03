using UnityEngine;
using UnityEngine.UI;

namespace VavilichevGD.Utils.Timing.Example
{
	public class WidgetTimerExample : MonoBehaviour
	{
		[SerializeField] private TimerType _timerType;
		[SerializeField] private float _remainingSeconds;
		[SerializeField] private Button _buttonStart;
		[SerializeField] private Button _buttonPause;
		[SerializeField] private Button _buttonStop;
		[Space] [SerializeField] private Text _textType;
		[SerializeField] private Text _textValue;

		private Color _colorPaused = Color.yellow;
		private Color _colorUnpaused = Color.white;
		private SyncedTimer _timer;

		private void Awake()
		{
			UpdatePauseButtonState();
			UpdateTimerTypeField();

			_textValue.text = $"Value: {_remainingSeconds.ToString()}";
		}

		private void OnEnable()
		{
			_buttonStart.onClick.AddListener(OnStartButtonClick);
			_buttonPause.onClick.AddListener(OnPauseButtonClick);
			_buttonStop.onClick.AddListener(OnStopButtonClick);
		}

		private void OnDisable()
		{
			_buttonStart.onClick.RemoveListener(OnStartButtonClick);
			_buttonPause.onClick.RemoveListener(OnPauseButtonClick);
			_buttonStop.onClick.RemoveListener(OnStopButtonClick);
		}

		private void SubscribeOnTimerEvents()
		{
			_timer.TimerValueChanged += TimerValueChanged;
			_timer.TimerFinished += TimerFinished;
		}

		private void UnsubscribeFromTimerEvents()
		{
			_timer.TimerValueChanged -= TimerValueChanged;
			_timer.TimerFinished -= TimerFinished;
		}

		private void UpdatePauseButtonState()
		{
			if (_timer == null)
			{
				_buttonPause.image.color = _colorUnpaused;
				return;
			}

			var color = _timer.isPaused ? _colorPaused : _colorUnpaused;
			_buttonPause.image.color = color;

			var text = _timer.isPaused ? "Unpause" : "Pause";
			var textField = _buttonPause.GetComponentInChildren<Text>();
			textField.text = text;
		}

		private void UpdateTimerTypeField()
		{
			_textType.text = $"Type: {_timerType.ToString()}";
		}

		private void OnStartButtonClick()
		{
			if (_timer == null)
			{
				_timer = new SyncedTimer(_timerType);
				SubscribeOnTimerEvents();
			}

			UpdateTimerTypeField();
			_timer.Start(_remainingSeconds);
			UpdatePauseButtonState();
		}

		private void OnPauseButtonClick()
		{
			if (_timer == null)
				return;

			if (_timer.isPaused)
				_timer.Unpause();
			else
				_timer.Pause();

			UpdatePauseButtonState();
		}

		private void OnStopButtonClick()
		{
			if (_timer == null)
				return;

			_timer.Stop();
			UpdatePauseButtonState();
		}

		private void TimerFinished()
		{
			_textValue.text = "Value: Finished (0)";
		}

		private void TimerValueChanged(float remainingSeconds, TimeChangingSource timeChangingSource)
		{
			_textValue.text = $"Value: {remainingSeconds.ToString()}";
		}
	}
}