using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ellenséges lövedék
public class EnemyBullet : MonoBehaviour
{
    //Lövdék sebessége
    float speed;

    public float Speed{
        get{
            return this.speed;
        }
        set{
            this.speed = value;
        }
    }

    //Lövedék iránya
    Vector2 _direction;

    //Logikai változó: be van-e állítva az irány
    bool isReady;

    //Inicializáláskor kezdeti értékek megadása(sebesség, van-e irány)
    void Awake()
    {
        speed = 5f;
        isReady = false;
    }

    //Irány beállítása (Ellenséges fegyver osztályok használják)
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        isReady = true;
    }

    //Minden frame során megvan hívva
    void Update()
    {
        //Ha az irány be van állítva, akkor mozogjon az irányba adott sebességgel
        if (isReady)
        {
            //Mozgás az irányban
            Vector2 position = transform.position;
            position += _direction * speed * Time.deltaTime;
            transform.position = position;

            //Ha a lövedék túl lépné a képernyő határait, akkor törölni kell
            //Határok
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            //Törlés
            if (transform.position.x < min.x || transform.position.x > max.x ||
                transform.position.y < min.y || transform.position.y > max.y)
            {
                Destroy(gameObject);
            }
        }
    }

    //Játékosnak ütközés esetén a lövedék eltünik (sebzés a PlayerControl.cs-ben)
    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "PlayerShipTag"){
            Destroy(gameObject);
        }
    }
}