using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    Animator zombieAnimator;
    NavMeshAgent zombieAgent;
    NavMeshObstacle zombieObstacle;
    [SerializeField] GameObject target;
    [SerializeField] float speed = 2f;
    [SerializeField] float sphereRotateSpeed = 12f;
    [SerializeField] Collider zombieAttackCollider;
    [SerializeField] Collider zombieBiteCollider;
    CharacterInfo zombieInfo;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] bool nearObstacle = false;
    [SerializeField] bool nearAnotherZombie = false;
    [SerializeField] List<AnimationClip> zombieAnimationClipList = new List<AnimationClip>();
    [SerializeField] GameObject zombieRaycastObject;
    Ray zombieRay;
    RaycastHit zombieRaycastHit;
    [SerializeField] float zombieRaycastDistance = 1f /* 2f */;
    int zombieLayerMask;
    [SerializeField] GameObject zombieAtFront;
    NavMeshPath zombiePath;
    public int attackDamage = 5;
    public int biteDamage = 10;

    void Start() {
        zombieAnimator = gameObject.GetComponentInChildren<Animator>();
        zombieAgent = gameObject.GetComponent<NavMeshAgent>();
        zombieObstacle = gameObject.GetComponent<NavMeshObstacle>();
        zombieInfo = gameObject.GetComponent<CharacterInfo>();

        if(target == null) {
            target = GameObject.FindWithTag("Player");
        }

        zombieLayerMask = 1 << 9;
        zombieLayerMask = ~zombieLayerMask;

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

    void FixedUpdate() {
        CheckObstacle();
    }

    void Move() {
        float targetDistance = Vector3.Distance(target.transform.position, transform.position);
        // Debug.Log("targetDistance: " + targetDistance);

        if(nearObstacle == false) {
            Vector3 rotateDirection = target.transform.position - transform.position;
            rotateDirection.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateDirection), sphereRotateSpeed * Time.deltaTime);
        }

        if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" || zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Walk") {
            if(targetDistance > 0.8f /*1f*/) {
                isMoving = true;
                isAttacking = false;
                zombieObstacle.enabled = false;
                zombieAgent.enabled = true;
                zombieAgent.CalculatePath(target.transform.position, zombiePath);
                // Debug.Log("zombiePath.status: " + zombiePath.status);
                
                if(zombiePath.status == NavMeshPathStatus.PathComplete && nearObstacle == false /*&& nearAnotherZombie == false*/) {
                    zombieAgent.isStopped = true;
                    zombieAgent.ResetPath();
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                } else {
                    isMoving = false;
                    zombieAgent.CalculatePath(target.transform.position, zombiePath);
                    // Debug.Log("another zombiePath.status: " + zombiePath.status);
                    if(zombiePath.status == NavMeshPathStatus.PathComplete) {
                        isMoving = true;
                        zombieAgent.destination = target.transform.position;
                    }
                }

                if(isMoving) {
                    Play_Walk_Animation();
                } else {
                    Play_Idle_Animation();
                }
            } else if(targetDistance <= 0.8f /*1f*/ && transform.parent == null) {
                isAttacking = true;
                isMoving = false;
            }
        } /*else if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Attack3" && transform.parent != null) {
            // Debug.Log("Zombie bites!");
            StartCoroutine(Unparent(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
        }*/
    }

    void Attack() {
        if(isAttacking) {
            zombieAgent.enabled = false;
            zombieObstacle.enabled = true;
            if(zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" || zombieAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Walk") {
                int randomNumber = Random.Range(1, 4);
                // randomNumber = 3;
                transform.LookAt(target.transform);
                /*if(randomNumber == 3) {
                    // zombieAgent.destination = target.transform.position;
                    transform.SetParent(target.transform, true);
                }*/
                Play_Attack_Animation(randomNumber);
            }
        }
    }

    void CheckObstacle() {
        zombieRay = new Ray(zombieRaycastObject.transform.position, zombieRaycastObject.transform.forward);
        Debug.DrawRay(zombieRaycastObject.transform.position, zombieRaycastObject.transform.forward * zombieRaycastDistance, Color.red);
        if(Physics.Raycast(zombieRay, out zombieRaycastHit, zombieRaycastDistance/*, zombieLayerMask*/)) {
            if(zombieRaycastHit.collider.gameObject.CompareTag("Obstacle")) {
                nearObstacle = true;
            }
            if(zombieRaycastHit.collider.gameObject.CompareTag("Enemy")) {
                nearAnotherZombie = true;
                zombieAtFront = zombieRaycastHit.collider.gameObject;
            }
        } else {
            nearObstacle = false;
            nearAnotherZombie = false;
            zombieAtFront = null;
        }
    }

    float CalculateNavMeshAgentPathDistance() {
        float navMeshAgentDistance = 0f;
        Vector3[] corners = zombieAgent.path.corners;
        for(int i = 1; i < corners.Length; i = i + 1) {
            navMeshAgentDistance += Vector3.Distance(corners[i - 1], corners[i]);
        }
        return navMeshAgentDistance;
    }

    // Animations
    void Play_Idle_Animation() {
        zombieAnimator.Play("Idle");
    }

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
        // Debug.Log("collideObject: " + objectCollision.collider.gameObject.name);
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
        zombieObstacle.enabled = false;
        // zombieAgent.updateRotation = false;
        zombiePath = new NavMeshPath();
    }

    IEnumerator Unparent(float duration) {
        // Debug.Log("Unparent, duration: " + duration);
        yield return new WaitForSeconds(duration/2);
        transform.parent = null;
    }

    public IEnumerator AttackCoroutine(string attackType) {
        // Debug.Log("attackType: " + attackType);
        if(attackType == "attack") {
            zombieAttackCollider.enabled = true;
            yield return new WaitForSeconds(0.05f/*0.1f*/);
            // yield return new WaitForEndOfFrame();
            zombieAttackCollider.enabled = false;
        } else {
            zombieBiteCollider.enabled = true;
            yield return new WaitForSeconds(0.05f/*0.1f*/);
            // yield return new WaitForEndOfFrame();
            zombieBiteCollider.enabled = false;
        }
    }
}