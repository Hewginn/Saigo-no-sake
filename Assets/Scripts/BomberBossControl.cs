using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BomberBossControl : MonoBehaviour
{
    //Főellenség mozgása
    float speed = 1f;

    //Hajtóművek száma
    int numberOfEngines;

    //Ellenséges lövedék
    public GameObject EnemyBullet;

    public int NumberOfEngines{
        get{
            return this.numberOfEngines;
        }
        set{
            this.numberOfEngines = value;
            if(numberOfEngines <= 0){
                DestroyFinalBoss();
                GameObject.Find("GameManagerGO").GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.Win);
            }
        }
    }

    //Első képkocka előtt kerül meghívásra
    void Start(){

        //Hajtóművek számának inicializálása
        numberOfEngines = 4;

        InvokeRepeating("MoveToStartingPosition", 0f, Time.deltaTime);
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
        
        //Objektum törlése
        Destroy(gameObject);
    }

    //Főellenség támadásai
    public void actions(){
        int actionSelector = Random.Range(1,4);
        switch(actionSelector){

            //Hajtóművek sugárlövése
            case 1:
                EngineControl [] engines = gameObject.transform.GetComponentsInChildren<EngineControl>();
                foreach(EngineControl engine in engines){
                    engine.engineBlast();
                }
                Invoke("actions", 3f);
                break;

            //Szoró lövés
            case 2:
                StartCoroutine(BulletRain());
                break;
            case 3:
                if(gameObject.transform.GetComponentInChildren<BossBigTurretControl>() != null){
                    gameObject.transform.GetComponentInChildren<BossBigTurretControl>().HellFireTrigger();
                }else{
                    Invoke("actions", 0f);
                }
                break;
        }
    }

    //Szort lövés
    private IEnumerator BulletRain(){

        bool oddRow = false;

        GameObject shootPosition = new GameObject("shootPosition");
        SpriteRenderer bomber = gameObject.GetComponent<SpriteRenderer>();
        shootPosition.transform.position = new Vector2(
            transform.position.x - bomber.bounds.size.x / 2,
            transform.position.y);

        shootPosition.transform.SetParent(gameObject.transform, true);

        for(int i = 0; i < 10; i++){
                    
                    
            while(shootPosition.transform.position.x <= transform.position.x + bomber.bounds.size.x / 2){

                GameObject bullet = Instantiate(EnemyBullet);
                bullet.transform.position = shootPosition.transform.position;
                bullet.GetComponent<EnemyBullet>().SetDirection(new Vector2(0, -1));
                bullet.GetComponent<EnemyBullet>().Speed = 2f;

                shootPosition.transform.position = new Vector2(
                    shootPosition.transform.position.x + 1.25f,
                    shootPosition.transform.position.y
                );

            }

            oddRow = !oddRow;
            shootPosition.transform.position = new Vector2(
                transform.position.x - bomber.bounds.size.x / 2 + (oddRow ? 0.5f : -0.5f),
                transform.position.y
            );
            yield return new WaitForSeconds((float)0.5);
        }
        Destroy(shootPosition);
        Invoke("actions", 3f);
    }


}
