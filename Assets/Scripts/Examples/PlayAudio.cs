using Tobo.Audio; // Note the include up here
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public Sound playThisByObject;
    public Sound.ID playThisByID;

    void Start()
    {
        // Play a sound
        Sound.Jump.PlayDirect();

        // Change some settings (volume, pitch), then play a sound
        Sound.Land.Override().SetVolume(0.5f).SetPitch(0.2f).Play();

        // Select a sound ScriptableObject in the inspector...
        playThisByObject.PlayDirect();

        // ... Or get it from an ID
        Sound.Get(playThisByID).PlayDirect();

        // ... Or use the extension method to play directly from the enum
        playThisByID.PlayDirect();

        // Loop a sound
        PooledAudioSource source = Sound.RedBallPickup.PlayDirect();
        source.IsLooping = true;

        // Loop a sound (more succinct)
        Sound.Dash.PlayDirect().IsLooping = true;
        

        // See "Guidelines > Tobo.Audio" to see how to import sounds into this system
    }
}
