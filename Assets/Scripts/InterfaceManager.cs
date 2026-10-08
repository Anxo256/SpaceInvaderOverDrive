using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LivesText;
    public WeaponSlotUI[] WeaponSlots;
    public GameObject GameOverScreen;
    public TextMeshProUGUI GameOverText;

    private int CurrentScore = 0;
    private int CurrentLives = 3;
    private bool GameEnded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        UpdateInterface();

        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(false);
        }
    }

    void Update()
    {
        if (GameEnded && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void AddPoints(int Points)
    {
        if (GameEnded) return;

        CurrentScore += Points;
        UpdateInterface();
    }

    public void LoseLife()
    {
        if (GameEnded) return;

        CurrentLives--;
        UpdateInterface();

        if (CurrentLives <= 0)
        {
            GameOver();
        }
    }

    void UpdateInterface()
    {
        ScoreText.text = "SCORE: " + CurrentScore.ToString("D4"); 
        LivesText.text = "LIVES: " + Mathf.Max(CurrentLives, 0).ToString();
    }

    public void UpdateWeaponHud(int CurrentWeapon, float[] RemainingTimes, float[] Cooldowns)
    {
        if (WeaponSlots == null) return;

        for (int i = 0; i < WeaponSlots.Length; i++)
        {
            if (WeaponSlots[i] != null)
            {
                WeaponSlots[i].SetState(i == CurrentWeapon, RemainingTimes[i], Cooldowns[i]);
            }
        }
    }

    public void GameOver()
    {
        EndGame("GAME OVER");
    }

    public void Win()
    {
        EndGame("YOU WIN!");
    }

    void EndGame(string Message)
    {
        if (GameEnded) return;
        GameEnded = true;

        if (GameOverText != null)
        {
            GameOverText.text = Message + "\nSCORE: " + CurrentScore.ToString("D4");
        }

        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(true);
        }

        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
