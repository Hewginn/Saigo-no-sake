using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ellenségek létrehozását kezelő
public class EnemySpawner : MonoBehaviour
{
    //Már megtörtént spawnolások száma
    int numberOfSpawns;

    //Ellenségek listája
    public GameObject [] enemies;

    //Ellenség létrehozási gyakoriság (alap: 5mp)
    float maxSpawnRateInSeconds;

    //Maximum létrehozott ellenségek száma
    int maxNumberOfEnemies;

    //Minimum létrehozott ellenségek száma
    int minNumberOfEnemies;

    //Funkció az ellenség létrehozására
    void SpawnEnemy()
    {
        //Határok meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        for(int i = 0; i < Random.Range(minNumberOfEnemies,maxNumberOfEnemies); i++){
            GameObject anEnemy = (GameObject)Instantiate(enemies[Random.Range(0, enemies.Length)]);
            anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
        }

        //Következő létrehozás rekurzív hívása
        ScheduleNextEnemySpawn();
    }

    //Létrehozás ütemezés
    void ScheduleNextEnemySpawn()
    {

        //Következő spawn száma
        numberOfSpawns++;

        //Létrehozás ütemezési ideje
        float spawnInSeconds;

        if(numberOfSpawns % 5 == 0){
            maxNumberOfEnemies++;
        }

        if(numberOfSpawns % 10 == 0){
            minNumberOfEnemies++;
        }

        if(numberOfSpawns % 15 == 0){
            IncreaseSpawnRate();
        }

        
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
        if (maxSpawnRateInSeconds > 5f)
            maxSpawnRateInSeconds--;
        if (maxSpawnRateInSeconds == 5f)
            CancelInvoke("IncreaseSpawnRate");
    }

    //Ellenségek létrehozásának megkezdése
    public void ScheduleEnemySpawner()
    {
        numberOfSpawns = 1;
        minNumberOfEnemies = 1;
        maxNumberOfEnemies = 2;
        maxSpawnRateInSeconds = 10f;
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
    }

    //Ellnségek létrehozásának megszakítása
    public void UnScheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }
}
