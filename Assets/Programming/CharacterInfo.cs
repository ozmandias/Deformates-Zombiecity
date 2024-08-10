using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
    public int health;
    public int maxHealth;
    public CharacterType characterType;
    public ParticleSystem bloodParticle;
    public GameObject[] groundBloodObjects;
    public Behaviour[] behavioursToDisable;
    GameManager characterGameManager;
    public bool isHit = false;
    public bool isDead = false;

    void Start() {
        health = maxHealth;
        characterGameManager = GameManager.instance;
    }

    public void TakeDamage(int incomingDamage) {
        if(isDead == false && isHit == false) {
            isHit = true;
            health = health - incomingDamage;

            if(characterType == CharacterType.Player) {
                Player playerCharacter = gameObject.GetComponent<Player>();
                playerCharacter.Play_Hit_Animation();
                UIManager.instance.UpdateHealthSlider(health);
                StartCoroutine(HitCoroutine(0.1f));
            } else if(characterType == CharacterType.Enemy) {
                Zombie zombieCharacter = gameObject.GetComponent<Zombie>();
                zombieCharacter.Play_Hit_Animation();
                AnimationClip zombieHitAnimationClip = gameObject.GetComponent<Zombie>().GetAnimationClipList().Find(zombieAnimationClip => zombieAnimationClip.name == "Hit");
                StartCoroutine(HitCoroutine(0.3f /*zombieHitAnimationClip.length*/));
            }

            bloodParticle.Play();

            if(health <= 0) {
                Death();
            }
        }
    }

    void Death() {
        if(characterType == CharacterType.Player) {
            Player playerCharacter = gameObject.GetComponent<Player>();
            playerCharacter.Play_Fall_Animation();
        } else if(characterType == CharacterType.Enemy) {
            Zombie zombieCharacter = gameObject.GetComponent<Zombie>();
            zombieCharacter.Play_Death_Animation();
        }
        isDead = true;

        foreach(GameObject groundBlood in groundBloodObjects) {
            groundBlood.SetActive(true);
        }

        foreach(Behaviour behaviourToDisable in behavioursToDisable) {
            behaviourToDisable.enabled = false;
        }

        if(characterType == CharacterType.Player) {
            AnimationClip playerFallAnimationClip = gameObject.GetComponent<Player>().GetAnimationClipList().Find(playerAnimationClip => playerAnimationClip.name == "Fall");
            StartCoroutine(DeathCoroutine(playerFallAnimationClip.length));
        } else if(characterType == CharacterType.Enemy) {
            gameObject.GetComponent<Zombie>().GetTarget().GetComponent<Player>().AddScore();
            AnimationClip zombieDeathAnimationClip = gameObject.GetComponent<Zombie>().GetAnimationClipList().Find(zombieAnimationClip => zombieAnimationClip.name == "Fall");
            StartCoroutine(DeathCoroutine(zombieDeathAnimationClip.length));
        }
    }

    IEnumerator HitCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        isHit = false;
    }

    IEnumerator DeathCoroutine(float duration) {
        yield return new WaitForSeconds(duration + 1);
        if(characterType == CharacterType.Player) {
            
        } else if(characterType == CharacterType.Enemy) {
            if(characterGameManager.zombieList.Any(zombie => zombie == this.gameObject)) {
                // Debug.Log("zombie found");
                int zombiePosition = characterGameManager.zombieList.FindIndex(zombie => zombie == this.gameObject);
                // Debug.Log("zombiePosition: " + zombiePosition);
                characterGameManager.zombieList.RemoveAt(zombiePosition);
            }
            Destroy(this.gameObject);
        }
    }
}

public enum CharacterType {
    Player,
    Enemy,
    Boss
}