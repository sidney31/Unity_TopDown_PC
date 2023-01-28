using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.loop = sound.loop;
        }
    }

    public void Play(string soundName)
    {
        foreach (Sound soundFromArray in sounds)
        {
            if (soundFromArray.clipName == soundName)
            {
                Sound sound = soundFromArray;
                sound.audioSource.Play();
            }
            else
            {
                return;
            }
        } 
    }
}
