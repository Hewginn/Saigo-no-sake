using System.Collections;
using System.Data;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EnemyGunTest
{
    GameObject mainCamera;
    GameObject enemyGun;
    GameObject Target;


    [OneTimeSetUp]
    public void SetupCamera(){

        mainCamera = new GameObject("mainCamera");
        mainCamera.transform.position = new Vector2(0,0);
        mainCamera.AddComponent<Camera>();
        mainCamera.GetComponent<Camera>().orthographic = true;
        mainCamera.GetComponent<Camera>().orthographicSize = 5;
        mainCamera.tag = "MainCamera";

    }

    [UnityTest]
    public IEnumerator NullPLayerTest()
    {
        yield return new WaitForFixedUpdate();

        Target = null;

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");

        Assert.IsNull(bullet);
    }

    [UnityTest]
    public IEnumerator DirectionUpTest()
    {
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");

        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(0,1)));
        GameObject.Destroy(bullet.gameObject);
    }

    [UnityTest]
    public IEnumerator DirectionDownTest()
    {
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,0));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));
        
        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");

        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(0,-1)));
        GameObject.Destroy(bullet.gameObject);
    }

    [UnityTest]
    public IEnumerator DirectionRightTest()
    {
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(1, (float)0.5));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));
        
        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");

        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(1,0)));
        GameObject.Destroy(bullet.gameObject);
    }

    [UnityTest]
    public IEnumerator DirectionLeftTest()
    {
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0, (float)0.5));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");

        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(-1, 0)));
        GameObject.Destroy(bullet.gameObject);
    }

    [UnityTest]
    public IEnumerator isZigZagTest(){
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = true;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);
        yield return new WaitForSeconds(2);
        bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);
    }

        [UnityTest]
        public IEnumerator isNotZigZagTest(){
        yield return new WaitForFixedUpdate();

        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));

        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        yield return new WaitForSeconds(1);
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);
        yield return new WaitForSeconds(2);
        bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNull(bullet);
        GameObject.Destroy(bullet);
    }


    [TearDown]
    public void EnemyGunTestTearDown(){
        GameObject.Destroy(GameObject.Find("TestBullet"));
        GameObject.Destroy(enemyGun);
        GameObject.Destroy(Target);
    }

    [OneTimeTearDown]
    public void TearDownCamera(){
        GameObject.Destroy(mainCamera.gameObject);
    }
}
