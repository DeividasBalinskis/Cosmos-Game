using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
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

        public int damage = 40;

        public void Init()
        {
            curentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmt = 0.1f;
    public float shakeLenght = 0.1f;

    public string deathSoundName = "Explosion";

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

        GameMaster.gm.onToggleUpgrdeMenu += OnUpgradeMenuToggle;

        if(deathParticles == null)
        {
            Debug.LogError("no death particles referenced on enemy");
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        //handle what happens, when upgrade menu is toggled
        /*if (this != null)
            return;*/
        GetComponent<EnemyAI>().enabled = !active;
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

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(99999);
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgrdeMenu -= OnUpgradeMenuToggle;
    }
}
