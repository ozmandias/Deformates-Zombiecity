using UnityEngine;

public class MainCamera : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] Vector3 distanceTransform;

    void Start() {
        player = GameObject.Find("Player");
    }

    void Update() {
        Move();
    }

    void Move() {
        gameObject.transform.position = player.transform.position + distanceTransform;
        
        Reposition();
    }

    void Reposition() {
        // stops camera at x: -36, 42 / z: 32, -64
        if(gameObject.transform.position.x < -36) {
            if(gameObject.transform.position.z > 32) {
                gameObject.transform.position = new Vector3(-36, gameObject.transform.position.y, 32);
            } else if(gameObject.transform.position.z < -64) {
                gameObject.transform.position = new Vector3(-36, gameObject.transform.position.y, -64);
            } else {
                gameObject.transform.position = new Vector3(-36, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } else if(gameObject.transform.position.x > 42) {
            if(gameObject.transform.position.z > 32) {
                gameObject.transform.position = new Vector3(42, gameObject.transform.position.y, 32);
            } else if(gameObject.transform.position.z < -64) {
                gameObject.transform.position = new Vector3(42, gameObject.transform.position.y, -64);
            } else {
                gameObject.transform.position = new Vector3(42, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } else if(gameObject.transform.position.z > 32) {
            if(gameObject.transform.position.x < -36) {
                gameObject.transform.position = new Vector3(-36, gameObject.transform.position.y, 32);
            } else if(gameObject.transform.position.x > 42) {
                gameObject.transform.position = new Vector3(42, gameObject.transform.position.y, 32);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 32);
            }
        } else if(gameObject.transform.position.z < -64) {
            if(gameObject.transform.position.x < -36) {
                gameObject.transform.position = new Vector3(-36, gameObject.transform.position.y, -64);
            } else if(gameObject.transform.position.x > 42) {
                gameObject.transform.position = new Vector3(42, gameObject.transform.position.y, -64);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -64);
            }
        }
    }
}