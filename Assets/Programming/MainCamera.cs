using UnityEngine;

public class MainCamera : MonoBehaviour {
    [SerializeField] GameObject player;
    [SerializeField] Vector3 distanceTransform;
    [SerializeField] float maxX;
    [SerializeField] float minX;
    [SerializeField] float maxZ;
    [SerializeField] float minZ;

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
        if(gameObject.transform.position.x < minX) {
            if(gameObject.transform.position.z > maxZ) {
                gameObject.transform.position = new Vector3(minX, gameObject.transform.position.y, maxZ);
            } else if(gameObject.transform.position.z < minZ) {
                gameObject.transform.position = new Vector3(minX, gameObject.transform.position.y, minZ);
            } else {
                gameObject.transform.position = new Vector3(minX, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } else if(gameObject.transform.position.x > maxX) {
            if(gameObject.transform.position.z > maxZ) {
                gameObject.transform.position = new Vector3(maxX, gameObject.transform.position.y, maxZ);
            } else if(gameObject.transform.position.z < minZ) {
                gameObject.transform.position = new Vector3(maxX, gameObject.transform.position.y, minZ);
            } else {
                gameObject.transform.position = new Vector3(maxX, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } else if(gameObject.transform.position.z > maxZ) {
            if(gameObject.transform.position.x < minX) {
                gameObject.transform.position = new Vector3(minX, gameObject.transform.position.y, maxZ);
            } else if(gameObject.transform.position.x > maxX) {
                gameObject.transform.position = new Vector3(maxX, gameObject.transform.position.y, maxZ);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, maxZ);
            }
        } else if(gameObject.transform.position.z < minZ) {
            if(gameObject.transform.position.x < minX) {
                gameObject.transform.position = new Vector3(minX, gameObject.transform.position.y, minZ);
            } else if(gameObject.transform.position.x > maxX) {
                gameObject.transform.position = new Vector3(maxX, gameObject.transform.position.y, minZ);
            } else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, minZ);
            }
        }
    }
}