using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LivesText;
    public TextMeshProUGUI WeaponText;
    public GameObject GameOverScreen;
    public TextMeshProUGUI GameOverText;

    private int CurrentScore = 0;
    private int CurrentLives = 3;
    private bool GameEnded = false;

    private static readonly string[] WeaponNames = { "BASE", "SHOTGUN", "MISSILE" };

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

    public void UpdateWeaponHud(int CurrentWeapon, bool Ready)
    {
        if (WeaponText == null) return;

        string Line = "";

        for (int i = 0; i < WeaponNames.Length; i++)
        {
            string Label = "[" + (i + 1) + "] " + WeaponNames[i];

            if (i == CurrentWeapon)
            {
                string Color = Ready ? "#FFFFFF" : "#FF9933";
                Line += "<color=" + Color + "><b>" + Label + "</b></color>";
            }
            else
            {
                Line += "<color=#777777>" + Label + "</color>";
            }

            if (i < WeaponNames.Length - 1) Line += "      ";
        }

        Line += "\n" + (Ready ? "<color=#66FF66>READY</color>" : "<color=#FF9933>RELOADING...</color>");
        WeaponText.text = Line;
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
