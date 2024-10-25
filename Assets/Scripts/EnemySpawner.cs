using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ellenségek létrehozását kezelő
public class EnemySpawner : MonoBehaviour
{
    //Első ellenség fajta
    public GameObject EnemyGO;

    //Második ellenség fajta
    public GameObject EnemyGO2;

    //Harmadik ellenség fajta
    public GameObject EnemyGO3;

    //Ellenség létrehozási gyakoriság (alap: 5mp)
    float maxSpawnRateInSeconds;

    //Funkció az ellenség létrehozására
    void SpawnEnemy()
    {
        //Határok meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Első ellenség létrehozása
        GameObject anEnemy = (GameObject)Instantiate(EnemyGO);
        anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Második ellenség létrehozása
        GameObject anEnemy2 = (GameObject)Instantiate(EnemyGO2);
        anEnemy2.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Harmadik ellenség létrehozása
        GameObject anEnemy3 = (GameObject)Instantiate(EnemyGO3);
        anEnemy3.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Következő létrehozás rekurzív hívása
        ScheduleNextEnemySpawn();
    }

    //Létrehozás ütemezés
    void ScheduleNextEnemySpawn()
    {
        //Létrehozás ütemezési ideje
        float spawnInSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            //Véletelnszerű ütemezés 1mp és a maximum Létrehozási gyakoriság között
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else{
            //Létrehozási ütemezés ideje minimum 1mp
            spawnInSeconds = 1f;
        }

        //Létrehozás rekurzív meghívása
        Invoke("SpawnEnemy", spawnInSeconds);
    }

    //Létrehozási gyakoriság növelése, amíg 1mp nem lesz
    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > 1f)
            maxSpawnRateInSeconds--;
        if (maxSpawnRateInSeconds == 1f)
            CancelInvoke("IncreaseSpawnRate");
    }

    //Ellenségek létrehozásának megkezdése
    public void ScheduleEnemySpawner()
    {
        
        maxSpawnRateInSeconds = 5f;
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        InvokeRepeating("IncreaseSpawnRate", 0f, 30f);

    }

    //Ellnségek létrehozásának megszakítása
    public void UnScheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }
}
