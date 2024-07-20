using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ZombiecityJoystick : Joystick {
    [SerializeField] Image[] joystickImages;

    public override void Start() {
        base.Start();
        joystickImages = GetComponentsInChildren<Image>();
    } 

    public override void OnDrag(PointerEventData data) {
        base.OnDrag(data);
        // Debug.Log("OnDrag");
        foreach(Image joystickImage in joystickImages) {
            joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, 1f);
        }
    }

    public override void OnPointerUp(PointerEventData data) {
        base.OnPointerUp(data);
        // Debug.Log("OnPointerUp");
        foreach(Image joystickImage in joystickImages) {
            joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, 0.5f);
        }
    }

    public override void OnPointerDown(PointerEventData data) {
        base.OnPointerDown(data);
        // Debug.Log("OnPointerDown");
        foreach(Image joystickImage in joystickImages) {
            joystickImage.color = new Color(joystickImage.color.r, joystickImage.color.g, joystickImage.color.b, 1f);
        }
    }
}