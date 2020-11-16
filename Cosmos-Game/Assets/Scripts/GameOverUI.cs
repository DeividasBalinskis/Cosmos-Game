using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    [SerializeField]
    string mouseHoverSound = "ButtonHover";

    [SerializeField]
    string buttonPressSound = "ButtonPress";

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgrdeMenu;

    [SerializeField]
    private GameObject upgradeMenu;

    AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("no audio manager found in the scene");
        }
    }

    public void  Shop()
    {
        audioManager.PlaySound(buttonPressSound);

        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        //waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgrdeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void Quit()
    {
        Debug.Log("Quited the Game");
        Application.Quit();
    }

    public void Retry()
    {
        audioManager.PlaySound(buttonPressSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMouseOver()
    {
        audioManager.PlaySound(mouseHoverSound);
    }
}
