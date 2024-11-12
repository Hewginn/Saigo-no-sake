using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackObject : MonoBehaviour
{
    //Objektum sebessége
    public float speed;

    public void Init(Vector2 _position, Sprite _sprite, float _rotateAngle){
        transform.position = _position;
        GetComponent<SpriteRenderer>().sprite = _sprite;
        transform.Rotate(0,0,_rotateAngle);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move(){
        //Az objektum helyének meghatározása
        Vector2 position = transform.position;

        //Az új pocizió megállapítása
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //Az objektum új pozicióba helyezése
        transform.position = position;

        //A játéktér aljának meghatározása(bal alsó sarok)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //Ha elhagyja a játékteret törlődjön az objektum
        if (transform.position.y + gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2 < min.y)
        {
            Destroy(gameObject);
        }
    }
}
