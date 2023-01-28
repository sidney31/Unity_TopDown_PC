using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume;
    public AudioSource audioSource;
    public bool loop;
}