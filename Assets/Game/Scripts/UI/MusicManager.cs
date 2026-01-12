using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    // Singleton instance - music persists between scenes
    public static MusicManager Instance { get; private set; }

    [Header("Music Tracks")]
    public AudioClip menuMusic;
    public AudioClip battleMusic;

    [Header("Music Settings")]
    public float musicVolume = 0.5f;
    public bool playOnStart = true;

    [Header("Scene Names (for auto-switching)")]
    public string[] menuScenes = { "MainMenu", "SelectCharacter", "SelectStage" };
    public string[] battleScenes = { "Map1", "Map2", "Map3" };

    private AudioSource audioSource;
    private AudioClip currentlyPlaying;

    void Awake()
    {
        // Singleton pattern - only one MusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSource();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (playOnStart)
        {
            PlayMusicForCurrentScene();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Check if it's a menu scene
        foreach (string menuScene in menuScenes)
        {
            if (sceneName == menuScene)
            {
                PlayMenuMusic();
                return;
            }
        }

        // Check if it's a battle scene
        foreach (string battleScene in battleScenes)
        {
            if (sceneName == battleScene)
            {
                PlayBattleMusic();
                return;
            }
        }

        // Unknown scene - keep current music playing
        Debug.Log($"MusicManager: Unknown scene '{sceneName}', keeping current music.");
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null && currentlyPlaying != menuMusic)
        {
            currentlyPlaying = menuMusic;
            audioSource.clip = menuMusic;
            audioSource.Play();
            Debug.Log("MusicManager: Playing menu music");
        }
    }

    public void PlayBattleMusic()
    {
        if (battleMusic != null && currentlyPlaying != battleMusic)
        {
            currentlyPlaying = battleMusic;
            audioSource.clip = battleMusic;
            audioSource.Play();
            Debug.Log("MusicManager: Playing battle music");
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            currentlyPlaying = null;
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }

    public void SetVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (audioSource != null)
        {
            audioSource.volume = musicVolume;
        }
    }

    public bool IsPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
