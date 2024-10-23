using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Különleges lövedék
public class PlayerSpecial : MonoBehaviour
{
    //Kezdő pozíció
    Vector2 startPosition;

    //Különleges lövedék robbanása
    public GameObject specialExplosion;

    //Lövedék sebessége
    float speed;


    //Első frame update előtt van meghívva
    void Start()
    {
        speed = 6f;

        startPosition = transform.position;
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
        //ha a töltény elhagyja a játékteret vagy ha a játékos megnyomja az 'F' gombot, akkor semmisüljön meg
        if(transform.position.y > max.y || startPosition.y + 5f < transform.position.y || Input.GetKeyDown("f"))
        {
            Destroy(gameObject);
        }


    }

    //Ütközés kezelő
    void OnTriggerEnter2D(Collider2D col){

        //Ha ellenfélnek ütközik semmisüljön meg
        if(col.tag == "EnemyShipTag"){
            Destroy(gameObject);
        }

    }

    //Törlödés esetén robbanjon fel
    private void OnDestroy() {
        GameObject explosion = (GameObject) Instantiate(specialExplosion);
        explosion.transform.position = transform.position;
    }
}
