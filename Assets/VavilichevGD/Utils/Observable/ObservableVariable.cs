namespace VavilichevGD.Utils.Observable {
	
	public delegate void ObservableVariableChangedEventHandler<in T>(T oldValue, T newValue);
	
	public sealed class ObservableVariable<T> {
		
		public event ObservableVariableChangedEventHandler<T> OnValueChangedEvent;

		private T _value;


		public T value {
			get => _value;
			set {
				if (_value == null || !_value.Equals(value)) {
					var oldValue = _value;
					_value = value;
					OnValueChangedEvent?.Invoke(oldValue, value);
				}
			}
		}

		public ObservableVariable() {
			_value = default;
		}

		public ObservableVariable(T valueByDefault) {
			_value = valueByDefault;
		}

		public override string ToString() {
			return value.ToString();
		}
	}
}
