using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Kis ágyú
public class BossSmallTurretControl : MonoBehaviour
{
    
    //Pontokat számláló UI szöveg
    GameObject scoreUITextGO;
    
    //Fegyver által ki lőtt lövedék
    public GameObject BulletGO;

    //A torony fegyvere
    public GameObject gun;

    //A torony életereje
    int health;

    //A játékos győzte le
    bool isDestroyedByPlayer;

    //Robbanás
    public GameObject ExplosionGO;

    //Sokszoros lövés lövedék száma
    int numberOfBullets;

    //Sokszoros lövés szórás szöge
    float spread;

    //Éledéskor inicializálás beállítása
    private void Start() {
        numberOfBullets = 3;
        spread = 20;
        health = 10;
        isDestroyedByPlayer = false;
        InvokeRepeating("FireBullet", 2f, Random.Range(3f, 8f));
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }


    //Lövés
    void FireBullet()
    {
        //Játékos objektum megkeresése
        GameObject playerPlane = GameObject.Find("PlayerGO");
        Vector2 direction = playerPlane.transform.position - gun.transform.position;

        //Ha létezik a játékos objektum lövedék lövése az irányába
        if (playerPlane != null)
        {
            if(Random.Range(0, 99) < 80){
                //Lövedék létrehozása
                GameObject bullet = (GameObject)Instantiate(BulletGO);
                bullet.transform.position = gun.transform.position;

                //Lövedék irányának megadása
                bullet.GetComponent<EnemyBullet>().SetDirection(direction);
            }else{
                //Létrehozott lövedékek tömbje
                GameObject[] bullets = new GameObject[numberOfBullets];

                //Adott mennyiségű lövedék kilövése
                for(int i = 0; i < numberOfBullets; i++){

                    //Lövedék létrehozása az adott pozícióban
                    bullets[i] = (GameObject) Instantiate(BulletGO);
                    bullets[i].transform.position = transform.position;
                
                    //Lövés vektorának véletlenszerű elforgatása
                    direction = playerPlane.transform.position - gun.transform.position;
                    float delta = UnityEngine.Random.Range(spread/2 * Mathf.Deg2Rad, -spread/2 * Mathf.Deg2Rad);
                    direction = new UnityEngine.Vector2
                        (direction.x * Mathf.Cos(delta) - direction.y * Mathf.Sin(delta),
                        direction.x * Mathf.Sin(delta) + direction.y * Mathf.Cos(delta));
                    
                    //Irány továbbítása a lövedék objektumnak
                    bullets[i].GetComponent<EnemyBullet>().SetDirection(direction);
                }
            }

            //Torony elforgatása a játékos felé
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction * -1);
        }
    }

    //Találatok ellenőrzése
    private void OnTriggerEnter2D(Collider2D col) {
        switch(col.tag){

            //Sima lövés ellenőrzése
            case "PlayerBulletTag":
                health--;
                break;

            //Különleges lövés ellenőrzése
            case "PlayerSpecialTag":
                health-=5;
                break;
            default:
                break;
        }
        
        if(health <= 0){

            //Robbanás lejátszása
            PlayerExplosion();

            //Játékos robbantotta fel
            isDestroyedByPlayer = true;

            //Törlés
            Destroy(gameObject);
        }
    }

    //Robbanást inicializáló kódrész
    void PlayerExplosion()
    {
        //Példányosítás
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        //Robbanás helyének meghatározása (objektum helye)
        explosion.transform.position = transform.position;
    }

    private void OnDestroy() {
        //Elpusztított repülők számolása
        if(isDestroyedByPlayer){

            scoreUITextGO.GetComponent<GameScore>().Score += 100;

        }
    }
}
