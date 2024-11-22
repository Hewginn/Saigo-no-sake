using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.IO;
using TMPro;

public class BomberBossControl : MonoBehaviour
{
    //Főellenség mozgása
    float speed = 1f;

    //Hajtóművek száma
    int numberOfEngines;

    //Ellenséges lövedék
    public GameObject EnemyBullet;

    //Robbanás lejátszó
    public GameObject ExplosionGO;

    //
    bool isDying;

    //Hajtómű szám getter, setter
    public int NumberOfEngines{
        get{
            return this.numberOfEngines;
        }
        set{
            this.numberOfEngines = value;
            //Hajtómű szám módosítása esetén, ha 0 akkor a gép felrobban
            if(numberOfEngines <= 0){
                DestroyFinalBoss();
            }
        }
    }

    //Első képkocka előtt kerül meghívásra
    void Start(){

        //Hajtóművek számának inicializálása
        numberOfEngines = 4;

        InvokeRepeating("MoveToStartingPosition", 0f, Time.deltaTime);

        isDying = false;
    }
    
    // Harc megkezdése
    void StartOfFight()
    {

        //Jobbra balre mozgás megkezdése
        InvokeRepeating("Move",0f, Time.deltaTime);

        //Támadások megkezdése
        Invoke("actions", 3f);
    }

    //Jobbra balra mozgás
    void Move(){

        //x-tengelyen mozgás megadása
        Vector2 position = transform.position;
        position = new Vector2(position.x + Time.deltaTime * speed, position.y);

        //Képernyő határainak megadása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1,1));

        float halfOfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;

        //Határok alkalmazása
        if(position.x - halfOfWidth < min.x || position.x + halfOfWidth > max.x){
            //Megfordulás ha elérte a határt
            speed *= -1;
        }else{
            //Új pozíció megadása
            transform.position = position;
        }
    }

    //Főellenség bemozgása a képernyőbe
    void MoveToStartingPosition(){

        //Főellenség Y pozíció célja
        float wantedY = Camera.main.ViewportToWorldPoint(new Vector2(1,(float)0.5)).y + 1.5f;

        //Mozgás a pozíció felé vagy, ha elérte elkezdeni jobbra és balra mozogni
        if(transform.position.y >= wantedY){
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime); 
        }else{
            CancelInvoke("MoveToStartingPosition");
            StartOfFight();
        }
    }

    //Főellenség elpusztítása
    public void DestroyFinalBoss(){


        //Mozgás befejezése
        CancelInvoke("Move");

        //Lövések befejezése
        StopAllCoroutines();
        CancelInvoke("actions");
        
        //Maradék ágyúk felrobbantása
        foreach(Transform child in gameObject.transform){
            Destroy(child.gameObject);
        }

        GameObject player = GameObject.Find("PlayerGO");
        player.GetComponent<PlayerControl>().loseControl();

        isDying = true;

        GameObject.Find("GameManagerGO").GetComponent<GameManager>().WriteFinalMessage();
    }

    //Kamikáze ütközés érzékelő
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "PlayerUndamagable" && isDying){
            PlayerExplosion();
            Destroy(gameObject);
            GameObject.Find("GameManagerGO").GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.Ending);
        }
    }

    //Főellenség támadásai
    public void actions(){

        //Támadás véletlenszerű kiválasztása
        int actionSelector = Random.Range(1,4);
        
        switch(actionSelector){

            //Hajtóművek sugárlövése
            case 1:

                //Hajtóművek megkeresése
                EngineControl [] engines = gameObject.transform.GetComponentsInChildren<EngineControl>();
                foreach(EngineControl engine in engines){

                    //Sugár kilövése
                    engine.engineBlast();

                }

                //Következő támadás időzítése
                Invoke("actions", 3f);

                break;

            //Szort lövés
            case 2:

                //Szoró lövés meghívása corutine formában
                StartCoroutine(BulletRain());
                break;

            //Nagy ágyú folyamatos lövése
            case 3:

                //Ellenőrzés: el lette-e már pusztítva a nagy ágyú
                if(gameObject.transform.GetComponentInChildren<BossBigTurretControl>() != null){

                    //Lövés megkezdése
                    gameObject.transform.GetComponentInChildren<BossBigTurretControl>().HellFireTrigger();

                }else{

                    //Függvény újra hívása
                    Invoke("actions", 0f);

                }
                break;
        }
    }

    //Szort lövés
    private IEnumerator BulletRain(){

        //Páratlan sor-e, az eltolás miatt
        bool oddRow = false;

        //Lövés pozíviójának mmeghatározására új objektum megadása
        GameObject shootPosition = new GameObject("shootPosition");

        //Boss SpriteRenderer a boss határaihoz
        SpriteRenderer bomber = gameObject.GetComponent<SpriteRenderer>();

        //Lövés pozíció
        shootPosition.transform.position = new Vector2(
            transform.position.x - bomber.bounds.size.x / 2,
            transform.position.y);

        //Lövés pozíció beállítása a boss gyerekének, hogy kövesse a mozgását (relatív pozíciója ne változzon)
        shootPosition.transform.SetParent(gameObject.transform, true);

        //10 sor lövése
        for(int i = 0; i < 10; i++){
                    
            //Sor lövése  
            while(shootPosition.transform.position.x <= transform.position.x + bomber.bounds.size.x / 2){

                //Lövedék inicializálása
                GameObject bullet = Instantiate(EnemyBullet);
                bullet.transform.position = shootPosition.transform.position;
                bullet.GetComponent<EnemyBullet>().SetDirection(new Vector2(0, -1));
                bullet.GetComponent<EnemyBullet>().Speed = 2f;

                //Lövés pozíció eltolása a következő pontra
                shootPosition.transform.position = new Vector2(
                    shootPosition.transform.position.x + 1.25f,
                    shootPosition.transform.position.y
                );

            }

            // A következő sor kezdő lövés pozíciójának meghatározása
            oddRow = !oddRow;
            shootPosition.transform.position = new Vector2(
                transform.position.x - bomber.bounds.size.x / 2 + (oddRow ? 0.5f : -0.5f),
                transform.position.y
            );

            //Várakozás a következő sorral
            yield return new WaitForSeconds((float)0.5);
        }

        //Lövés pozíció törlése
        Destroy(shootPosition);

        //Következő támadás meghívása
        Invoke("actions", 3f);
    }

    //Robbanást inicializáló kódrész
    void PlayerExplosion()
    {
        //Példányosítás
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        explosion.transform.localScale = new Vector3(5f, 5f, 1f); 

        //Robbanás helyének meghatározása (objektum helye)
        explosion.transform.position = transform.position;
    }
}
