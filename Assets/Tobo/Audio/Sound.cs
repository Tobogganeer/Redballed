using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobo.Audio
{
    [CreateAssetMenu(menuName = "Audio/Sound")]
    public partial class Sound : ScriptableObject
    {
        [SerializeField] private ID soundID;
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private float maxDistance = 35f;
        [SerializeField] private AudioCategory category = AudioCategory.SFX;
        [Range(0f, 1f)]
        [SerializeField] private float volume = 1.0f;
        [SerializeField] private float minPitch = 0.85f;
        [SerializeField] private float maxPitch = 1.1f;

        public ID SoundID => soundID;
        public AudioClip[] Clips => clips;
        public float MaxDistance => maxDistance;
        public AudioCategory Category => category;
        public float Volume => volume;
        public float MinPitch => minPitch;
        public float MaxPitch => maxPitch;

        /// <summary>
        /// Gets the Sound with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the Sound ScriptableObject</param>
        /// <returns></returns>
        public static Sound Get(string name)
        {
            // See if the passed name is a SoundID directly (similar, but different to file name)
            if (!SoundIDNameToSoundID.TryGetValue(name, out ID id))
                // Check if the file name converts
                if (FilenameToSoundIDName.TryGetValue(name, out name))
                    id = SoundIDNameToSoundID[name];
                else
                {
                    Debug.LogWarning("Couldn't find sound with ID: " + name);
                    return Get(ID.None);
                }
            return Get(id);
        }

        /// <summary>
        /// Gets the Sound with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Sound Get(ID id)
        {
            return AudioManager.GetSound(id);
        }

        /// <summary>
        /// Starts overriding the default settings on the Sound with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>Follow with calls to SetPosition(), SetVolume(), etc.</remarks>
        public static Audio Override(string name)
        {
            return Get(name).Override();
        }

        /// <summary>
        /// Starts overriding the default settings on the Sound with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Follow with calls to SetPosition(), SetVolume(), etc.</remarks>
        public static Audio Override(ID id)
        {
            return Get(id).Override();
        }

        /// <summary>
        /// Starts overriding the default settings on this Sound.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Follow with calls to SetPosition(), SetVolume(), etc.</remarks>
        public Audio Override()
        {
            return GetAudio();
        }
        
        private Audio GetAudio()
        {
            return new Audio(this);
        }

        #region Play
        /// <summary>
        /// Plays this Sound at the given <paramref name="position"/> in world space.
        /// </summary>
        /// <param name="position">Position in world space</param>
        /// <param name="parent">Object to parent the Sound to</param>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayAtPosition(Vector3 position, Transform parent = null)
        {
            return AudioManager.PlayAudio(GetAudio().SetPosition(position).SetParent(parent));
        }

        /// <summary>
        /// Plays this Sound directly into the listener's ears (i.e. not in world space).
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayDirect()
        {
            return AudioManager.PlayAudio(GetAudio().Set2D());
        }

#if TOBO_NET
        /// <summary>
        /// Plays this Sound locally at the given <paramref name="position"/> in world space. It will not be networked.
        /// </summary>
        /// <param name="position">Position in world space</param>
        /// <param name="parent">Object to parent the Sound to</param>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayAtPositionLocal(Vector3 position, Transform parent = null)
        {
            return AudioManager.PlayAudioLocal(GetAudio().SetPosition(position).SetParent(parent));
        }

        /// <summary>
        /// Plays this Sound locally directly into the listener's ears (i.e. not in world space). It will not be networked.
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayDirectLocal()
        {
            return AudioManager.PlayAudioLocal(GetAudio().Set2D());
        }
#endif
        #endregion

        internal static Sound CreateInternal(string name, List<AudioClip> clips, AudioCategory category)
        {
            Sound s = CreateInstance<Sound>();

            s.name = name;
            s.soundID = (ID)AudioCodegen.GetSoundIDBeforeCompilation(name);
            s.clips = clips.ToArray();
            s.category = category;

            return s;
        }
    }

    public static class SoundIDExtensions
    {
        /// <summary>
        /// Starts overriding the default settings on the Sound with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>Follow with calls to SetPosition(), SetVolume(), etc.</remarks>
        public static Audio Override(this Sound.ID id)
        {
            return Sound.Override(id);
        }

        /// <summary>
        /// Plays this Sound at the given <paramref name="position"/> in world space.
        /// </summary>
        /// <param name="position">Position in world space</param>
        /// <param name="parent">Object to parent the Sound to</param>
        /// <returns>The spawned AudioSource</returns>
        public static PooledAudioSource PlayAtPosition(this Sound.ID id, Vector3 position, Transform parent = null)
        {
            return Sound.Get(id).PlayAtPosition(position, parent);
        }

        /// <summary>
        /// Plays this Sound directly into the listener's ears (i.e. not in world space).
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public static PooledAudioSource PlayDirect(this Sound.ID id)
        {
            return Sound.Get(id).PlayDirect();
        }

        /// <summary>
        /// Returns <see cref="Sound.ID.None"/> if this <see cref="Sound"/> is null.
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        /// <remarks>Use to play a <see cref="Sound"/> that might be null (like something assignable from the inspector)</remarks>
        public static Sound MaybeNull(this Sound sound)
        {
            return sound ?? Sound.Get(Sound.ID.None);
        }

#if TOBO_NET
        /// <summary>
        /// Plays this Sound locally at the given <paramref name="position"/> in world space. It will not be networked.
        /// </summary>
        /// <param name="position">Position in world space</param>
        /// <param name="parent">Object to parent the Sound to</param>
        /// <returns>The spawned AudioSource</returns>
        public static PooledAudioSource PlayAtPositionLocal(this Sound.ID id, Vector3 position, Transform parent)
        {
            return Sound.Get(id).PlayAtPositionLocal(position, parent);
        }

        /// <summary>
        /// Plays this Sound locally directly into the listener's ears (i.e. not in world space). It will not be networked.
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public static PooledAudioSource PlayDirectLocal(this Sound.ID id)
        {
            return Sound.Get(id).PlayDirectLocal();
        }
#endif
    }
}
