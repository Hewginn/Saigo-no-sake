using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Szoró ellenséges lövés
public class EnemySpreadGun : MonoBehaviour
{
    //Ellenséges lövedék
    public GameObject EnemyBulletGO;

    //A lövés szórása fokban mérve
    private float spread;

    //Egyszerre kilőtt lövedékek száma
    private int numberOfBullets; 
    
    //Első frame update előtt van meghívva
    void Start()
    {
        //Inicializálás
        spread = 20;
        numberOfBullets = 3;
        Invoke("FireEnemyBullet", 1f);
    }

    //Szóró lövés
    void FireEnemyBullet(){

        //Játékos objektumának megkeresés
        GameObject playerPlane = GameObject.Find ("PlayerGO");

        //Lövés, ha a játékos él még
        if(playerPlane != null){

            //Létrehozott lövedékek tömbje
            GameObject[] bullets = new GameObject[numberOfBullets];

            //Adott mennyiségű lövedék kilövése
            for(int i = 0; i < numberOfBullets; i++){

                //Lövedék létrehozása az adott pozícióban
                bullets[i] = (GameObject) Instantiate(EnemyBulletGO);
                bullets[i].transform.position = transform.position;
            
                //Lövés vektorának véletlenszerű elforgatása
                UnityEngine.Vector2 direction = playerPlane.transform.position - bullets[i].transform.position;
                float delta = UnityEngine.Random.Range(spread/2 * Mathf.Deg2Rad, -spread/2 * Mathf.Deg2Rad);
                direction = new UnityEngine.Vector2
                    (direction.x * Mathf.Cos(delta) - direction.y * Mathf.Sin(delta),
                    direction.x * Mathf.Sin(delta) + direction.y * Mathf.Cos(delta));
                
                //Irány továbbítása a lövedék objektumnak
                bullets[i].GetComponent<EnemyBullet>().SetDirection(direction);
            }
        }
    }
}
