using System;

namespace VavilichevGD.Attributes {
	public static class ExponentialViewUtility {

		#region CONSTANTS

		public const double MANTISSA_MIN = 1.00d;
		public const double MANTISSA_MAX = 9.99d;
		public const int EXPONENT_MIN = -2;
		public const int EXPONENT_MAX = 307;

		#endregion

		public static readonly double minValue = MANTISSA_MIN * Math.Pow(10, EXPONENT_MIN);
		public static readonly double maxValue = MANTISSA_MAX * Math.Pow(10, EXPONENT_MAX);


		public static double GetRandomBigNumber(double min, double max) {
			var random = new Random();
			var rDouble = random.NextDouble(); 							// between 0 and 1
			var difference = max - min;
			var differenceExponent = difference.GetExponent();
			var rExponent = random.Next(1, differenceExponent);
			var rDoubleFinal = rDouble / Math.Pow(10, rExponent - 1);	// -1 because of rDouble placed between 0 and 1
			var result = rDoubleFinal * difference + min;
			result = Math.Round(result, 2);
			
			return result;
		}

		public static double GetRandomBigNumber() {
			return GetRandomBigNumber(minValue, maxValue);
		}
		
	}
}