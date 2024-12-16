using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerControlTest
{

    private GameObject player;

    //Teszt környezet felállítása
    [SetUp]
    public void PlayerConntrolTestSetUp(){

        SceneManager.LoadScene("TestScene");

    }

    //Inicializálás tesztelése
    [UnityTest]
    public IEnumerator InitTest()
    {
        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();

        //Assert
        Assert.IsTrue(GameObject.Find("PlayerGO").gameObject.activeInHierarchy);
    }

    //Lövés tesztelése
    [UnityTest]
    public IEnumerator ShootTest()
    {
        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));

        //Assert
        Assert.IsTrue(GameObject.Find("PlayerBulletGO(Clone)") != null);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    //Különleges lövés tesztelése
    [UnityTest]
    public IEnumerator ShootSpecialTest()
    {
        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject SpecialSupply = (GameObject)Object.Instantiate(Resources.Load("ForTests/SpecialPUGO"));
        SpecialSupply.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.GetComponent<PlayerControl>().SpecialsUIText.text == "1 X");
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'E'";
        yield return new WaitUntil(() => Input.GetKey("e"));

        //Assert
        Assert.IsTrue(GameObject.Find("PlayerBombGO(Clone)") != null);
        Assert.IsTrue(player.GetComponent<PlayerControl>().SpecialsUIText.text == "0 X");
        GameObject.Destroy(GameObject.Find("PlayerBombGO(Clone)"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    //Egyes fejlesztés tesztelése
    [UnityTest]
    public IEnumerator Upgrade1Test()
    {
        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject UpgradeSupply = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        yield return new WaitForSeconds(1);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";

        //Assert
        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(4,numberOfBullets);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    //Kettes fejéesztés tesztelése
    [UnityTest]
    public IEnumerator Upgrade2Test()
    {
        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject UpgradeSupply1 = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        GameObject UpgradeSupply2 = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply2.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        yield return new WaitForSeconds(1);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";

        //Assert
        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(6,numberOfBullets);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    //Fejlesztés elvesztésének tesztelése
    [UnityTest]
    public IEnumerator Upgrade0Test(){

        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject UpgradeSupply1 = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        GameObject Damage = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        Damage.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        yield return new WaitForSeconds(1);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";

        //Assert
        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(2,numberOfBullets);
    }

    //Mozgás tesztelése
    [UnityTest]
    public IEnumerator MoveTest(){

        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'W', 'A', 'S' or 'D' or Arrowes";
        yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";

        //Assert
        Assert.IsFalse(player.transform.position.Equals(Camera.main.ViewportToWorldPoint(new Vector2(0,0))));
    }

    //Halhatatlanság tesztelése
    [UnityTest]
    public IEnumerator InvincibelModeTest(){

        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject Damage1 = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        Damage1.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        GameObject Damage2 = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage2.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        Damage2.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        yield return new WaitForSeconds(1);

        //Assert
        Assert.IsTrue(player.GetComponent<PlayerControl>().LivesUIText.text == "2 X");
    }

    //Gyógyulás tesztelése
    [UnityTest]
    public IEnumerator HealTest(){

        //Arrange
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);

        //Act
        player.GetComponent<PlayerControl>().Init();
        GameObject Damage = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        Damage.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        GameObject Heal = (GameObject)Object.Instantiate(Resources.Load("ForTests/HealPUGO"));
        Heal.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        yield return new WaitForSeconds(1);

        Debug.Log(player.GetComponent<PlayerControl>().LivesUIText.text);

        //Arrange
        Assert.IsTrue(player.GetComponent<PlayerControl>().LivesUIText.text == "3 X");
    }
    
}
