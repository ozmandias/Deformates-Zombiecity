using UnityEngine;

public class AudioManager: MonoBehaviour {
    public AudioSource mainMenuMusic;
    public AudioSource gameMusic;
    public AudioSource gameEndsSound;
    public AudioSource gameEndsMusic;
    public AudioSource deathSound;
    public AudioSource deathMusic;

    #region Singleton
        public static AudioManager instance;

        void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else if(instance != this) {
                Destroy(this.gameObject);
            }
        }
    #endregion

    void Start() {
        Play_MainMenu_Music();
    }

    public void Play_MainMenu_Music() {
        mainMenuMusic.Play();
    }
    public void Stop_MainMenu_Music() {
        mainMenuMusic.Stop();
    }

    public void Play_Game_Music() {
        gameMusic.Play();
    }
    public void Stop_Game_Music() {
        gameMusic.Stop();
    }

    public void Play_GameEnds_Sound() {
        gameEndsSound.Play();
    }

    public void Play_GameEnds_Music() {
        gameEndsMusic.Play();
    }
    public void Stop_GameEnds_Music() {
        gameEndsMusic.Stop();
    }

    public void Play_Death_Sound() {
        
    }

    public void Play_Death_Music() {
        deathMusic.Play();
    }
    public void Stop_Death_Music() {
        deathMusic.Stop();
    }
}