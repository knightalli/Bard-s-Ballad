using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource[] _allMusicSources;

    [Header("Main Music")]
    [SerializeField] private AudioClip mainMenuMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip buttonClickClip;

    private const float MUSIC_VOLUME = 0.5f;
    private const float SFX_VOLUME = 0.5f;

    private float musicVolume;
    private float soundVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
            SceneManager.sceneLoaded += OnSceneLoaded; // <--- подписка на событие загрузки сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }   

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded; // <--- отпишись!
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) // или scene.name == "»м€—цены"
        {
            // —обрать все AudioSource на сцене 1 (Ќ≈ включа€ DontDestroy объекты)
            _allMusicSources = FindObjectsOfType<AudioSource>(false);
        }
    }

    private void InitializeAudio()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicVolume = MUSIC_VOLUME;
        }
        musicSource.volume = musicVolume;

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            soundVolume = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            soundVolume = SFX_VOLUME;
        }
        sfxSource.volume = soundVolume;
    }


    public void PlayMainMenuMusic()
    {
        if (musicSource.clip == mainMenuMusic && musicSource.isPlaying) return;
        FadeMusic(mainMenuMusic);
    }

    private void FadeMusic(AudioClip newMusic)
    {
        musicSource.clip = newMusic;
        musicSource.Play();
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip);
    }

    private void PlaySFX(AudioClip sfx)
    {
        if (sfx != null)
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");
            sfxSource.PlayOneShot(sfx);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (scene.buildIndex == 1)
        {
            foreach (var src in _allMusicSources)
            {
                if (src != null)
                    src.volume = volume;
            }
        }
        else
        {
            if (sfxSource != null)
                sfxSource.volume = volume;
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void StopMusic() => musicSource.Stop();
    public void PauseMusic() => musicSource.Pause();
    public void ResumeMusic() => musicSource.UnPause();
}

