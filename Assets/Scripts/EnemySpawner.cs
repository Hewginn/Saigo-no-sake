using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Ellenségek létrehozását kezelő
public class EnemySpawner : MonoBehaviour
{
    public GameObject GameManager;
    //Már megtörtént spawnolások száma
    int numberOfSpawns;

    //Ellenségek listája
    public GameObject[] enemies;

    //Ellenség létrehozási gyakoriság (alap: 5mp)
    float maxSpawnRateInSeconds;

    //Maximum létrehozott ellenségek száma
    int maxNumberOfEnemies;

    //Minimum létrehozott ellenségek száma
    int minNumberOfEnemies;

    //Funkció az ellenség létrehozására

    string jsonFile;// a json fájl elérési útvonala

    Missions data; //a történetet tárolja a json fájlból

    int i; //a nehézségi szint száma a tömbben
    void SpawnEnemy()
    {
        //Határok meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        for (int i = 0; i < Random.Range(minNumberOfEnemies, maxNumberOfEnemies); i++)
        {
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

        //Maximum ellenség inkrementálása
        if (numberOfSpawns % 5 == 0)
        {
            maxNumberOfEnemies++;
        }

        //Minimum ellenség inkrementálása
        if (numberOfSpawns % 10 == 0)
        {
            minNumberOfEnemies++;
        }

        //Éledési gyakoriság inkrementálása
        if (numberOfSpawns % 10 == 0)
        {
            IncreaseSpawnRate();
        }


        if (maxSpawnRateInSeconds > 1f)
        {
            //Véletelnszerű ütemezés 1mp és a maximum Létrehozási gyakoriság között
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else
        {
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
        minNumberOfEnemies = data.difficulty[i].min_number_of_enemy;
        maxNumberOfEnemies = 2;
        maxSpawnRateInSeconds = data.difficulty[i].maxspawntime;
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
    }

    //Ellnségek létrehozásának megszakítása
    public void UnScheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }

    void Start()
    {
        // a beolvasott fájl útvonala
        jsonFile = File.ReadAllText(Application.dataPath + "/Resources/story.json");
        // a beolvasott fájl adatait eltároló változó
        data = JsonUtility.FromJson<Missions>(jsonFile);
        // a nehézségi szint kiválasztása
        for (i = 0; i < data.difficulty.Length; i++)
        {
            if (data.difficulty[i].type == data.choosed_difficulty)
            {
                break;
            }
        }
    }
}
