using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameObject scoreUITextGO;
    GameObject killsUITextGO;

    public GameObject ExpolsionGO;

    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
    }

    // Update is called once per frame
    void Update()
    {
        //az ellenség helyének meghatározása
        Vector2 position = transform.position;
        //az új pocizió megállapítása
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //az ellenség új pozicióba helyezése
        transform.position = position;

        //a játéktér aljának meghatározása(bal alsó sarok)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        //ha elhagyja a játékteret törlődjön a hajó
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag"))
        {

            PlayerExplosion();

            scoreUITextGO.GetComponent<GameScore>().Score += 100;
            killsUITextGO.GetComponent<DestroyedEnemy>().Kills += 1;

            Destroy(gameObject);
        }
    }
    void PlayerExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        explosion.transform.position = transform.position;
    }
}
