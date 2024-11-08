using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Játékos lövedék
public class PlayerBullet : MonoBehaviour
{
    //Lövedék sebessége
    float speed;

    //Első frame update előtt van meghívva
    void Start()
    {
        //Sebesség inicializálása
        speed = 8f;
    }

    //Minden frame során megvan hívva
    void Update()
    {
        // a lövedék jelenlegi helyzete
        Vector2 position = transform.position;
        // a lövedék új helyének meghatározása
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        // a lövedék új helyének beállítása
        transform.position = position;

        //ez a játék jobb felső sarka
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));
        //ha a töltény elhagyja a játékteret, akkor semmisüljön meg
        if(transform.position.y >max.y)
        {
            Destroy(gameObject);
        }


    }

    //Ütközési esemény kezelő
    void OnTriggerEnter2D(Collider2D col){

        //Lövedék elpusztítása, ha ellenfélnek ütközik
        if(col.tag == "EnemyShipTag" || col.tag == "BossEngines"){
            Destroy(gameObject);
        }
    }
}
