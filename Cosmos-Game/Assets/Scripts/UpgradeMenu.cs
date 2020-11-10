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
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        UpdateValues();
    }
    public void UpgradeSpeed()
    {
        stats.movementSpeed = Mathf.Round(stats.movementSpeed * speedMultiplier);
        UpdateValues();
    }
}
