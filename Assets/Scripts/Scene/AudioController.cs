using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Defines all audio-related actions.
/// </summary>
public class AudioController : Singleton<AudioController>
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private List<AudioClip> playlist;

    /// <summary>
    /// Change the volume of a specific audio channel.
    /// </summary>
    public void ChangeVolume(string channel, float volume)
    {
        mixer.SetFloat(channel, Mathf.Log10(volume) * 20);
    }

    /// <summary>
    /// Get the volume of a specific audio channel.
    /// </summary>
    public float GetVolume(string channel)
    {
        float volume;
        mixer.GetFloat(channel, out volume);
        return Mathf.Pow(10, volume / 20);
    }

    /// <summary>
    /// Play the specificed track from the playlist. Loop by default.
    /// </summary>
    public void PlayTrack(int trackNum, bool loop = true)
    {
        source.clip = playlist[trackNum];
        source.loop = loop;
        source.Stop();
        source.Play();
    }
}
