using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecial : MonoBehaviour
{
    
    Vector2 startPosition; //Kezdő pozíció
    public GameObject specialExplosion;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 6f;

        startPosition = transform.position;
    }

    // Update is called once per frame
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
        if(transform.position.y > max.y || startPosition.y + 5f < transform.position.y || Input.GetKeyDown("f"))
        {
            Destroy(gameObject);
        }


    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "EnemyShipTag"){
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameObject explosion = (GameObject) Instantiate(specialExplosion);
        explosion.transform.position = transform.position;
    }
}
