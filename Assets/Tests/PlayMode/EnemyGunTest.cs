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

    //Kamera felállítása
    [OneTimeSetUp]
    public void SetupCamera(){

        mainCamera = new GameObject("mainCamera");
        mainCamera.transform.position = new Vector2(0,0);
        mainCamera.AddComponent<Camera>();
        mainCamera.GetComponent<Camera>().orthographic = true;
        mainCamera.GetComponent<Camera>().orthographicSize = 5;
        mainCamera.tag = "MainCamera";

    }

    //Hiányzó játékos tesztelése
    [UnityTest]
    public IEnumerator NullPLayerTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Target = null;
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        //Act
        yield return new WaitForSeconds(1);

        //Assert
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNull(bullet);
    }

    //Felfele lövés tesztelése
    [UnityTest]
    public IEnumerator DirectionUpTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        //Act
        yield return new WaitForSeconds(1);

        //Assert
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(0,1)));
        GameObject.Destroy(bullet.gameObject);
    }

    //Lefele lövés tesztelése
    [UnityTest]
    public IEnumerator DirectionDownTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,0));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));
        
        //Act 
        yield return new WaitForSeconds(1);

        //Assert
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(0,-1)));
        GameObject.Destroy(bullet.gameObject);
    }

    //Jobbra lövés tesztelése
    [UnityTest]
    public IEnumerator DirectionRightTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(1, (float)0.5));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));
        
        //Act
        yield return new WaitForSeconds(1);

        //Assert
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(1,0)));
        GameObject.Destroy(bullet.gameObject);
    }

    //Balra lövés tesztelése
    [UnityTest]
    public IEnumerator DirectionLeftTest()
    {
        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0, (float)0.5));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        //Act
        yield return new WaitForSeconds(1);

        //Assert
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNotNull(bullet);
        Assert.IsTrue(bullet.GetComponent<EnemyBullet>().GetDirection().Equals(new Vector2(-1, 0)));
        GameObject.Destroy(bullet.gameObject);
    }

    //Zig-Zag kétszer lövésének tesztelése
    [UnityTest]
    public IEnumerator isZigZagTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = true;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        //Act 1
        yield return new WaitForSeconds(1);

        //Assert 1
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);

        //Act 2
        yield return new WaitForSeconds(2);

        //Assert 2
        bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);
    }

    //Nem Zig-Zag ellenfél egyszer lövésének tesztelése
    [UnityTest]
    public IEnumerator isNotZigZagTest(){

        //Arrange
        yield return new WaitForFixedUpdate();
        Target = new GameObject("PlayerGO");
        Target.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,1));
        enemyGun = new GameObject("EnemyGunGO");
        enemyGun.AddComponent<EnemyGun>();
        enemyGun.GetComponent<EnemyGun>().isEnemyZigZag = false;
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO = new GameObject("TestBullet");
        enemyGun.GetComponent<EnemyGun>().EnemyBulletGO.AddComponent<EnemyBullet>();
        enemyGun.transform.position = Camera.main.ViewportToWorldPoint(new Vector2((float)0.5,(float)0.5));

        //Act 1
        yield return new WaitForSeconds(1);

        //Assert 1
        GameObject bullet = GameObject.Find("TestBullet(Clone)");
        Assert.NotNull(bullet);
        GameObject.Destroy(bullet);

        //Act 2
        yield return new WaitForSeconds(2);

        //Assert 2
        bullet = GameObject.Find("TestBullet(Clone)");
        Assert.IsNull(bullet);
        GameObject.Destroy(bullet);
    }

    //EnemyGun objektum törlése
    [TearDown]
    public void EnemyGunTestTearDown(){
        GameObject.Destroy(GameObject.Find("TestBullet"));
        GameObject.Destroy(enemyGun);
        GameObject.Destroy(Target);
    }

    //Kamera törlése
    [OneTimeTearDown]
    public void TearDownCamera(){
        GameObject.Destroy(mainCamera.gameObject);
    }
}
