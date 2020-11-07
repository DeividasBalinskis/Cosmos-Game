using UnityEngine.UI;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text waveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawner.SpawnState previousState;

    void Start()
    {
        if(spawner == null)
        {
            Debug.LogError("no spawner referenced");
            this.enabled = false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("no waveAnimator referenced");
            this.enabled = false;
        }
        if (waveCountdownText == null)
        {
            Debug.LogError("no waveCountdownText referenced");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("no waveCountText referenced");
            this.enabled = false;
        }

    }

    void Update()
    {
        switch(spawner.State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }

        previousState = spawner.State;
    }

    void UpdateCountingUI()
    {
        if(previousState != WaveSpawner.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountdown", true);
        }

        waveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }
    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveCountdown", false);
            waveAnimator.SetBool("WaveIncoming", true);

            waveCountText.text = spawner.NextWave.ToString();
        }
    }
}
