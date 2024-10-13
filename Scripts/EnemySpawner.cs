using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyGO;

    //Ellenséges repülők éledési gyakorisága
    float SpawnRateCeiling = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //Folyamatos ellenséges repülők éledésének megkezdése
        Invoke ("SpawnEnemy", SpawnRateCeiling); 

        //Repülők gyakoriságának növelése
        InvokeRepeating ("IncreaseSpawnRate",0f, 30f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Ellenséges repülő éledése
    void SpawnEnemy(){

        //Ellenséges repülők éledési határai
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2( 0, 1));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));


        //Ellenséges repülő létrehozása
        GameObject enemy = (GameObject)Instantiate(EnemyGO);
        enemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y + 0.225f);

        //Következő repülő éledésének rekurzív meghívása
        NextSpawn();

    }

    //Következő ellenséges repülő éledésének időzítése
    void NextSpawn(){

        //Következő repülő konkrét éledésének ideje
        float spawnTime;

        //Idő véletlenszerű meghatározása
        if(SpawnRateCeiling > 1f){
            spawnTime = Random.Range(1f, SpawnRateCeiling);
        }else{
            spawnTime = 1f;
        }

        //Rekurzív hívás
        Invoke ("SpawnEnemy", spawnTime); 
    }

    //Idő során éledési gyakoriság növelése adott gyakoriságik (1f)
    void IncreaseSpawnRate(){
        if(SpawnRateCeiling > 1f){
            SpawnRateCeiling--;
        }else{
            CancelInvoke("IncreaseSpawnRate");
        }
    }

}
