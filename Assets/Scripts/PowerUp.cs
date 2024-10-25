using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Felvehető tárgyak
public class PowerUp : MonoBehaviour
{
    //Tárgyak sebessége
    float speed;

    //Első frame update előtt van meghívva
    void Start()
    {
        //Sebesség inicializálása
        speed = 2f;
    }

    //Minden frame során megvan hívva
    void Update()
    {
        //Mozgás
        MoveForward();
    }

    //Az objektum folyamatos lefele mozgását leíró függvény
    void MoveForward(){

        //Régi pozíció
        Vector2 position = transform.position;

        //Határ meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0,0));

        //Új pozíció megadása
        position.y -= speed * Time.deltaTime;
        transform.position = position;
        
        //Határ alkalmazása
        if(transform.position.y < min.y){
            Destroy(gameObject);
        }

    }

    //Ütközés kezelő
    void OnTriggerEnter2D(Collider2D col){

        //Ha a játékosnak ütközik fegye fel (törlődjön)
        if(col.tag == "PlayerShipTag" || col.tag == "PlayerUndamagable"){
            Destroy(gameObject);
        }
        
    }
}
