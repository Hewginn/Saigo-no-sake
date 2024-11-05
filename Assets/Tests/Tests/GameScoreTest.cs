using NUnit.Framework;
using UnityEngine;
using TMPro;

public class GameScoreTest : MonoBehaviour
{
    private GameObject gameScoreGO; // A GameScore GameObject
    private GameScoreTest gameScore; // A GameScore komponens
    private TextMeshProUGUI scoreTextUI; // A pontszám megjelenítésére szolgáló UI komponens

    [SetUp]
    public void Setup()
    {
        // A GameScore objektum létrehozása és a szükséges komponens hozzáadása
        gameScoreGO = new GameObject();
        gameScore = gameScoreGO.AddComponent<GameScoreTest>();

        // TextMeshProUGUI komponens hozzáadása a pontszám megjelenítéséhez
        scoreTextUI = gameScoreGO.AddComponent<TextMeshProUGUI>();
    }

    [Test]
    public void GameScore_SetScore_UpdatesScoreTextUI()
    {
        // A pontszám beállítása
        gameScore.Score = 12345;

        // Ellenőrizzük, hogy a megjelenített szöveg megfelel a formázott pontszámnak
        Assert.AreEqual("012345", scoreTextUI.text);
    }

    [Test]
    public void GameScore_SetScore_FormatsScoreWithLeadingZeros()
    {
        // Alacsonyabb pontszám beállítása és ellenőrizzük, hogy a vezető nullák helyesen lettek-e hozzáadva
        gameScore.Score = 42;

        // Ellenőrizzük, hogy a formázott string vezető nullákat tartalmaz
        Assert.AreEqual("000042", scoreTextUI.text);
    }

    [Test]
    public void GameScore_InitialScore_IsZero()
    {
        // Ellenőrizzük a kezdeti pontszámot (implicit módon 0) és ellenőrizzük a megjelenített formát
        Assert.AreEqual(0, gameScore.Score);
        Assert.AreEqual("000000", scoreTextUI.text);
    }

    [TearDown]
    public void TearDown()
    {
        // Tisztítsuk meg a létrehozott GameObject-et
        Object.Destroy(gameScoreGO);
    }
}
