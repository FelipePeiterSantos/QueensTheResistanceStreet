using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class spawner : MonoBehaviour {

    public GameObject pref_skeleton;
    public GameObject pref_troll;
    public float timerSpawn;
    public float[] posY;

    int lastRng;
    public static List<int> spawnCount;
    int rng;
    int spawnTroll;
    int maxSpawn;

    float initialTime;
    float countFiveMinutes;

    void Start() {
        if(spawnCount == null || spawnCount.Count > 0) {
            spawnCount = new List<int>();
        }
        if(EnemiesManager.EnemiesAttacking == null || EnemiesManager.EnemiesAttacking.Count > 0) {
            EnemiesManager.EnemiesAttacking = new List<int>();
        }
        maxSpawn = 10;
        InvokeRepeating("SpawnTimer",1f,timerSpawn);
        initialTime = 300f;
    }

    void SpawnTimer() {
        if(spawnCount.Count < maxSpawn) {
            int rng = Random.Range(0,posY.Length);
            while (lastRng == rng) {
                rng = Random.Range(0,posY.Length);
            }
            if(spawnTroll > 4) {
                GameObject aux = Instantiate(pref_troll,new Vector2(transform.position.x,posY[rng]),Quaternion.identity) as GameObject;
                aux.transform.parent = transform.parent;
                spawnTroll = Random.Range(0,2);
            }
            else {
                GameObject aux = Instantiate(pref_skeleton,new Vector2(transform.position.x,posY[rng]),Quaternion.identity) as GameObject;
                aux.transform.parent = transform.parent;
                spawnTroll++;
            }
            lastRng = rng;
        }

        if(Time.time > initialTime) {
            initialTime += 60f;
            maxSpawn += 10;
            InvokeRepeating("SpawnTimer",1f,timerSpawn);
        }
    }
}
