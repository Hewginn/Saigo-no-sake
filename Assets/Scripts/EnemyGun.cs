using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;
    public GameObject Enemy2;
    // Start is called before the first frame update
    void Start()
    {

        Invoke("FireEnemyBullet", 1f);
        if (Enemy2)
        {
            Invoke("FireEnemyBullet", 3f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FireEnemyBullet()
    {
        GameObject playerShip = GameObject.Find("PlayerGO");

        if (playerShip != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);
            bullet.transform.position = transform.position;

            Vector2 direction = playerShip.transform.position - bullet.transform.position;
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }


    }
}
