using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text damageText;

    [SerializeField]
    float healthMultiplier = 1.3f;

    [SerializeField]
    float damageMultiplier = 1.3f;

    [SerializeField]
    private int upgradeCost = 50;

    private PlayerStats stats;
    private Weapon weapon;

    void OnEnable()
    {
        stats = PlayerStats.Instance;
        weapon = Weapon.Instance;


        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth.ToString();
        damageText.text = "Damage: " + weapon.Damage.ToString();
    }

    public void UpgradeHealth()
    {
        if(GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        else
        {
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        }


        UpdateValues();
    }

    public void UpgradeDamage()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        else
        {
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            weapon.Damage = (int)(weapon.Damage * damageMultiplier); 
        }


        UpdateValues();
    }
}
