using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {
    [SerializeField] Animator playerAnimator;
    [SerializeField] Weapon playerWeapon;
    [SerializeField] NavMeshAgent playerAgent;
    [SerializeField] float speed = 4f;
    [SerializeField] float rotateSpeed = 8f;
    [SerializeField] float lookRotateSpeed = 16f;
    float rotateAngle = 0;
    [SerializeField] GameObject playerGunRaycastObject;
    Ray gunRay;
    RaycastHit gunRayHit;
    CharacterInfo playerInfo;
    int shootCount = 0;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isShooting = false;
    [SerializeField] bool isReloading = false;
    [SerializeField] bool isHurt = false;
    [SerializeField] bool autoAim = false;
    [SerializeField] GameObject nearestZombie;
    float autoAimDistance = 0;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] List<AnimationClip> playerAnimationClipList = new List<AnimationClip>();
    [SerializeField] GameManager playerGameManager;
    [SerializeField] UIManager playerUIManager;
    [SerializeField] int resetCount = 0;
    [SerializeField] int maximumResetCount = 1;
    int totalKills = 0;

    void Start() {
        playerAnimator = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        playerWeapon = gameObject.GetComponentInChildren<Weapon>();
        foreach(AnimationClip runtimeAnimationClip in playerAnimator.runtimeAnimatorController.animationClips) {
            if(playerAnimationClipList.Contains(runtimeAnimationClip) == false) {
                playerAnimationClipList.Add(runtimeAnimationClip);
            }
        }
        playerInfo = gameObject.GetComponent<CharacterInfo>();
        playerGameManager = GameManager.instance;
        playerUIManager = UIManager.instance;
    }

    void Update() {
        #if UNITY_EDITOR
            Move();
            Shoot();
            Reload();
            // MobileMove();
            // MobileShoot();
            // MobileReload();
        #elif UNITY_ANDROID
            MobileMove();
            MobileShoot();
            MobileReload();
        #endif
        CheckNearestZombie();
        // Debug.Log("currentAnimationClip: " + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        // Debug.Log("transform.forward: " + transform.forward);
    }

    void FixedUpdate() {
        CheckPhysics();
    }

    void Move() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        if((horizontal < 1f && horizontal > 0) || (horizontal > -1f && horizontal < 0) || (vertical < 1f && vertical > 0) || (vertical > -1f && vertical < 0)) {
            isMoving = true;
            rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            Play_Run_Animation();
        } else if(horizontal == 0 && vertical == 0) {
            isMoving = false;
            Stop_Run_Animation();
        }
        
        if(autoAim == true && nearestZombie != null){
            // transform.LookAt(nearestZombie.transform);
            Vector3 rotateDirection = nearestZombie.transform.position - gameObject.transform.position;
            Vector3 rotateTowards = Vector3.RotateTowards(transform.forward, rotateDirection, rotateSpeed, 0);
            Quaternion lookRotation = Quaternion.LookRotation(rotateTowards);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookRotateSpeed * Time.deltaTime) /*lookRotation*/;
        } else if(autoAim == false) {
            Quaternion rotation = Quaternion.Euler(0, rotateAngle, 0);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime) /*rotation*/;
        }

        gameObject.transform.position = gameObject.transform.position + direction;
    }

    void MobileMove() {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        if((horizontal < 1f && horizontal > 0) || (horizontal > -1f && horizontal < 0) || (vertical < 1f && vertical > 0) || (vertical > -1f && vertical < 0)) {
            isMoving = true;
            rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
            Play_Run_Animation();
        } else if(horizontal == 0 && vertical == 0) {
            isMoving = false;
            Stop_Run_Animation();
        }

        if(autoAim == true && nearestZombie != null){
            // transform.LookAt(nearestZombie.transform);
            Vector3 rotateDirection = nearestZombie.transform.position - gameObject.transform.position;
            Vector3 rotateTowards = Vector3.RotateTowards(transform.forward, rotateDirection, rotateSpeed, 0);
            Quaternion lookRotation = Quaternion.LookRotation(rotateTowards);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, lookRotateSpeed * Time.deltaTime) /*lookRotation*/;
        } else if(autoAim == false) {
            Quaternion rotation = Quaternion.Euler(0, rotateAngle, 0);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime) /*rotation*/;
        }

        gameObject.transform.position = gameObject.transform.position + direction;
    }

    void Shoot() {
        if(nearestZombie != null) {
            autoAimDistance = Vector3.Distance(nearestZombie.transform.position, gameObject.transform.position);
        } else {
            autoAimDistance = 0;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            if(playerWeapon.currentGun.currentBullets > 0 && isReloading == false) {
                // Debug.Log("Shoot");
                isShooting = true;
                shootCount = shootCount + 1;
                if(autoAimDistance > 0 && isShooting == true && autoAimDistance <= maxDistance && nearestZombie != null) {
                    autoAim = true;
                }
                Play_Shoot_Animation();
                playerWeapon.currentGun.PlayGunParticles();
                playerWeapon.currentGun.ReduceBullet();
                AnimationClip shootAnimationClip = playerAnimationClipList.Find(playerAnimationClip => playerAnimationClip.name == "HumanShoot");
                StartCoroutine(ResetProperty("shoot", shootAnimationClip.length));
                playerUIManager.UpdateCurrentBulletsText(playerWeapon.currentGun.currentBullets);
            }
        }
        /*if(isShooting == true) {
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shoot")) {
                // Debug.Log("ShootState Animation: " + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                // if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Shoot") {
                //     StartCoroutine(ResetProperty("shoot", playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                // }
                if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanShoot") {
                    StartCoroutine(ResetProperty("shoot", playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                }
            } else if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunShootBlendTree")) {
                // Debug.Log("RunShootBlendTree Animation: " + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Run") {
                    StartCoroutine(ResetProperty("shoot", playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                }
            }
        }*/
        if(autoAimDistance == 0 || isShooting == false) {
            autoAim = false;
        }
    }

    void MobileShoot() {
        if(nearestZombie != null) {
            autoAimDistance = Vector3.Distance(nearestZombie.transform.position, gameObject.transform.position);
        } else {
            autoAimDistance = 0;
        }
        if(CrossPlatformInputManager.GetButtonDown("Shoot")) {
            // Debug.Log("MobileShoot");
            isShooting = true;
            shootCount = shootCount + 1;
            if(autoAimDistance > 0 && isShooting == true && autoAimDistance <= maxDistance && nearestZombie != null) {
                autoAim = true;
            }
            Play_Shoot_Animation();
            playerWeapon.currentGun.PlayGunParticles();
            playerWeapon.currentGun.ReduceBullet();
            AnimationClip shootAnimationClip = playerAnimationClipList.Find(playerAnimationClip => playerAnimationClip.name == "HumanShoot");
            StartCoroutine(ResetProperty("shoot", shootAnimationClip.length));
            playerUIManager.UpdateCurrentBulletsText(playerWeapon.currentGun.currentBullets);
        }
        if(autoAimDistance == 0 || isShooting == false) {
            autoAim = false;
        }
    }

    void Reload() {
        if(Input.GetKeyDown(KeyCode.R)) {
            if(playerWeapon.currentGun.currentBullets < playerWeapon.currentGun.maxBullets && isShooting == false && isReloading == false) {
                // Debug.Log("Reload");
                isReloading = true;
                Play_Reload_Animation();
                playerWeapon.currentGun.AddBullets();
                AnimationClip reloadAnimationClip = playerAnimationClipList.Find(playerAnimationClip => playerAnimationClip.name == "Reload");
                StartCoroutine(ResetProperty("reload", reloadAnimationClip.length));
            }
        }
        /*if(isReloading == true) {
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload")) {
                // Debug.Log("ReloadState Animation: " + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Reload") {
                    StartCoroutine(ResetProperty("reload", playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                }
            } else if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunReloadBlendTree")) {
                // Debug.Log("RunReloadBlendTree Animation: " + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Run") {
                    StartCoroutine(ResetProperty("reload", playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length));
                }
            }
        }*/
    }

    void MobileReload() {
        if(CrossPlatformInputManager.GetButtonDown("Reload")) {
            if(playerWeapon.currentGun.currentBullets < playerWeapon.currentGun.maxBullets && isShooting == false && isReloading == false) {
                // Debug.Log("MobileReload");
                isReloading = true;
                Play_Reload_Animation();
                playerWeapon.currentGun.AddBullets();
                AnimationClip reloadAnimationClip = playerAnimationClipList.Find(playerAnimationClip => playerAnimationClip.name == "Reload");
                StartCoroutine(ResetProperty("reload", reloadAnimationClip.length));
            }
        }
    }

    void CheckNearestZombie() {
        // Debug.Log("CheckNearestZombie");
        if(playerGameManager.zombieList.Count > 0) {
            int nearestCount = 0;
            float nearestDistance = 0;
            foreach(GameObject zombie in playerGameManager.zombieList) {
                float checkDistance = Vector3.Distance(zombie.transform.position, gameObject.transform.position);
                if(nearestCount == 0) {
                    nearestDistance = checkDistance;
                    if(zombie.GetComponent<Zombie>().GetCharacterInfo().isDead == false) {
                        nearestZombie = zombie;
                        nearestCount = nearestCount + 1;
                    }
                } else {
                    if(checkDistance < nearestDistance) {
                        nearestDistance = checkDistance;
                        if(zombie.GetComponent<Zombie>().GetCharacterInfo().isDead == false) {
                            nearestZombie = zombie;
                        }
                    }
                }
            }
            if(nearestZombie != null) {
                float confirmDistance = Vector3.Distance(nearestZombie.transform.position, gameObject.transform.position);
                // Debug.Log("confirmDistance: " + confirmDistance);
                if(confirmDistance > maxDistance) {
                    nearestZombie = null;
                }
            }
        }
    }

    void CheckPhysics() {
        if(isShooting == true && shootCount == 1) {
            gunRay = new Ray(playerGunRaycastObject.transform.position, playerGunRaycastObject.transform.forward);
            // Debug.DrawLine(gunRay.origin, gunRay.direction, Color.white);
            if(Physics.Raycast(gunRay, out gunRayHit, maxDistance)) {
                // Debug.Log("hit: " + gunRayHit.collider.gameObject);
                if(gunRayHit.collider.gameObject.CompareTag("Enemy")) {
                    Debug.Log("enemyHit");
                    CharacterInfo zombieCharacterInfo = gunRayHit.collider.gameObject.GetComponent<CharacterInfo>();
                    zombieCharacterInfo.TakeDamage(playerWeapon.currentGun.gunDamage);
                    shootCount = 0;
                }
            }
        } else if(isShooting == false) {
            shootCount = 0;
        }
    }

    public void AddScore() {
        totalKills = totalKills + 1;
        playerUIManager.UpdateTotalKillsText(totalKills);
    }

    // Animations
    void Play_Run_Animation() {
        playerAnimator.SetBool("Run", true);
    }

    void Stop_Run_Animation() {
        playerAnimator.SetBool("Run", false);
    }

    void Play_Shoot_Animation() {
        playerAnimator.SetTrigger("Shoot");
        /*if(isMoving == true) {
            playerAnimator.SetFloat("RunShootBlend", 0.5f);
        }*/
    }

    void Play_Reload_Animation() {
        playerAnimator.SetTrigger("Reload");
    }

    // Collisions
    void OnCollisionEnter(Collision objectCollision) {

    }

    // Coroutines
    IEnumerator ResetProperty(string propertyName, float duration) {
        // Debug.Log("ResetProperty");
        resetCount = resetCount + 1;
        float resetDuration = 0;
        resetDuration = maximumResetCount / 10f;
        yield return new WaitForSeconds(resetDuration /*0.2f*/);
        
        if(resetCount == 1) {
            // Debug.Log("SingleReset");
        } else if(resetCount > 1) {
            // Debug.Log("MultipleReset");
            resetCount = 0;
            StartCoroutine(ResetProperty(propertyName, duration));
            yield break;
        }

        yield return new WaitForSeconds(duration);

        // if(resetCount == 1) {
            switch(propertyName) {
                case "move":
                    isMoving = false;
                    break;
                
                case "shoot":
                    // Debug.Log("shootReset");
                    isShooting = false;
                    break;

                case "reload":
                    // Debug.Log("reloadReset");
                    playerUIManager.UpdateCurrentBulletsText(playerWeapon.currentGun.currentBullets);
                    isReloading = false;
                    break;

                default:
                    break;
            }
        // }

        resetCount = 0;
    }
}