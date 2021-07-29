	using System;
	using UnityEngine;

	namespace VavilichevGD.Tools.Numerics {
		public static class BigNumberExtensions {

			public static int GetExponent(this double d) {
				var doubleParts = ExtractScientificNotationParts(d);
				return Convert.ToInt32(doubleParts[1]);
			}

			public static double GetMantissa(this double d, int digits) {
				var doubleParts = ExtractScientificNotationParts(d);
				var mantissa = Convert.ToDouble(doubleParts[0]);
				var roundedMantissa = Math.Round(mantissa, digits);
				return roundedMantissa;
			}

			private static string[] ExtractScientificNotationParts(this double d) {
				var doubleParts = d.ToString(@"E17").Split('E');
				if (doubleParts.Length != 2)
					throw new ArgumentException();

				return doubleParts;
			}

			public static string ToStringFormatted(this double d) {
				return BigNumberTranslator.Translate(d, SystemLanguage.English);
			}

			public static string ToStringTranslated(this double d) {
				return BigNumberTranslator.Translate(d);
			}

		}
	}