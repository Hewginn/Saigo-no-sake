using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Különleges lövedék
public class PlayerSpecial : Bomb
{

    public override void Init(Vector2 _position, Vector2 _direction, float _speed)
    {
        base.Init(_position, _direction, _speed);
    }

    //Minden frame során megvan hívva
    protected override void Update()
    {
        base.Update();

        //ha a játékos megnyomja az 'F' gombot, akkor semmisüljön meg
        if(Input.GetKeyDown("f"))
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

    public void SetDirection(Vector2 dir){
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction * -1);
    }
    
    //Törlödés esetén robbanjon fel
    void OnDestroy() {
        GameObject explosion = (GameObject) Instantiate(Explosion);
        explosion.transform.position = transform.position;
        explosion.tag = "PlayerSpecialTag";
    }
}
