using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Főellenség bombája
public class BossBomb : Bomb
{

    Vector2 startPosition;

    public override void Init(Vector2 _position, Vector2 _direction, float _speed)
    {
        base.Init(_position, _direction, _speed);
        startPosition = _position;
    }

    // Minden frémnél frissítsére kerül
    protected override void Update()
    {
        base.Update();

        if(Vector2.Distance(startPosition, transform.position) > 5f){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerShipTag" || other.tag == "PlayerBulletTag"){
            Destroy(gameObject);
        }
    }

    //Törlödés esetén robbanjon fel
    void OnDestroy() {
        GameObject explosion = (GameObject) Instantiate(Explosion);
        explosion.transform.position = transform.position;
        explosion.tag = "EnemyBulletTag";
    }
}
