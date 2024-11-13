using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Egyszerű ellenséges mozgás
public class EnemyControl : MonoBehaviour
{
    //Pontokat számláló UI szöveg
    GameObject scoreUITextGO;

    //Kiiktatásokat számláló UI szöveg
    GameObject killsUITextGO;

    //Logikai változó: Játékos pusztította-e el az ellenséget
    bool isDestroyedByPlayer = false;

    //Robbanás
    public GameObject ExpolsionGO;

    //Sebesség
    float speed;

    //Első frame update előtt van meghívva
    void Start()
    {
        //Objektum létrehozásakor szükséges változók inicializálása
        speed = 2f;
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
    }

    //Minden frame során megvan hívva
    void Update()
    {
        //Az ellenség helyének meghatározása
        Vector2 position = transform.position;

        //Az új pocizió megállapítása
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //Az ellenség új pozicióba helyezése
        transform.position = position;

        //A játéktér aljának meghatározása(bal alsó sarok)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //Ha elhagyja a játékteret törlődjön a repülő
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    //Ütközési esemény kezelő
    void OnTriggerEnter2D(Collider2D col)
    {
        //Játékossal való ütközési esemény megadása
        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag") || (col.tag == "PlayerSpecialTag"))
        {

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
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        //Robbanás helyének meghatározása (objektum helye)
        explosion.transform.position = transform.position;
    }

    //Konstruktor során lefutó kódrész
    private void OnDestroy() {

        //Elpusztított repülők számolása
        if(isDestroyedByPlayer){

            scoreUITextGO.GetComponent<GameScore>().Score += 100;
            killsUITextGO.GetComponent<DestroyedEnemy>().Kills += 1;
            //Robbanás lejátszása
            PlayerExplosion();

        }
        
    }
}
