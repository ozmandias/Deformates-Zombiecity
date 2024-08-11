using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField] GameObject spawnObject;
    public bool spawnEnds = false;
    [SerializeField] int spawnStage = 0;
    [SerializeField] Transform[] spawnLocations;
    GameManager spawnGameManager;

    #region Singleton
        public static SpawnManager instance;

        void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    void Start() {
        spawnGameManager = GameManager.instance;
        
        Spawn();
    }

    void Spawn() {
        StartCoroutine(SpawnCoroutine());
    }
    
    public void StageUp() {
        if(spawnStage != 10) {
            spawnStage = spawnStage + 1;
        }
    }

    IEnumerator SpawnCoroutine() {
        while(spawnEnds == false) {
            StageUp();
            if(spawnStage == 10) {
                spawnEnds = true;
            }
            foreach(Transform spawnLocation in spawnLocations) {
                for(int i=0; i<spawnStage; i=i+1) {
                    Vector3 spawnPosition = new Vector3(spawnLocation.position.x, spawnLocation.position.y - 0.65f, spawnLocation.position.z);
                    GameObject zombieObject = Instantiate(spawnObject, spawnPosition, Quaternion.identity);
                    spawnGameManager.RegisterZombie(zombieObject);
                    yield return new WaitForSeconds(1f);
                }
            }
            yield return new WaitForSeconds(60f);
        }
    }
}