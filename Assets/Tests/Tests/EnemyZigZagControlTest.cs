using NUnit.Framework;
using UnityEngine;
using System.Collections;
using UnityEngine.TestTools;

public class EnemyZigZagControlTest : MonoBehaviour
{
    private GameObject enemyGO;
    private EnemyZigZagControlTest enemyZigZagControl; // Itt a helyes típust használd
    private GameObject scoreUITextGO;
    private GameObject killsUITextGO;

    [SetUp]
    public void SetUp()
    {
        enemyGO = new GameObject();
        enemyZigZagControl = enemyGO.AddComponent<EnemyZigZagControlTest>();

        scoreUITextGO = new GameObject("ScoreUITextGO");
        scoreUITextGO.tag = "ScoreTextTag";
        scoreUITextGO.AddComponent<GameScoreTest>();

        killsUITextGO = new GameObject("KillsUITextGO");
        killsUITextGO.tag = "DestroyedEnemies";
        killsUITextGO.AddComponent<DestroyedEnemyTest>();
    }

    [UnityTest]
    public IEnumerator EnemyZigZagControl_MovesInZigZagPattern()
    {
        Vector2 initialPosition = enemyGO.transform.position;
        yield return new WaitForSeconds(0.5f);
        Vector2 newPosition = enemyGO.transform.position;

        Assert.AreNotEqual(initialPosition.x, newPosition.x);
        Assert.AreNotEqual(initialPosition.y, newPosition.y);
    }

    [UnityTest]
    public IEnumerator EnemyZigZagControl_DestroysOnLeavingScreen()
    {
        enemyGO.transform.position = new Vector2(0, Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y - 1);
        yield return null;

        Assert.IsTrue(enemyGO == null);
    }

    [UnityTest]
    public IEnumerator EnemyZigZagControl_ScoreAndKillsIncreaseOnPlayerCollision()
    {
        GameObject playerGO = new GameObject("PlayerGO");
        playerGO.tag = "PlayerShipTag";
        
        enemyGO.AddComponent<BoxCollider2D>();
        playerGO.AddComponent<BoxCollider2D>();
        
        enemyGO.transform.position = playerGO.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(100, scoreUITextGO.GetComponent<GameScoreTest>().Score);
        Assert.AreEqual(1, killsUITextGO.GetComponent<DestroyedEnemyTest>().Kills);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(enemyGO);
        Object.Destroy(scoreUITextGO);
        Object.Destroy(killsUITextGO);
    }
}
