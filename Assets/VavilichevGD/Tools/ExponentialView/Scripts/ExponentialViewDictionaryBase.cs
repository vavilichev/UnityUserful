using System;
using System.Globalization;
using UnityEngine;

namespace VavilichevGD.Attributes {
    public abstract class ExponentialViewDictionaryBase {

        protected abstract string[] dictionary { get; }

        
        public string Translate(double d) {
            var exponent = d.GetExponent();
            if (dictionary.Length < exponent / 3) {
                Debug.LogError($"Dictionary doesn't have name for exponent: {exponent}");
                return ReturnSimplifiedDouble(d, exponent);
            }
            
            var rank = Mathf.FloorToInt(exponent / 3f) - 1;
            if (rank < 0) {
                var roundedValue = Math.Round(d, 2);
                return roundedValue.ToString(CultureInfo.InvariantCulture);
            }
            
            
            var mantissa = d.GetMantissa(2);
            if (exponent % 3 != 0)
                mantissa *= Math.Pow(10, exponent % 3);
            
            return $"{mantissa}{dictionary[rank]}";
        }

        private string ReturnSimplifiedDouble(double d, int exponent) {
            var mantissa = d.GetMantissa(2);
            return $"{mantissa}+E{exponent}";
        }
    }
}