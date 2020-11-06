using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _curentHealth;
        public int curentHealth
        {
            get { return _curentHealth; }
            set { _curentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    [Header("Optional")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();

        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curentHealth, stats.maxHealth);
        }
    }

    public void DamageEnemy(int damage)
    {
        stats.curentHealth -= damage;
        if (stats.curentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curentHealth, stats.maxHealth);
        }
    }
}
