using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject UpgradePUGO;
    public GameObject HealPUGO;
    public GameObject SpecialPUGO;

    float maxSpawnRateInSeconds = 5f;
    // ez az előre elkészített ellenség
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ez funkció az ellenségek megjelenésére
    void SpawnPowerUp()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        int powerUpSwitch = Random.Range(0, 3);

        GameObject aPowerUp = null;

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

        aPowerUp.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        ScheduleNextPowerUpSpawn();
    }
    void ScheduleNextPowerUpSpawn()
    {
        float spawnInSeconds;
        if (maxSpawnRateInSeconds > 1f)
        {
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);
        }
        else
            spawnInSeconds = 1f;

        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);



    }

    public void SchedulePowerUpSpawner()
    {
        maxSpawnRateInSeconds = 5f;

        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);

    }


    public void UnSchedulePowerUpSpawner()
    {
        CancelInvoke("SpawnPowerUp");
    }
}
