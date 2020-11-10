using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;
        }

    }

    public PlayerStats stats = new PlayerStats();

    public int fallBoundry = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();

        if(statusIndicator == null)
        {
            Debug.LogError("no status indicator referrenced on player");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgrdeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("no audio manage r in scene");
        }
    }

    void Update()
    {
        if(transform.position.y <= fallBoundry)
        {
            DamagePlayer(99999999);
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void OnUpgradeMenuToggle(bool active)
    {
        //handle what happens, when upgrade menu is toggled
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgrdeMenu -= OnUpgradeMenuToggle;
    }

    public void DamagePlayer (int damage)
    {
        stats.curHealth -= damage;
        if(stats.curHealth <=0)
        {
            audioManager.PlaySound(deathSoundName);

            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }
    }
}
