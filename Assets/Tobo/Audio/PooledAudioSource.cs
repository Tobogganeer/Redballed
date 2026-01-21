using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tobo.Audio
{
    /// <summary>
    /// A class used by the <see cref="AudioManager"/> to re-use AudioSources.
    /// </summary>
    public class PooledAudioSource : MonoBehaviour
    {
        public AudioSource AudioSource
        {
            get
            {
                if (audioSource == null)
                    audioSource = GetComponent<AudioSource>();
                return audioSource;
            }
        }
        /// <summary>
        /// The <see cref="Tobo.Audio.Sound"/> that is playing.
        /// </summary>
        public Sound Sound => Sound.Get(soundID);
        /// <summary>
        /// The ID of the <see cref="Tobo.Audio.Sound"/> that is playing.
        /// </summary>
        public Sound.ID SoundID => soundID;
        /// <summary>
        /// Is this source back in the pool (sound has finished playing)?
        /// </summary>
        public bool IsInPool => transform.parent == audioSourceContainer;
        /// <summary>
        /// Sets this sound to loop. Remember to call <see cref="ReturnToPool"/> manually if the sound is looping.
        /// </summary>
        public bool IsLooping
        {
            get => AudioSource.loop;
            set
            {
                if (IsInPool)
                    return;

                AudioSource.loop = value;
                if (value == true)
                    StopAllCoroutines(); // Don't disable after time
                else
                    // Make it disable after clip is finished
                    ReturnToPoolAfterTime(AudioSource.clip.length / AudioSource.pitch + 0.25f);
            }
        }

        // The original container that holds all audio sources
        private Transform audioSourceContainer;
        private AudioSource audioSource;

        private Sound.ID soundID;

        private void Awake()
        {
            audioSourceContainer = transform.parent;
            gameObject.SetActive(false);
        }

        internal void Init(Sound.ID soundID)//, Transform parent, bool direct)
        {
            this.soundID = soundID;
            //assignedParent = parent;
            //wasPlayedDirect = direct;
        }

        /// <summary>
        /// Returns this audio source to the shared pool after the given amount of <paramref name="seconds"/> 
        /// </summary>
        /// <param name="seconds"></param>
        public void ReturnToPoolAfterTime(float seconds)
        {
            if (IsInPool)
                return;

            // Check if our parent is disabled
            if (!gameObject.activeInHierarchy || seconds == 0)
            {
                ReturnToPool();
                return;
            }

            StopAllCoroutines();
            StartCoroutine(ReturnAfterSecondsCoroutine(seconds));
        }

        /// <summary>
        /// Returns this audio source to the shared pool
        /// </summary>
        public void ReturnToPool()
        {
            if (IsInPool)
                return;

            // Return to original container so we don't get destroyed
            transform.SetParent(audioSourceContainer);
            gameObject.SetActive(false);
        }

        private IEnumerator ReturnAfterSecondsCoroutine(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            ReturnToPool();
        }

        private void OnDestroy()
        {
            AudioMaster.OnAudioSourceDestroyed(this);
        }
    }
}
