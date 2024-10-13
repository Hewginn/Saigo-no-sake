using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyGO;

    float SpawnRateCeiling = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke ("SpawnEnemy", SpawnRateCeiling); 

        InvokeRepeating ("IncreaseSpawnRate",0f, 30f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy(){
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2( 0, 1));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        GameObject enemy = (GameObject)Instantiate(EnemyGO);
        enemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y + 0.225f);

        NextSpawn();

    }

    void NextSpawn(){
        float spawnTime;

        if(SpawnRateCeiling > 1f){
            spawnTime = Random.Range(1f, SpawnRateCeiling);
        }else{
            spawnTime = 1f;
        }

        Invoke ("SpawnEnemy", spawnTime); 
    }

    void IncreaseSpawnRate(){
        if(SpawnRateCeiling > 1f){
            SpawnRateCeiling--;
        }else{
            CancelInvoke("IncreaseSpawnRate");
        }
    }

}
