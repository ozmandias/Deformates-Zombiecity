using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<GameObject> zombieList = new List<GameObject>();

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
        HideCursor();
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
}