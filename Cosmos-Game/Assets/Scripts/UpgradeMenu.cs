using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    float healthMultiplier = 1.3f;

    [SerializeField]
    float speedMultiplier = 1.3f;

    [SerializeField]
    private int upgradeCost = 50;

    private PlayerStats stats;

    void OnEnable()
    {
        stats = PlayerStats.Instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth.ToString();
        speedText.text = "Speed: " + stats.movementSpeed.ToString();
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
    public void UpgradeSpeed()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        else
        {
            stats.movementSpeed = Mathf.Round(stats.movementSpeed * speedMultiplier);
            AudioManager.instance.PlaySound("Money");
            GameMaster.Money -= upgradeCost;
        }

        UpdateValues();
    }
}
