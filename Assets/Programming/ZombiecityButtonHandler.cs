using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ZombiecityButtonHandler : ButtonHandler {
    [SerializeField] Image[] buttonImages;

    void Start() {
        buttonImages = GetComponentsInChildren<Image>();
    }

    public override void SetDownState() {
        base.SetDownState();
        // Debug.Log("Button Down");
        foreach(Image buttonImage in buttonImages) {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
        }
    }

    public override void SetUpState() {
        base.SetUpState();
        // Debug.Log("Button Up");
        foreach(Image buttonImage in buttonImages) {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0.5f);
        }
    }
}