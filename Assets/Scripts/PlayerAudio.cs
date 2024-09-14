using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioSource RollAudioSource;
    public string BaseAudioClipPath = "Audio/AudioClips/";  // Path to the audio clips folder

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the AudioSource component is attached
        if (AudioSource == null || RollAudioSource == null)
        {
            Debug.LogError("Did you forget to set the AudioSource or RollAudioSource in Sphere?");
        }
    }

    public AudioSource GetAudioSource()
    {
        return AudioSource;
    }

    public void PlayRoll(string soundName = "roll")
    {
        SelectRandomClip(soundName, RollAudioSource);
        if (!AudioExists(RollAudioSource)) return;

        RollAudioSource.Play();
    }

    // Plays a random jump sound
    public void PlaySound(string soundName, bool withVariation = false)
    {
        SelectRandomClip(soundName, AudioSource);
        if ( !AudioExists(AudioSource) ) return;

        if (withVariation)
            { SelectRandomPitch(); }

        Debug.Log("in play sound");
        AudioSource.Play();
    }
    
    // Pitches the audio randomly
    private void SelectRandomPitch()
    {
        float minPitch = 0.8f;
        float maxPitch = 1.2f;
        AudioSource.pitch = Random.Range(minPitch, maxPitch);
    }

    // Return True if AudioSource exists
    public bool AudioExists(AudioSource audioSource)
    {
        if (audioSource != null && audioSource.clip != null)
            { return true; }
        return false;
    }

    // Selects a random clip from the specified folder
    public void SelectRandomClip(string folder, AudioSource audioSource)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(BaseAudioClipPath + folder.ToLower());
        if (clips.Length > 0)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
        }
        else
        {
            Debug.LogWarning("No audio clips found in folder: " + BaseAudioClipPath + folder.ToLower());
        }
    }

    // Adjust volume and pitch based on speed
    internal void SetVolumeAndPitch(float normalizedVolume, float pitchVelocity)
    {
        AudioSource.volume = normalizedVolume;
        AudioSource.pitch = pitchVelocity;
    }

    internal void SetRollVolumeAndPitch(float normalizedVolume, float pitchVelocity)
    {
        RollAudioSource.volume = normalizedVolume;
        RollAudioSource.pitch = pitchVelocity;
    }

    internal AudioSource GetRollAudioSource()
    {
        return RollAudioSource;
    }
}
