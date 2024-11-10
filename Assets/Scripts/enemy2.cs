using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using math = System.Math;

public class enemy2 : MonoBehaviour
{
    GameObject scoreUITextGO;
    GameObject killsUITextGO;

    public GameObject ExpolsionGO;

    float speed;

    bool isDestroyedByPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 0)
        {
            speed = 2f;
        }
        else
        {
            speed = -2f;
        }

        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");

        killsUITextGO = GameObject.FindGameObjectWithTag("DestroyedEnemies");
    }

    // Update is called once per frame
    void Update()
    {
        //az ellenség helyének meghatározása
        Vector2 position = transform.position;
        //az új pocizió megállapítása
        position = new Vector2(position.x - speed * Time.deltaTime * 2, position.y - math.Abs(speed) * Time.deltaTime / 2);

        //az ellenség új pozicióba helyezése
        transform.position = position;

        //a játéktér aljának meghatározása(bal alsó sarok)
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        //ha elhagyja a játékteret törlődjön a hajó
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
        if ((transform.position.x < min.x) || (transform.position.x > max.x))
        {
            speed *= -1;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "PlayerShipTag") || (col.tag == "PlayerBulletTag"))
        {

            PlayerExplosion();

            isDestroyedByPlayer = true;

            Destroy(gameObject);
        }
    }
    void PlayerExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExpolsionGO);

        explosion.transform.position = transform.position;
    }
        private void OnDestroy() {

        //Elpusztított repülők számolása
        if(isDestroyedByPlayer){
            scoreUITextGO.GetComponent<GameScore>().Score += 100;
            killsUITextGO.GetComponent<DestroyedEnemy>().Kills += 1;
        }
    }
}
