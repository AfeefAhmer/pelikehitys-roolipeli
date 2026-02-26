using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // ==============================
    // Singleton
    // ==============================
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeAudioSources();
        InitializeSoundDictionary();
    }

    // ==============================
    // AudioSources
    // ==============================
    private AudioSource sfxSource;
    private AudioSource musicSource;

    private void InitializeAudioSources()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
    }

    // ==============================
    // Enum Sound Effects
    // ==============================
    public enum SoundEffect
    {
        Jump,
        Shoot,
        Explosion,
        Click
    }

    [System.Serializable]
    public class SoundEntry
    {
        public SoundEffect sound;
        public AudioClip clip;
    }

    [SerializeField]
    private List<SoundEntry> soundEntries;

    private Dictionary<SoundEffect, AudioClip> soundDictionary;

    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<SoundEffect, AudioClip>();

        foreach (var entry in soundEntries)
        {
            if (!soundDictionary.ContainsKey(entry.sound))
            {
                soundDictionary.Add(entry.sound, entry.clip);
            }
        }
    }

    // ==============================
    // Public Methods
    // ==============================

    // Soittaa suoraan AudioClipin
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("PlaySound: clip is null!");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    // Soittaa enum-arvon perusteella
    public void PlaySound(SoundEffect sound)
    {
        if (soundDictionary.TryGetValue(sound, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SoundEffect {sound} not found!");
        }
    }

    // Aloittaa musiikin
    public void PlayMusic(AudioClip music)
    {
        if (music == null)
        {
            Debug.LogWarning("PlayMusic: music is null!");
            return;
        }

        musicSource.clip = music;
        musicSource.Play();
    }

    // Lopettaa musiikin
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}