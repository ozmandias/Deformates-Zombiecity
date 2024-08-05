using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject confirmExitPanel;
    [SerializeField] GameManager uiGameManager;
    [SerializeField] Slider healthSlider;
    [SerializeField] Text currentBulletsText;
    [SerializeField] Text maxBulletsText;
    [SerializeField] Text totalKillsText;
    AudioManager uiAudioManager;

    #region Singleton
        public static UIManager instance;

        void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    void Start() {
        uiGameManager = GameManager.instance;
        uiAudioManager = AudioManager.instance;

        HideConfirmExitPanel();
        HideMenuPanel();
    }

    void Update() {
        ToggleMenuPanel();
    }

    public void ShowMenuPanel() {
        uiGameManager.ShowCursor();
        menuPanel.SetActive(true);
    }
    public void HideMenuPanel() {
        uiGameManager.HideCursor();
        menuPanel.SetActive(false);
    }
    public void ToggleMenuPanel() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(menuPanel.activeSelf == false) {
                if(confirmExitPanel.activeSelf == true) {
                    HideConfirmExitPanel();
                }
                ShowMenuPanel();
            } else {
                HideMenuPanel();
            }
        }
    }
    public void MobileToggleMenuPanel() {
        if(menuPanel.activeSelf == false) {
            if(confirmExitPanel.activeSelf == true) {
                HideConfirmExitPanel();
            }
            ShowMenuPanel();
        } else {
            HideMenuPanel();
        }
    }

    public void ShowConfirmExitPanel() {
        if(menuPanel.activeSelf == true) {
            HideMenuPanel();
        }
        uiGameManager.ShowCursor();
        confirmExitPanel.SetActive(true);
    }
    public void HideConfirmExitPanel() {
        uiGameManager.HideCursor();
        if(menuPanel.activeSelf == false) {
            ShowMenuPanel();
        }
        confirmExitPanel.SetActive(false);
    }

    public void UpdateHealthSlider(int _health) {
        healthSlider.value = _health;
    }

    public void UpdateCurrentBulletsText(int _currentBullets) {
        currentBulletsText.text = "" + _currentBullets;
    }

    public void UpdateMaxBulletsText(int _maxBullets) {
        maxBulletsText.text = "" + _maxBullets;
    }

    public void UpdateTotalKillsText(int _totalKills) {
        totalKillsText.text = "" + _totalKills;
    }

    public void LightUpButton(GameObject buttonObject) {
        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
    }
    public void LightDownButton(GameObject buttonObject) {
        Image buttonImage = buttonObject.GetComponent<Image>();
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);
    }

    public void ReturnToMainMenu() {
        uiAudioManager.Stop_Game_Music();
        uiAudioManager.Play_MainMenu_Music();
        SceneManager.LoadScene("mainmenu");
    }
}