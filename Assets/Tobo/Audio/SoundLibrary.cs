using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using Tobo.Util.Editor;
#endif

namespace Tobo.Audio
{
    //[CreateAssetMenu(menuName = "Scriptable Objects/Sound Library")]
    public class SoundLibrary : ScriptableObject
    {
        // Doing this because I was getting some weird serialization bugs when
        //  AudioManager just had a Sound[]
        // Probably because it was in a prefab in resources or smth, idk

        [Header("Fill through Menu (Audio/UpdateCurrentSounds)")]
        public Sound[] sounds;

#if UNITY_EDITOR
        [MenuItem("Audio/Update Current Sounds")]
        static void FillSounds()
        {
            SoundLibrary lib = LibraryUtil.FillLibrary<SoundLibrary, Sound>(nameof(SoundLibrary.sounds));
            AudioCodegen.Generate(lib);
        }

        public static void FillAndGenerateSounds(bool generate = true)
        {
            SoundLibrary lib = LibraryUtil.FillLibrary<SoundLibrary, Sound>(nameof(SoundLibrary.sounds));
            if (generate)
                AudioCodegen.Generate(lib);
        }

        public static void Generate()
        {
            AudioCodegen.Generate(LibraryUtil.FindLibrary<SoundLibrary>());
        }
#endif
    }
}
