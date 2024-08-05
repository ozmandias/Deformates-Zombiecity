using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    AudioManager mainMenuAudioManager;

    void Start() {
        mainMenuAudioManager = AudioManager.instance;
    }

    public void StartGame() {
        SceneManager.LoadScene("game");
        mainMenuAudioManager.Stop_MainMenu_Music();
        mainMenuAudioManager.Play_Game_Music();
    }

    public void ExitGame() {
        Application.Quit();
    }
}