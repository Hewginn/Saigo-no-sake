using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Egyszerű ellenséges lövés
public class EnemyGun : MonoBehaviour
{
    //Lövedék
    public GameObject EnemyBulletGO;

    //Cikázó ellnfél
    public bool isEnemyZigZag;

    //Első frame update előtt van meghívva
    public void Start()
    {

        //Lövés 1 másodperc elteltével
        Invoke("FireEnemyBullet", 1f);

        //Ha az ellenfél cikázik 3 másodperc múlva is löjjön egyet
        if (isEnemyZigZag)
        {
            Invoke("FireEnemyBullet", 3f);
        }
    }

    //Lövés
    void FireEnemyBullet()
    {
        //Játékos objektum megkeresése
        GameObject playerPlane = GameObject.Find("PlayerGO");

        //Ha létezik a játékos objektum lövedék lövése az irányába
        if (playerPlane != null)
        {
            //Lövedék létrehozása
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);
            bullet.transform.position = transform.position;

            //Lövedék irányának megadása
            Vector2 direction = playerPlane.transform.position - bullet.transform.position;
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }


    }
}
