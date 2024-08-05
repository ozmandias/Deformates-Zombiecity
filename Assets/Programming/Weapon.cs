using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {
    public GunInfo currentGun;
    UIManager weaponUIManager;

    void Start() {
        weaponUIManager = UIManager.instance;

        currentGun.AddBullets();
        weaponUIManager.UpdateMaxBulletsText(currentGun.maxBullets);
        weaponUIManager.UpdateCurrentBulletsText(currentGun.currentBullets);
    }

    void Update() {
        #if UNITY_EDITOR
            
        #elif UNITY_ANDROID
            
        #endif
    }
}