using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;
    ZombieAI zombieAI;
    bool isEnemyDead;

    public Collider[] enemyCol;

    private void Start()
    {
        zombieAI = GetComponent<ZombieAI>();
    }

    public void DeductEnemyHealth(float deductHealth)
    {
        if (!isEnemyDead)
        {
            enemyHealth -= deductHealth;
            if (enemyHealth <= 0) { EnemyDeath(); }
        } 
    }
    void EnemyDeath()
    {
        isEnemyDead = true;
        zombieAI.DeathAnim();
        zombieAI.agent.speed = 0f;
        foreach (var col in enemyCol)
        {
            col.enabled = false;
        }
        enemyHealth = 0f;
        Destroy(gameObject, 8);
    }


}
