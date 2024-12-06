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

    [SetUp]
    public void PlayerConntrolTestSetUp(){

        SceneManager.LoadScene("TestScene");

    }


    [UnityTest]
    public IEnumerator InitTest()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        Assert.IsTrue(GameObject.Find("PlayerGO").gameObject.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator ShootTest()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        Assert.IsTrue(GameObject.Find("PlayerBulletGO(Clone)") != null);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    [UnityTest]
    public IEnumerator ShootSpecialTest()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject SpecialSupply = (GameObject)Object.Instantiate(Resources.Load("ForTests/SpecialPUGO"));
        SpecialSupply.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        yield return new WaitForSeconds(1);
        Assert.IsTrue(player.GetComponent<PlayerControl>().SpecialsUIText.text == "1 X");
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'E'";
        yield return new WaitUntil(() => Input.GetKey("e"));
        Assert.IsTrue(GameObject.Find("PlayerBombGO(Clone)") != null);
        Assert.IsTrue(player.GetComponent<PlayerControl>().SpecialsUIText.text == "0 X");
        GameObject.Destroy(GameObject.Find("PlayerBombGO(Clone)"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    [UnityTest]
    public IEnumerator Upgrade1Test()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject UpgradeSupply = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        yield return new WaitForSeconds(1);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(4,numberOfBullets);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    [UnityTest]
    public IEnumerator Upgrade2Test()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject UpgradeSupply1 = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        GameObject UpgradeSupply2 = (GameObject)Object.Instantiate(Resources.Load("ForTests/UpgradePUGO"));
        UpgradeSupply2.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        yield return new WaitForSeconds(1);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'SPACE'";
        yield return new WaitUntil(() => Input.GetKey("space"));
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(6,numberOfBullets);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";
    }

    [UnityTest]
    public IEnumerator Upgrade0Test(){
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
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

        int numberOfBullets = 0;
        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("PlayerBulletTag")){
            numberOfBullets++;
        }
        Assert.AreEqual(2,numberOfBullets);
    }

    [UnityTest]
    public IEnumerator MoveTest(){
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();

        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "Input Test: Press 'W', 'A', 'S' or 'D' or Arrowes";
        yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        GameObject.Find("TestMessage").GetComponent<TextMeshProUGUI>().text = "";

        Assert.IsFalse(player.transform.position.Equals(Camera.main.ViewportToWorldPoint(new Vector2(0,0))));
    }

    [UnityTest]
    public IEnumerator InvincibelModeTest(){
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject Damage1 = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        Damage1.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        GameObject Damage2 = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage2.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);
        Damage2.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));

        yield return new WaitForSeconds(1);

        Assert.IsTrue(player.GetComponent<PlayerControl>().LivesUIText.text == "2");
    }

    [UnityTest]
    public IEnumerator HealTest(){
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        player = GameObject.Find("PlayerGO");
        player.SetActive(false);
        player.GetComponent<PlayerControl>().Init();
        GameObject Damage = (GameObject)Object.Instantiate(Resources.Load("ForTests/EnemyBulletGO"));
        Damage.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1f);
        Damage.GetComponent<EnemyBullet>().SetDirection(new Vector2(0,-1));
        GameObject Heal = (GameObject)Object.Instantiate(Resources.Load("ForTests/HealPUGO"));
        Heal.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 2f);

        yield return new WaitForSeconds(1);

        Assert.IsTrue(player.GetComponent<PlayerControl>().LivesUIText.text == "3");
    }
    
}
