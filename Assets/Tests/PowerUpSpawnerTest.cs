using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Felvehető tárgyak létrehozása
public class PowerUpSpawner : MonoBehaviour
{
    //Fejlesztés
    public GameObject UpgradePUGO;

    //Gyógyítás
    public GameObject HealPUGO;

    //Különleges lövedék
    public GameObject SpecialPUGO;

    //Max létrehozási gyakoriság
    float maxSpawnRateInSeconds;

    //ez funkció az ellenségek megjelenésére
    void SpawnPowerUp()
    {
        //Létrehozási határok
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Egy véletelenszerű tárgy kiválasztására szám
        int powerUpSwitch = Random.Range(0, 3);

        //A tárgy
        GameObject aPowerUp = null;

        //A tárgy értékadása
        switch(powerUpSwitch){
            case 0:
                aPowerUp = (GameObject)Instantiate(UpgradePUGO);
                break;
            case 1:
                aPowerUp = (GameObject)Instantiate(HealPUGO);
                break;
            case 2:
                aPowerUp = (GameObject)Instantiate(SpecialPUGO);
                break;
        }

        //Létrehozás véletelnszerű helyen a határok közöttt
        aPowerUp.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Kövekező tárgy rekurzív ütemezése
        ScheduleNextPowerUpSpawn();
    }

    //Következő tárgy ütemezése
    void ScheduleNextPowerUpSpawn()
    {
        //Létrehozási idő mp-ben
        float spawnInSeconds;

        //értéke legalább 1mp
        if (maxSpawnRateInSeconds > 1f)
        {   
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else{
            spawnInSeconds = 1f;
        }

        //Létrehozás rekurzív meghívása x mp-en belül
        Invoke("SpawnPowerUp", spawnInSeconds);
    }

    //Első létreghozás ütemezése
    public void SchedulePowerUpSpawner()
    {
        //Max létrehozási gyakoriság értékadás
        maxSpawnRateInSeconds = 5f;

        //Létrehozás meghívás x mp-en belül
        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);

    }

    //Létrehozás befejezése
    public void UnSchedulePowerUpSpawner()
    {
        CancelInvoke("SpawnPowerUp");
    }
}
