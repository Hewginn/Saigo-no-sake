using NUnit.Framework;             // NUnit testing framework
using UnityEngine;                // Unity core functionalities
using UnityEngine.TestTools;      // Unity testing tools
using System.Collections;         // Required for collections

public class EnemySpreadGunTest : MonoBehaviour
{
    private GameObject enemyGO;          // The enemy GameObject
    private EnemySpreadGunTest enemySpreadGun; // The EnemySpreadGun component

    [SetUp]
    public void SetUp()
    {
        // Create a GameObject for the enemy and add the EnemySpreadGun component
        enemyGO = new GameObject();
        enemySpreadGun = enemyGO.AddComponent<EnemySpreadGunTest>();
    }

    [UnityTest]
    public IEnumerator EnemySpreadGun_FiresBulletsTowardsPlayerWithSpread()
    {
        // Create a player GameObject
        GameObject playerGO = new GameObject("PlayerGO");
        playerGO.transform.position = Vector2.zero; // Set player position

        enemyGO.transform.position = new Vector2(0, 5); // Set enemy position
        yield return new WaitForSeconds(1.1f); // Wait for the shots

        EnemyBullet[] bullets = Object.FindObjectsOfType<EnemyBullet>(); // Find all bullets
        Assert.AreEqual(3, bullets.Length); // Check that there are three bullets

        // Check that the bullets are flying in different directions
        Vector2 direction1 = (Vector2)(bullets[0].transform.position - enemyGO.transform.position).normalized;
        Vector2 direction2 = (Vector2)(bullets[1].transform.position - enemyGO.transform.position).normalized;
        Vector2 direction3 = (Vector2)(bullets[2].transform.position - enemyGO.transform.position).normalized;

        Assert.AreNotEqual(direction1, direction2); // Check that directions are different
        Assert.AreNotEqual(direction1, direction3);
        Assert.AreNotEqual(direction2, direction3);
    }

    [TearDown]
    public void TearDown()
    {
        // Cleanup after tests
        if (enemyGO != null)
        {
            Object.Destroy(enemyGO);
        }
    }
}
