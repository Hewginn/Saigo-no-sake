using NUnit.Framework;
using UnityEngine;
using TMPro;

public class PlayerControlTest : MonoBehaviour
{
    private GameObject playerGO;             // A játékos GameObject
    private PlayerControlTest playerControl;     // A PlayerControl komponens

       //Játék kezelő
    public GameObject GameManagerGO;

    //ez a játékos előgyártott lövedéke
    public GameObject PlayerBulletGO;

    //Játékos fegyvereinek pozíciója
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject bulletPosition03;
    public GameObject bulletPosition04;
    public GameObject bulletPosition05;
    public GameObject bulletPosition06;

    //Fegyverek fejlesztési szintje
    int upgradeLevel;

    //Speciális lövés kezdeti pozíciója
    public GameObject specialPosition;

    //Robbanás
    public GameObject ExpolsionGO;

    //Speciális lövedék
    public GameObject SpecialGO;

    //Élet szám UI szöveg
    public TextMeshProUGUI LivesUIText;

    //Különleges lövedékek száma UI szöveg
    public TextMeshProUGUI SpecialsUIText;

    //Pontszámláló UI szöveg
    public GameObject scoreUITextGO;

    //Maximum élet
    const int maxLives = 3;

    //Aktuális élet
    int lives;

    //Repülő gyorsasága
    public float speed;

    //Aktuális különleges lövedék
    int specials;

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a játékos számára és hozzáadjuk a PlayerControl komponenst
        playerGO = new GameObject();
        playerControl = playerGO.AddComponent<PlayerControlTest>();

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása

        // Inicializáljuk a TextMeshProUGUI komponenseket
        playerControl.LivesUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.SpecialsUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.scoreUITextGO = new GameObject(); // Placeholder a pontszám UI számára
        playerControl.scoreUITextGO.AddComponent<GameScore>();

        // Dummy GameObject-ek létrehozása más hivatkozásokhoz
        playerControl.GameManagerGO = new GameObject();
        playerControl.PlayerBulletGO = new GameObject();
        playerControl.bulletPosition01 = new GameObject();
        playerControl.bulletPosition02 = new GameObject();
        playerControl.bulletPosition03 = new GameObject();
        playerControl.bulletPosition04 = new GameObject();
        playerControl.bulletPosition05 = new GameObject();
        playerControl.bulletPosition06 = new GameObject();
        playerControl.specialPosition = new GameObject();
        playerControl.ExpolsionGO = new GameObject();
        playerControl.SpecialGO = new GameObject();

        // Meghívjuk az Init metódust a kezdeti értékek beállításához
        playerControl.Init();
    }

    [Test]
    public void Init_SetsInitialValues()
    {
        // Ellenőrizzük, hogy a kezdeti életek helyesen vannak beállítva
        Assert.AreEqual("3", playerControl.LivesUIText.text);
        Assert.AreEqual(0, playerControl.GetType().GetField("upgradeLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual(0, playerControl.GetType().GetField("specials", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual("X 0", playerControl.SpecialsUIText.text);
    }

    [Test]
    public void Move_UpdatesPlayerPosition()
    {
        // Beállítjuk a sebességet és szimuláljuk a mozgást
        playerControl.speed = 5f;

        // Jobbra mozgás szimulálása
        playerGO.transform.position = new Vector3(0, 0, 0); // Kezdő pozíció
        playerControl.Move(new Vector2(1, 0)); // Jobbra mozgás

        // Ellenőrizzük, hogy a játékos jobbra mozdult
        Assert.Greater(playerGO.transform.position.x, 0);

        // Balra mozgás szimulálása
        playerControl.Move(new Vector2(-1, 0)); // Balra mozgás

        // Ellenőrizzük, hogy a játékos balra mozdult
        Assert.Less(playerGO.transform.position.x, 0);
    }

    [Test]
    public void Shoot_FiresBullets()
    {
        playerControl.lastShoot = 0f; // Legutóbbi lövés időpontjának nullázása
 }
}
