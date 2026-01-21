using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobo.Util;
#if TOBO_NET
using Tobo.Net;
#endif

namespace Tobo.Audio
{
    [RequireComponent(typeof(AudioMaster))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        private void Awake()
        {
            instance = this;

            // Attached to App object, already in DontDestroyOnLoad

            /*

            if (Instance == null) Instance = this;

            else if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            */

            Init();
        }

        private void Init()
        {
            for (int i = 0; i < soundLibrary.sounds.Length; i++)
            {
                if (soundsDictionary.ContainsKey(soundLibrary.sounds[i].SoundID))
                {
                    Debug.LogError($"Tried to add with same ID twice: {soundsDictionary[soundLibrary.sounds[i].SoundID].name}" +
                        $" was added first with ID {soundLibrary.sounds[i].SoundID}, and {soundLibrary.sounds[i].name} has the same ID!");
                }
                soundsDictionary.Add(soundLibrary.sounds[i].SoundID, soundLibrary.sounds[i]);
            }

            // Make sure we have a 'None' sound
            if (!soundsDictionary.ContainsKey(Sound.ID.None))
                soundsDictionary.Add(Sound.ID.None, ScriptableObject.CreateInstance<Sound>());
            //throw new System.Exception("AudioManager requires a sound with ID " + Sound.ID.None + "!");
            
            /*
            clips.Clear();
            clipNameToIndex.Clear();

            int count = 0;

            for (int i = 0; i < singleClips.Length; i++)
            {
                clips.Add(singleClips[i]);
                if (clipNameToIndex.ContainsKey(clips[count].name))
                    Debug.LogError("Duplicate single audio key: " + clips[count].name);
                clipNameToIndex.Add(clips[count].name, count++);
            }

            for (int i = 0; i < clipGroups.Length; i++)
            {
                int[] indices = new int[clipGroups[i].clips.Length];

                for (int j = 0; j < clipGroups[i].clips.Length; j++)
                {
                    clips.Add(clipGroups[i].clips[j]);
                    if (clipNameToIndex.ContainsKey(clips[count].name))
                        Debug.LogError("Duplicate group audio clip key: " + clips[count].name);
                    clipNameToIndex.Add(clips[count].name, count);
                    indices[j] = count++;
                }

                if (groupNameToIndices.ContainsKey(clipGroups[i].name))
                    Debug.LogError("Duplicate group audio group key: " + clipGroups[i].name);
                groupNameToIndices.Add(clipGroups[i].name, indices);
            }
            */
        }

        //public AudioClip[] singleClips;
        //public AudioClipGroup[] clipGroups;
        //public Sound[] sounds;
        public SoundLibrary soundLibrary;

        private static readonly Dictionary<Sound.ID, Sound> soundsDictionary = new Dictionary<Sound.ID, Sound>();
        //private static readonly List<AudioClip> clips = new List<AudioClip>();
        //private static readonly Dictionary<string, int> clipNameToIndex = new Dictionary<string, int>();
        //private static readonly Dictionary<string, int[]> groupNameToIndices = new Dictionary<string, int[]>();

        private void OnDestroy()
        {
            soundsDictionary.Clear();
        }

        /*
        public static int GetClipIndex(string clipOrGroup)
        {
            if (groupNameToIndices.TryGetValue(clipOrGroup, out int[] indices))
                return indices[Random.Range(0, indices.Length)];
            else if (clipNameToIndex.TryGetValue(clipOrGroup, out int clip))
                return clip;
            else
            {
                Debug.LogWarning("Could not get clip for " + clipOrGroup);
                return -1;
            }
        }
        */


        /// <summary>
        /// Returns the <see cref="Sound"/> associated with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Sound GetSound(Sound.ID id)
        {
            if (!soundsDictionary.TryGetValue(id, out Sound sound))
            {
                Debug.LogWarning("No sound assigned for ID " + id);
                return soundsDictionary[Sound.ID.None];
            }

            return sound;
        }

        /*
        #region Play Methods
        public static PooledAudioSource Play(Sound sound, Vector3 position, Transform parent = null)
        {
            PlayAudio(sound.GetAudio().SetPosition(position).SetParent(parent));
        }

        public static PooledAudioSource Play2D(Sound sound)
        {
            return PlayAudio(sound.GetAudio().Set2D());
        }

        public static PooledAudioSource Play(Sound.ID soundID, Vector3 position, Transform parent = null)
        {
            Play(GetSound(soundID), position, parent);
        }

        public static PooledAudioSource Play2D(Sound.ID soundID)
        {
            Play2D(GetSound(soundID));
        }

        public static PooledAudioSource PlayLocal(Sound sound, Vector3 position, Transform parent = null)
        {
            PlayAudioLocal(sound.GetAudio().SetPosition(position).SetParent(parent));
        }

        public static PooledAudioSource PlayLocal2D(Sound sound)
        {
            PlayAudioLocal(sound.GetAudio().Set2D());
        }

        public static void PlayLocal(Sound.ID soundID, Vector3 position, Transform parent = null)
        {
            PlayLocal(GetSound(soundID), position, parent);
        }

        public static void PlayLocal2D(Sound.ID soundID)
        {
            PlayLocal2D(GetSound(soundID));
        }
        #endregion
        */

        public static PooledAudioSource PlayAudio(Audio audio)
        {
            // Send over network
#if TOBO_NET
            AudioPacket p = new AudioPacket(audio);

            if (NetworkManager.IsServer)
                p.SendTo(S_Client.This, true); // Send to everyone but us
            else if (NetworkManager.ConnectedToServer)
                p.Send(Client.This);
#endif

            // Play on our side
            return PlayAudioLocal(audio);
        }

#if TOBO_NET
        /// <summary>
        /// Method for playing audio that was predicted on a client
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="dontSendTo"></param>
        /// <param name="sendToServer"></param>
        /// <returns></returns>
        public static PooledAudioSource PlayAudio(Audio audio, S_Client dontSendTo, bool sendToServer = true)
        {
            if (NetworkManager.IsServer)
            {
                AudioPacket p = new AudioPacket(audio);

                foreach (S_Client client in S_Client.All.Values)
                {
                    bool localClient = client.LocalClient;
                    bool blacklist = client == dontSendTo;

                    if (blacklist || (localClient && !sendToServer))
                        continue;

                    p.SendTo(client);
                }
            }

            // Play on our side
            return PlayAudioLocal(audio);
        }
#endif

        public static PooledAudioSource PlayAudioLocal(Audio audio)
        {
            if (!soundsDictionary.TryGetValue(audio.ID, out Sound sound))
            {
                Debug.LogWarning("Tried to play audio with no corresponding Sound, ID " + audio.ID);
            }

            if (sound.SoundID == Sound.ID.None)
                return null;

            if (sound.Clips == null || sound.Clips.Length == 0)
            {
                Debug.LogWarning("Tried to play sound with no clips assigned, ID " + sound.SoundID);
            }

            if (audio.ClipIndex < 0 || audio.ClipIndex >= sound.Clips.Length)
            {
                Debug.LogWarning($"Clip (index: {audio.ClipIndex}) was outside range for Sound.ID {audio.ID} (sound has {sound.Clips.Length} registered clips).");
                return null;
            }

            GameObject sourceObj = AudioMaster.GetAudioSource();

            if (audio.Parent != null && !audio.Parent.gameObject.activeInHierarchy)
            {
                // Parent is turned off
                Debug.Log($"Skipping audio played on disabled parent ({audio.Parent.name})");
                sourceObj.SetActive(false);
                return null;
            }

            sourceObj.transform.SetParent(audio.Parent);
            sourceObj.transform.position = audio.Position;

            AudioSource source = sourceObj.GetComponent<AudioSource>();

            source.clip = sound.Clips[audio.ClipIndex];
            if (source.clip == null)
                Debug.LogWarning($"Chosen audio clip was null - Sound.ID: {sound.SoundID} - Clip Index: {audio.ClipIndex}");
            source.maxDistance = audio.MaxDistance;
            source.pitch = audio.Pitch;
            source.volume = audio.Volume;
            source.spatialBlend = audio.Flags.HasFlagByte(Audio.AudioFlags.Global) ? 0f : 1f; // 0 for 2d, 1 for 3d
            source.outputAudioMixerGroup = AudioMaster.GetGroup(audio.Category);
            source.Play();

            PooledAudioSource pooledSource = sourceObj.GetComponent<PooledAudioSource>();
            pooledSource.Init(audio.ID);//, audio.Parent, !audio.Flags.HasFlag(Audio.AudioFlags.Global));
            pooledSource.ReturnToPoolAfterTime(source.clip.length / audio.Pitch + 0.25f); // 0.25 seconds extra for good measure
            return pooledSource;
        }

        public static void OnNetworkAudio(Audio audio)
        {
            PlayAudioLocal(audio);
        }
    }

    public class Audio
#if TOBO_NET
    : IBufferStruct
#endif
    {
        public Sound.ID ID;
        public byte ClipIndex { get; private set; }
        public Vector3 Position { get; private set; }
        public Transform Parent { get; private set; }
        public float MaxDistance { get; private set; }
        public AudioCategory Category { get; private set; }
        public float Volume { get; private set; }
        public float Pitch { get; private set; }

        public AudioFlags Flags { get; private set; }

        public Audio() { }
        public Audio(Sound sound)
        {
            LoadDefaultsFrom(sound);
        }

        void LoadDefaultsFrom(Sound sound)
        {
            ID = sound.SoundID;
            MaxDistance = sound.MaxDistance;
            Category = sound.Category;
            Volume = sound.Volume;
            Pitch = Random.Range(sound.MinPitch, sound.MaxPitch);
            //if (sound.Is2d) Flags = AudioFlags.Global;
            Flags = AudioFlags.Global; // Will be un-set when a position is set
            if (sound.Clips.Length > 1)
            {
                Flags |= AudioFlags.Index;
                ClipIndex = (byte)Random.Range(0, sound.Clips.Length);
            }
        }

        #region Args

        public Audio SetClip(int clipIndex)
        {
            ClipIndex = (byte)clipIndex;
            Flags |= AudioFlags.Index;
            return this;
        }

        public Audio SetPosition(Vector3 position)
        {
            Position = position;
            Flags &= ~AudioFlags.Global; // Remove global flag
            return this;
        }

        public Audio SetParent(Transform parent)
        {
            Parent = parent;
            if (parent != null)
                Flags |= AudioFlags.Parent;
            return this;
        }

        public Audio SetDistance(float maxDistance)
        {
            if (MaxDistance != maxDistance)
            {
                MaxDistance = maxDistance;
                Flags |= AudioFlags.Distance;
            }
            return this;
        }

        public Audio SetVolume(float volume)
        {
            if (Volume != volume)
            {
                Volume = volume;
                Flags |= AudioFlags.Volume;
            }
            return this;
        }

        public Audio SetPitch(float min, float max)
        {
            Pitch = Random.Range(min, max);
            return this;
        }

        public Audio SetPitch(float pitch)
        {
            Pitch = pitch;
            return this;
        }

        public Audio SetCategory(AudioCategory category)
        {
            if (Category != category)
            {
                Category = category;
                Flags |= AudioFlags.Category;
            }
            return this;
        }

        public Audio SetGlobal()
        {
            Flags |= AudioFlags.Global;
            return this;
        }

        public Audio Set2D()
        {
            return SetGlobal();
        }

        #endregion

        /// <summary>
        /// Plays this Audio with the current settings.
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource Play()
        {
            return AudioManager.PlayAudio(this);
        }

        /// <summary>
        /// Plays this Audio with the current settings for the current client.
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayLocal()
        {
            return AudioManager.PlayAudio(this);
        }

        /// <summary>
        /// Plays this Audio, calling SetPosition() and SetParent().
        /// </summary>
        /// <returns>The spawned AudioSource</returns>
        public PooledAudioSource PlayAtPosition(Vector3 position, Transform parent = null)
        {
            return AudioManager.PlayAudio(SetPosition(position).SetParent(parent));
        }

        #region Net
#if TOBO_NET
        public void Serialize(ByteBuffer message)
        {
            message.Add(ID);
            message.Add(Flags);
            message.Add(Pitch);

            if (Flags.HasFlagByte(AudioFlags.Index))
                message.Add(ClipIndex);

            if (!Flags.HasFlagByte(AudioFlags.Global))
            {
                message.Add(Position);

                //NetworkID netObj = Parent != null ? Parent.GetComponent<NetworkID>() : null;
                //if (Flags.HasFlag(AudioFlags.Parent) && netObj != null)
                //    message.Add(netObj);

                if (Flags.HasFlagByte(AudioFlags.Distance))
                    message.Add(MaxDistance);
            }

            if (Flags.HasFlagByte(AudioFlags.Volume))
                message.Add(Volume);

            if (Flags.HasFlagByte(AudioFlags.Category))
                message.Add(Category);
        }

        public void Deserialize(ByteBuffer message)
        {
            ID = message.Read<Sound.ID>();
            LoadDefaultsFrom(Sound.Get(ID));

            Flags = message.Read<AudioFlags>();
            Pitch = message.Read<float>();

            if (Flags.HasFlagByte(AudioFlags.Index))
                ClipIndex = message.Read<byte>();

            if (!Flags.HasFlagByte(AudioFlags.Global))
            {
                Position = message.Read<Vector3>();

                //if (Flags.HasFlag(AudioFlags.Parent))
                //    SetParent(message.GetNetworkID()?.transform);

                if (Flags.HasFlagByte(AudioFlags.Distance))
                    MaxDistance = message.Read<float>();
            }

            if (Flags.HasFlagByte(AudioFlags.Volume))
                Volume = message.Read<float>();

            if (Flags.HasFlagByte(AudioFlags.Category))
                Category = message.Read<AudioCategory>();
        }
#endif
        #endregion

        public override string ToString()
        {
            return $"Audio:" +
                $" - ID: {ID}" +
                $" - ClipIndex: {ClipIndex}" +
                $" - Position: {Position}" +
                $" - Parent: {(Parent == null ? "null" : Parent)}" +
                $" - MaxDistance: {MaxDistance}" +
                $" - Category: {Category}" +
                $" - Volume: {Volume}" +
                $" - Pitch: {Pitch}" +
                $" - Flags: {Flags}";
        }

        [System.Flags]
        public enum AudioFlags : byte
        {
            None = 0,
            Global = 1 << 0,
            Parent = 1 << 1,
            Distance = 1 << 2,
            Volume = 1 << 3,
            //Pitch = 1 << 4, // Will have pitch every time
            Category = 1 << 5,
            Index = 1 << 6
        }
    }

    public enum AudioCategory : byte
    {
        Master,
        SFX,
        Ambient,
        Music
    }
}
