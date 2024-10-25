using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using math = System.Math;

//Cikázó ellenség
public class EnemyZigZagControl : MonoBehaviour
{
    //Pontszámoló UI szöveg
    GameObject scoreUITextGO;

    //Kiiktatást számoló UI szöveg
    GameObject killsUITextGO;

    //Robbanás objektum
    public GameObject ExpolsionGO;

    //Ellenség sebessége
    float speed;

    //Logikai változó: ellenség játékos által lett-e elpusztítva
    bool isDestroyedByPlayer = false;

    //Első frame update előtt van meghívva
    void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            speed = 2f;
        }
        else
        {
            speed = -2f;
        }

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
    }

    //Minden frame során megvan hívva (Unity függvény)
    void Update()
    {
        //Az ellenség helyének meghatározása
        Vector2 position = transform.position;

        //Az új pocizió megállapítása
        position = new Vector2(position.x - speed * Time.deltaTime * 2, position.y - math.Abs(speed) * Time.deltaTime / 2);

        //Az ellenség új pozicióba helyezése
        transform.position = position;

        //A játéktér aljának meghatározása(bal alsó sarok)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        
        //Ha elhagyja a játékteret törlődjön a hajó vagy pattanjon vissza
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
        if ((transform.position.x < min.x) || (transform.position.x > max.x))
        {
            speed *= -1;
        }
    }

    //Ütközés kezelő
    void OnTriggerEnter2D(Collider2D col)
    {
        //Játékossal való ütközési esemény megadása
        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag") || (col.tag == "PlayerSpecialTag"))
        {
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
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        //Robbanás helyének meghatározása(objektum helye)
        explosion.transform.position = transform.position;
    }

    //Konstruktor során lefutó kódrész
    private void OnDestroy() {
        //Elpusztított repülők számolása
        if(isDestroyedByPlayer){
            scoreUITextGO.GetComponent<GameScore>().Score += 100;
            killsUITextGO.GetComponent<DestroyedEnemy>().Kills += 1;
        }
    }
}
