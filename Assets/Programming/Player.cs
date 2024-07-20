using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
    [SerializeField] Animator playerAnimator;
    [SerializeField] float speed = 4f;
    float rotateAngle = 0;

    void Start() {
        playerAnimator = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    void Update() {
        #if UNITY_EDITOR
            Move();
        #elif UNITY_ANDROID
            MobileMove();
        #endif
    }

    void Move() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        if((horizontal < 1f && horizontal > 0) || (horizontal > -1f && horizontal < 0) || (vertical < 1f && vertical > 0) || (vertical > -1f && vertical < 0)) {
            rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            PlayRunAnimation();
        } else if(horizontal == 0 && vertical == 0) {
            StopRunAnimation();
        }

        gameObject.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
        gameObject.transform.position = gameObject.transform.position + direction;
    }

    void MobileMove() {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        if((horizontal < 1f && horizontal > 0) || (horizontal > -1f && horizontal < 0) || (vertical < 1f && vertical > 0) || (vertical > -1f && vertical < 0)) {
            rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            PlayRunAnimation();
        } else if(horizontal == 0 && vertical == 0) {
            StopRunAnimation();
        }

        gameObject.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
        gameObject.transform.position = gameObject.transform.position + direction;
    }

    void PlayRunAnimation() {
        playerAnimator.SetBool("Run", true);
    }

    void StopRunAnimation() {
        playerAnimator.SetBool("Run", false);
    }
}