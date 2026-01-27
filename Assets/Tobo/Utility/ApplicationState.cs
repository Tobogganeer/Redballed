using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tobo.Util
{

    // https://github.com/FirstGearGames/FishNet/blob/main/Assets/FishNet/Runtime/Plugins/GameKit/Dependencies/Utilities/ApplicationState.cs

    /// <summary>
    /// Used to detect whether the application is playing or quitting.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class ApplicationState
    {
#if !UNITY_EDITOR
        /// <summary>
        /// True if application is quitting.
        /// </summary>
        private static bool _isQuitting;
#endif
        static ApplicationState()
        {
#if !UNITY_EDITOR
            _isQuitting = false;
#endif
            Application.quitting -= Application_quitting;
            Application.quitting += Application_quitting;
        }

        private static void Application_quitting()
        {
#if !UNITY_EDITOR
            _isQuitting = true;
#endif
        }

        public static bool IsQuitting()
        {
#if UNITY_EDITOR
            if ((!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying) || !EditorApplication.isPlaying)
                return true;
            else
                return false;
#else
            return _isQuitting;
#endif
        }

        public static bool IsPlaying()
        {
#if UNITY_EDITOR
            return EditorApplication.isPlaying;
#else
            return Application.isPlaying;
#endif
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
