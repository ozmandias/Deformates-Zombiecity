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
    public bool isDead = false;

    void Start() {
        health = maxHealth;
        characterGameManager = GameManager.instance;
    }

    public void TakeDamage(int incomingDamage) {
        if(isDead == false) {
            health = health - incomingDamage;

            if(characterType == CharacterType.Player) {
                Player playerCharacter = gameObject.GetComponent<Player>();
            } else if(characterType == CharacterType.Enemy) {
                Zombie zombieCharacter = gameObject.GetComponent<Zombie>();
                zombieCharacter.Play_Hit_Animation();
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

        if(characterType == CharacterType.Enemy) {
            gameObject.GetComponent<Zombie>().GetTarget().GetComponent<Player>().AddScore();
            AnimationClip zombieDeathAnimationClip = gameObject.GetComponent<Zombie>().GetAnimationClipList().Find(zombieAnimationClip => zombieAnimationClip.name == "Fall");
            StartCoroutine(ZombieDeathCoroutine(zombieDeathAnimationClip.length));
        }
    }

    IEnumerator ZombieDeathCoroutine(float duration) {
        yield return new WaitForSeconds(duration + 1);
        if(characterGameManager.zombieList.Any(zombie => zombie == this.gameObject)) {
            // Debug.Log("zombie found");
            int zombiePosition = characterGameManager.zombieList.FindIndex(zombie => zombie == this.gameObject);
            // Debug.Log("zombiePosition: " + zombiePosition);
            characterGameManager.zombieList.RemoveAt(zombiePosition);
        }
        Destroy(this.gameObject);
    }
}

public enum CharacterType {
    Player,
    Enemy,
    Boss
}