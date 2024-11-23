using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Service
{
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        foreach (AudioClip clip in clips)
            audioClips[clip.name] = clip;
    }

    public void PlaySound(string soundName)
    {
        if (audioClips.ContainsKey(soundName))
            audioSource.PlayOneShot(audioClips[soundName]);
        else
            Debug.LogWarning("Este audio no existe o lo pusiste en la carpeta equivocada: " + soundName); 
    }

    public void PlaySound(string[] possibleSounds)
    {
        List<AudioClip> possibleAudios = new List<AudioClip>();

        foreach (string soundName in possibleSounds)
        {
            if (audioClips.ContainsKey(soundName))
                possibleAudios.Add(audioClips[soundName]);
        }

        if (possibleAudios.Count > 0)
            audioSource.PlayOneShot(possibleAudios[Random.Range(0, possibleAudios.Count)]);
        else
            Debug.LogWarning("Ninguno de esos audios está en la carpeta");
    }

    public void PlayLoop(string soundName)
    {
        if (audioSource.isPlaying)
            return;

        if (audioClips.ContainsKey(soundName))
        {
            audioSource.clip = audioClips[soundName];
            audioSource.loop = true;
            audioSource.Play();
        }
        else
            Debug.LogWarning("Este audio no existe o lo pusiste en la carpeta equivocada: " + soundName);
    }

    public void StopLoop()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
