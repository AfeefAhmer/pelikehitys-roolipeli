using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip playerOpenDoor;
    [SerializeField] private AudioClip playerMeetMerchant;
    [SerializeField] private AudioClip playerBuyItem;
    [SerializeField] private AudioClip playerInvalidAction;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic; // Taustamusiikki

    [Header("Settings")]
    [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
    [SerializeField] private bool randomizePitch = true;

    public enum SoundEffect
    {
        PlayerOpenDoor,
        PlayerMeetMerchant,
        PlayerBuyItem,
        PlayerInvalidAction
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Aloitetaan taustamusiikki automaattisesti, jos määritelty
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }

    public void PlaySound(SoundEffect sound)
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("SFX Source puuttuu AudioManagerista!");
            return;
        }

        AudioClip clip = GetClip(sound);

        if (clip == null)
        {
            Debug.LogWarning("AudioClip puuttuu: " + sound);
            return;
        }

        if (randomizePitch)
            sfxSource.pitch = Random.Range(0.95f, 1.05f);
        else
            sfxSource.pitch = 1f;

        sfxSource.PlayOneShot(clip, masterVolume);
    }

    private AudioClip GetClip(SoundEffect sound)
    {
        switch (sound)
        {
            case SoundEffect.PlayerOpenDoor: return playerOpenDoor;
            case SoundEffect.PlayerMeetMerchant: return playerMeetMerchant;
            case SoundEffect.PlayerBuyItem: return playerBuyItem;
            case SoundEffect.PlayerInvalidAction: return playerInvalidAction;
            default: return null;
        }
    }

    public void PlayMusic(AudioClip music)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("Music Source puuttuu AudioManagerista!");
            return;
        }

        if (music == null)
        {
            Debug.LogWarning("Music clip on null!");
            return;
        }

        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = masterVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    // 🔹 Uusi metodi: soita taustamusiikki
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null || musicSource == null) return;

        if (musicSource.isPlaying) return; // Jos musiikki jo soi, ei uudelleen

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = masterVolume;
        musicSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = masterVolume;

        if (musicSource != null)
            musicSource.volume = masterVolume;
    }

    public void ToggleMute(bool mute)
    {
        AudioListener.pause = mute;
    }
}