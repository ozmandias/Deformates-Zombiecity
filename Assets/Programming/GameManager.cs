using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public bool gameEnds = false;
    public bool gameEnded = false;
    UIManager gameUIManager;
    public List<GameObject> zombieList = new List<GameObject>();
    
    public delegate void OnGameEnds(bool status);
    public OnGameEnds OnGameEndsCallback;

    #region Singleton
        public static GameManager instance;

        void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    void Start() {
        gameUIManager = UIManager.instance;

        HideCursor();

        OnGameEndsCallback += GameEnds;
    }

    public void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
    }
    public void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RegisterZombie(GameObject _zombie) {
        zombieList.Add(_zombie);
    }

    public void UnregisterZombie(GameObject _zombie) {
        zombieList.Remove(_zombie);
    }

    public void GameEnds(bool _gameEnds) {
        // Debug.Log("GameEnds");
        gameEnds = _gameEnds;
        if(gameEnds == true) {
            gameUIManager.SetGameEndsText("Mission Completed!");
            AudioManager.instance.Stop_Game_Music();
            AudioManager.instance.Play_GameEnds_Music();
        } else {
            gameUIManager.SetGameEndsText("You Died!");
            AudioManager.instance.Stop_Game_Music();
            AudioManager.instance.Play_Death_Music();
        }
        gameEnded = true;
        gameUIManager.ShowGameEndsPanel();
    }
}