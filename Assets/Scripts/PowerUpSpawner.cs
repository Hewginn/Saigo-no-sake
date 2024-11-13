using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Felvehető tárgyak létrehozása
public class PowerUpSpawner : MonoBehaviour
{
    //Fejlesztések
    public GameObject [] PowerUps;

    //Max létrehozási gyakoriság
    float maxSpawnRateInSeconds = 15f;

    //ez funkció az ellenségek megjelenésére
    void SpawnPowerUp()
    {
        //Létrehozási határok
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //Egy véletelen powerup létrehozása
        GameObject aPowerUp = (GameObject)Instantiate(PowerUps[Random.Range(0, PowerUps.Length)]);

        //Létrehozás véletelnszerű helyen a határok közöttt
        aPowerUp.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //Kövekező tárgy rekurzív ütemezése
        Invoke("SpawnPowerUp", Random.Range(8f, maxSpawnRateInSeconds));
    }

    //Első létreghozás ütemezése
    public void SchedulePowerUpSpawner()
    {

        //Létrehozás meghívás x mp-en belül
        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);

    }

    //Létrehozás befejezése
    public void UnSchedulePowerUpSpawner()
    {
        CancelInvoke("SpawnPowerUp");
    }
}
