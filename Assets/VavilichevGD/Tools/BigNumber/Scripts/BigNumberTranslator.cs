using UnityEngine;

namespace VavilichevGD.Tools.Numerics {
    public static class BigNumberTranslator {
        
        public static bool isInitialized => _dictionaryBase != null;

        private static BigNumberDictionaryBase _dictionaryBase;
        private static SystemLanguage _language;

        public static void UpdateDictionary(SystemLanguage newLanguage = SystemLanguage.English) {
            if (newLanguage == _language)
                return;

            _language = newLanguage;
            switch (_language) {
                
                case SystemLanguage.English:
                    _dictionaryBase = new BigNumberDictionaryEn();
                    break;
                
                case SystemLanguage.Russian:
                    _dictionaryBase = new BigNumberDictionaryRu();
                    break;
                
                default:
                    _dictionaryBase = new BigNumberDictionaryEn();
                    break;
                
            }
        }

        public static string Translate(double d) {
            if (!isInitialized)
                UpdateDictionary();
            return _dictionaryBase.Translate(d);
        }

        public static string Translate(double d, SystemLanguage language) {
            UpdateDictionary(language);
            return _dictionaryBase.Translate(d);
        }
        
    }
}