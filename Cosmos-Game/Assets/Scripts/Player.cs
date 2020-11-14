using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
//[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    public int fallBoundry = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.Instance;

        stats.curHealth = stats.maxHealth;

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

        InvokeRepeating("RegenHealth", 1 / stats.healthRegenRate, 1 / stats.healthRegenRate);
    }

    void RegenHealth()
    {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
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
        //GetComponent<SpriteRenderer>().enabled = !active;
        ArmRotation _armR = GetComponentInChildren<ArmRotation>();
        if(_armR !=null)
        {
            _armR.enabled = !active;
        }
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
        {
            _weapon.enabled = !active;
        }

        if(active)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
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
