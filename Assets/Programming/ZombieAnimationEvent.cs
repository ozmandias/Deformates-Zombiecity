using UnityEngine;

public class ZombieAnimationEvent : MonoBehaviour {
    [SerializeField] Zombie zombieProgramming;

    void Start() {
        zombieProgramming = transform.root.gameObject.GetComponent<Zombie>();
        // Debug.Log("parent: " + transform.root);
    }

    public void EnableAttackCollider(string attackColliderName) {
        // Debug.Log("attackColliderName: " + attackColliderName);
        if(attackColliderName == "AttackCollider") {
            StartCoroutine(zombieProgramming.AttackCoroutine("attack"));
        } else {
            StartCoroutine(zombieProgramming.AttackCoroutine("bite"));
        }
    }
}