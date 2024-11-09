using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    //Különleges lövedék robbanása
    public GameObject Explosion;

    //Lövedék sebessége
    protected float speed;

    
    public float Speed{
        get{
            return this.speed;
        }
        set{
            this.speed = value;
        }
    }

    //Bomba haladási iránya (Játékosnál egyenesen felfele)
    protected Vector2 direction;

    //Bomba inicializálása
    virtual public void Init(Vector2 _position, Vector2 _direction, float _speed)
    {
        //Irány
        direction = _direction.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        //Sebesség
        speed = _speed;

        //Pozíció
        transform.position = _position;
    }

    //Minden frame során megvan hívv
    virtual protected void Update()
    {
        // a lövedék jelenlegi helyzete
        Vector2 position = transform.position;
        // a lövedék új helyének meghatározása
        position += direction * speed * Time.deltaTime;
        // a lövedék új helyének beállítása
        transform.position = position;

        //ez a játék jobb felső sarka
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1));

        Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0,0));
        //ha a töltény elhagyja a játékteret vagy ha a játékos megnyomja az 'F' gombot, akkor semmisüljön meg
        if(transform.position.y > max.y || transform.position.x > max.x || transform.position.y < min.y || transform.position.x < min.x)
        {
            Destroy(gameObject);
        }


    }


}
