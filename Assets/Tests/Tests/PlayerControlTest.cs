using NUnit.Framework;
using UnityEngine;
using TMPro;

public class PlayerControlTest : MonoBehaviour
{
    private GameObject playerGO; // A játékos GameObject
    private PlayerControlTest playerControl; // A PlayerControl komponens

    // Élet szám UI szöveg
    public TextMeshProUGUI LivesUIText;

    // Különleges lövedékek száma UI szöveg
    public TextMeshProUGUI SpecialsUIText;

    // Fegyverek fejlesztési szintje
    int upgradeLevel;

    // Maximum élet
    const int maxLives = 3;

    // Aktuális élet
    int lives;

    // Repülő gyorsasága
    public float speed;

    // Aktuális különleges lövedék
    int specials;

    float lastShoot;

    // Repülő inicializálása
    public void Init()
    {
        // Élet
        lives = maxLives;
        LivesUIText.text = lives.ToString();

        // Helyzet
        transform.position = new Vector2(0, 0);

        // Aktiválás
        gameObject.SetActive(true);

        // Fejlesztési szint
        upgradeLevel = 0;

        // Különleges lövedékek száma
        specials = 0;
        SpecialsUIText.text = "X " + specials.ToString();
    }

    [SetUp]
public void Setup()
{
    // Létrehozunk egy GameObject-et a játékos számára és hozzáadjuk a PlayerControl komponenst
    playerGO = new GameObject();
    playerControl = playerGO.AddComponent<PlayerControlTest>();

    // Beállítunk egy kamerát a viewport számításokhoz
    GameObject cameraObject = new GameObject("Main Camera");
    Camera camera = cameraObject.AddComponent<Camera>();
    cameraObject.tag = "MainCamera"; // Beállítjuk a kamerát MainCamera tag-re

    // Inicializáljuk a TextMeshProUGUI komponenseket
    playerControl.LivesUIText = new GameObject().AddComponent<TextMeshProUGUI>();
    playerControl.SpecialsUIText = new GameObject().AddComponent<TextMeshProUGUI>();

    // Meghívjuk az Init metódust a kezdeti értékek beállításához
    playerControl.Init();
}


    void Move(Vector2 direction)
    {
        // Képernyő határainak meghatározása
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        max.x = max.x - 0.225f;
        min.x = min.x + 0.225f;
        max.y = max.y - 0.285f;
        min.y = min.y + 0.285f;

        // Aktuális pozíció
        Vector2 pos = transform.position;

        // Új pozíció
        pos += direction * speed * Time.deltaTime;

        // Határok alkalmazása
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // Új pozíció átadása az objektumnak
        transform.position = pos;
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
}
