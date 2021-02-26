using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
#if RESOLVER_EXISTS
using GooglePlayServices;
#endif

namespace VavilichevGD.Utils.Editor.Builder {
    public static class AndroidBuildHelper {
        
#if UNITY_ANDROID
        
        #region Const

        private const string CONFIG_PATH = "Assets/VavilichevGD/Utils/Editor/Builder/Android/Config/AndroidBuildConfig.asset";
        private const string NAMESPACE_RESOLVER = "GooglePlayServices";
        private const string SYMBOL_RESOLVER_EXISTS = "RESOLVER_EXISTS";
        private const BuildOptions OPTIONS_DEVELOPMENT = BuildOptions.Development | BuildOptions.AutoRunPlayer;
        private const BuildOptions OPTIONS_NONE = BuildOptions.None;
        private const BuildOptions OPTIONS_AUTO_RUN = BuildOptions.None;
        #endregion

        
        
        #region CHECK ANDROID DESOLVER

        [InitializeOnLoadMethod]
        private static void CheckPlayServicesResolver() {
            if (!NamespaceExists(NAMESPACE_RESOLVER))
                return;
            
            var definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split ( ';' ).ToList ();
            
            if (allDefines.Contains(SYMBOL_RESOLVER_EXISTS))
                return;
            
            allDefines.Add(SYMBOL_RESOLVER_EXISTS);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }
        
        
        public static bool NamespaceExists(string desiredNamespace)
        {
            foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach(Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }

        #endregion
        
        
        
        
        #region Buttons
        
        [MenuItem("Builds/Settings", false, 999)]
        public static void SelectSettings() {
            EditorHelper.LoadOrCreateAsset<AndroidBuildConfig>(CONFIG_PATH);
        }

        #region DEVELOPMENT

        [MenuItem("Builds/Build and Run Development", false, 0)]
        public static void RunDevelopmentBuildAndroid() {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(
                BuildTargetGroup.Android,
                ScriptingImplementation.Mono2x
            );
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            ResolveAndBuild(OPTIONS_DEVELOPMENT, true);
        }

        #endregion

        #region APK

        [MenuItem("Builds/Build and Run Release APK", false, 1)]
        public static void BuildAndRunAPK() {
            BuildAPK(OPTIONS_AUTO_RUN);
        }

        [MenuItem("Builds/Build Release APK", false, 2)]
        public static void BuildAPK() {
            BuildAPK(OPTIONS_NONE);
        }

        public static void BuildAPK(BuildOptions options) {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,
                ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            ResolveAndBuild(options, false);
        }

        #endregion


        #region AAB

        [MenuItem("Builds/Build Release AAB", false, 3)]
        public static void BuildAAB() {
            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,
                ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures =
                AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.bundleVersionCode++;
            
            ResolveAndBuild(OPTIONS_NONE, true);
        }

        #endregion
       
        #endregion


        private static void ResolveAndBuild(BuildOptions options, bool aabBundle, bool devBuild = false) {
            if (!aabBundle)
                UpdateVersion();
            
#if RESOLVER_EXISTS
            PlayServicesResolver.Resolve(() => {
                var filePath = GetAndroidBuildPath(aabBundle, devBuild);
                Build(options, filePath);
            });
#else
            var filePath = GetAndroidBuildPath(aabBundle, devBuild);
            Build(options, filePath);
#endif
        }

        #region UPDATE VERSION

        private static void UpdateVersion() {
            var version = GetCurrentVersion();
            var newVersion = System.Math.Round(version + 0.01f, 2);
            SetNewVersion(newVersion);
        }

        private static double GetCurrentVersion() {
            var ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(PlayerSettings.bundleVersion, NumberStyles.Any, ci);
        }

        private static void SetNewVersion(double newVersion) {
            PlayerSettings.bundleVersion =
                newVersion.ToString("0.00", CultureInfo.InvariantCulture);
        }

        #endregion
        
        
        private static string GetAndroidBuildPath(bool aabBundle = false, bool devBuild = false) {
            var endWord = aabBundle ? "aab" : "apk";
            var projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            var appTitle = Application.productName;
            var version = Application.version;
            var symbolD = devBuild ? "d" : "";
            var path = $"{projectPath}/Builds/{appTitle} v.{version}{symbolD}.{endWord}";
            return path;
        }
        
        private static void Build(BuildOptions buildOptions, string path) {
            var config = EditorHelper.LoadOrCreateAsset<AndroidBuildConfig>(CONFIG_PATH);
            PreparePasswords(config);

            var scenes = config.GetScenesPaths();
            var message = BuildPipeline.BuildPlayer(
                scenes,
                path,
                BuildTarget.Android,
                buildOptions
            );
            
            EditorUtility.RevealInFinder(path);
            Debug.Log($"Android build complete: {message}");
        }

        private static void PreparePasswords(AndroidBuildConfig config) {
            PlayerSettings.runInBackground = false;

            PlayerSettings.keystorePass = config.keyStorePassword;
            PlayerSettings.keyaliasPass = config.keyAliasPassword;
        }

#endif

    }
}