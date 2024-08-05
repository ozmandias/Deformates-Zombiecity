using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    Animator zombieAnimator;
    NavMeshAgent zombieAgent;
    [SerializeField] GameObject target;
    [SerializeField] float speed = 2f;
    CharacterInfo zombieInfo;
    [SerializeField] bool isAttacking = false;
    [SerializeField] List<AnimationClip> zombieAnimationClipList = new List<AnimationClip>();

    void Start() {
        zombieAnimator = gameObject.GetComponentInChildren<Animator>();
        zombieAgent = gameObject.GetComponent<NavMeshAgent>();
        zombieInfo = gameObject.GetComponent<CharacterInfo>();

        if(target == null) {
            target = GameObject.FindWithTag("Player");
        }

        if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Spawn") {
            StartCoroutine(SetupZombie(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
        }

        foreach(AnimationClip runtimeAnimationClip in zombieAnimator.runtimeAnimatorController.animationClips) {
            if(zombieAnimationClipList.Contains(runtimeAnimationClip) == false) {
                zombieAnimationClipList.Add(runtimeAnimationClip);
            }
        }
    }

    void Update() {
        Move();
        Attack();
    }

    void Move() {
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);
        // Debug.Log("targetDistance: " + targetDistance);

        Vector3 rotateDirection = target.transform.position - transform.position;
        Vector3 rotation = Vector3.RotateTowards(transform.forward, rotateDirection, speed, 0);
        transform.rotation = Quaternion.LookRotation(rotation);

        if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" || zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Walk") {
            if(targetDistance > 1f) {
                isAttacking = false;
                
                // transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime);
                if(zombieAgent.isOnNavMesh == true) {
                    zombieAgent.destination = target.transform.position;
                    Play_Walk_Animation();
                }
            } else if(targetDistance <= 1f && transform.parent == null) {
                isAttacking = true;
            }
        } else if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack3" && transform.parent != null) {
            // Debug.Log("Zombie bites!");
            StartCoroutine(Unparent(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
        }
    }

    void Attack() {
        if(isAttacking) {
            if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" || zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Walk") {
                int randomNumber = Random.Range(1, 4);
                // randomNumber = 3;
                if(randomNumber == 3) {
                    // zombieAgent.destination = target.transform.position;
                    transform.SetParent(target.transform, true);
                }
                Play_Attack_Animation(randomNumber);
            }
        }
    }

    // Animations
    void Play_Walk_Animation() {
        zombieAnimator.Play("Walk");
    }

    void Play_Attack_Animation(int attackAnimationNumber) {
        zombieAnimator.Play("Attack"+attackAnimationNumber);
    }

    public void Play_Hit_Animation() {
        zombieAnimator.Play("Hit");
    }

    public void Play_Death_Animation() {
        zombieAnimator.Play("Fall");
    }

    // Collisions
    void OnCollisionEnter(Collision objectCollision) {
        
    }

    // Utilities
    public List<AnimationClip> GetAnimationClipList() {
        return zombieAnimationClipList;
    }

    public CharacterInfo GetCharacterInfo() {
        return zombieInfo;
    }

    public GameObject GetTarget() {
        return target;
    }
    
    // Coroutines
    IEnumerator SetupZombie(float duration) {
        // Debug.Log("SetupZombie");
        // Debug.Log("SetupZombie duration: " + duration);
        yield return new WaitForSeconds(duration - 0.25f);
        zombieAgent.enabled = true;
    }

    IEnumerator Unparent(float duration) {
        // Debug.Log("Unparent, duration: " + duration);
        yield return new WaitForSeconds(duration/2);
        transform.parent = null;
    }
}