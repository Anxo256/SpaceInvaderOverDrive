using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LivesText;
    public GameObject GameOverScreen;

    private int CurrentScore = 0;
    private int CurrentLives = 3;

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
        UpdateInterface();
        GameOverScreen.SetActive(false);
    }

    public void AddPoints(int Points)
    {
        CurrentScore += Points;
        UpdateInterface();
    }

    public void LoseLife()
    {
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
        LivesText.text = "LIVES: " + CurrentLives.ToString();
    }

    void GameOver()
    {
        GameOverScreen.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
