using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpreadGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;

    private float spread = 20; //A lövés szórása fokban mérve

    private int numberOfBullets = 3; //Egyszerre kilőtt lövedékek száma
    


    // Start is called before the first frame update
    void Start()
    {
        Invoke("FireEnemyBullet", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireEnemyBullet(){
        GameObject playerShip = GameObject.Find ("PlayerGO");
        if(playerShip != null){

            GameObject[] bullets = new GameObject[numberOfBullets];

            //Adott mennyiségű lövedék kilövése
            for(int i = 0; i < numberOfBullets; i++){

                //Lövedék létrehozása az adott pozícióban
                bullets[i] = (GameObject) Instantiate(EnemyBulletGO);
                bullets[i].transform.position = transform.position;
            
                //Lövés vektorának véletlenszerű elforgatása
                UnityEngine.Vector2 direction = playerShip.transform.position - bullets[i].transform.position;
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
