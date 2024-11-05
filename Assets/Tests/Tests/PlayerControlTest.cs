using NUnit.Framework;             // NUnit tesztelési keretrendszer
using UnityEngine;                // Unity alapvető funkciók
using TMPro;                     // TextMeshProUGUI használata

public class PlayerControlTest : MonoBehaviour
{
    private GameObject playerGO;             // A játékos GameObject
    private PlayerControl playerControl;     // A PlayerControl komponens

    [SetUp]
    public void Setup()
    {
        // Létrehozunk egy GameObject-et a játékos számára és hozzáadjuk a PlayerControl komponenst
        playerGO = new GameObject();
        playerControl = playerGO.AddComponent<PlayerControl>();

        // Beállítunk egy kamerát a viewport számításokhoz
        Camera.main = new GameObject().AddComponent<Camera>();
        Camera.main.transform.position = new Vector3(0, 0, -10); // A kamera pozíciójának beállítása

        // Inicializáljuk a TextMeshProUGUI komponenseket
        playerControl.LivesUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.SpecialsUIText = new GameObject().AddComponent<TextMeshProUGUI>();
        playerControl.scoreUITextGO = new GameObject(); // Placeholder a pontszám UI számára
        playerControl.scoreUITextGO.AddComponent<GameScore>();

        // Hozzárendelünk dummy GameObject-eket más referenciákhoz
        playerControl.GameManagerGO = new GameObject();
        playerControl.PlayerBulletGO = new GameObject();
        playerControl.bulletPosition01 = new GameObject();
        playerControl.bulletPosition02 = new GameObject();
        playerControl.bulletPosition03 = new GameObject();
        playerControl.bletPosition04 = new GameObject();
        playerControl.bulletPosition05 = new GameObject();
        playerControl.bulletPosition06 = new GameObject();
        playerControl.specialPosition = new GameObject();
        playerControl.ExpolsionGO = new GameObject();
        playerControl.SpecialGO = new GameObject();

        // Meghívjuk az Init-et a kezdeti állapotok beállításához
        playerControl.Init();
    }

    [Test]
    public void Init_SetsInitialValues()
    {
        // Ellenőrizzük, hogy a kezdeti életek helyesen vannak beállítva
        Assert.AreEqual(3, playerControl.GetType().GetField("lives", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual(0, playerControl.GetType().GetField("upgradeLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual(0, playerControl.GetType().GetField("specials", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(playerControl));
        Assert.AreEqual("X 0", playerControl.SpecialsUIText.text);
    }

    [Test]
    public void Move_UpdatesPlayerPosition()
    {
        // Beállítjuk a sebességet és szimuláljuk a mozgáshoz szükséges bemenetet
        playerControl.speed = 5f;

        // Simulate moving right
        SimulateInput("Horizontal", 1f);
        playerControl.Update();

        // Ellenőrizzük, hogy a játékos jobbra mozdult
        Assert.Greater(playerGO.transform.position.x, 0);

        // Simulat
