using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDifficultManager : MonoBehaviour
{
    [Header("Difficult Components")]
    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [Header("UI")]
    [SerializeField] private Image timerImage;
    [SerializeField] private Text timerText;
    [SerializeField] private Text collectedText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button restartEnd;
    [SerializeField] private Button mainMenuEnd;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject endGamePanel;
    //difficult variables    
    private int difficult;
    private int maxTreasures;
    private int currentTreasures;
    private int maxEnemies;
    private float maxTime;
    private float time;
    private bool IsLose = false;
    private int score;

    private void Awake()
    {
        InitDifficult();
    }

    private void Start()
    {
        time = maxTime;
        player.OnLose += GameOver;
        player.OnCollected += IncreaseCollected;

        pauseButton.onClick.AddListener(() => OnPause());
        resumeButton.onClick.AddListener(() => OnResume());
        restartButton.onClick.AddListener(() => OnRestart());
        mainMenuButton.onClick.AddListener(() => OnMainMenu());
        mainMenuEnd.onClick.AddListener(() => OnMainMenu());
        restartEnd.onClick.AddListener(() => OnRestart());
    }

    private void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        timerImage.fillAmount = Mathf.InverseLerp(0, maxTime, time);
        timerText.text = $"{(int)time}";
        if(time <= 0 && !IsLose)
        {
            IsLose = true;
            GameOver();
        }
    }

    private void InitDifficult()
    {
        difficult = PlayerPrefs.GetInt("difficult");
        switch (difficult)
        {
            case 1:
                maxTreasures = 3;
                maxEnemies = 3;
                maxTime = 480f;
                break;
            case 2:
                maxTreasures = 3;
                maxEnemies = 4;
                maxTime = 420f;
                break;
            case 3:
                maxTreasures = 4;
                maxEnemies = 5;
                maxTime = 400f;
                break;
        }
        SetDifficult();
    }

    public void IncreaseCollected()
    {
        currentTreasures++;
        collectedText.text = collectedText.text = $"Collected: {currentTreasures}/{maxTreasures}";
        if(currentTreasures >= maxTreasures)
        {
            GameOver();
        }
    }
    private int CalculateScore(int Score)
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (PlayerPrefs.GetInt("BestScore") > Score)
            {
                Score = PlayerPrefs.GetInt("BestScore");
                return Score;
            }
            else
            {
                PlayerPrefs.SetInt("BestScore", Score);
                return Score;
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", Score);
            return Score;
        }
    }

    private void SetDifficult()
    {
        currentTreasures = 0;
        spawner.enemyCount = maxEnemies;
        spawner.treasureCount = maxTreasures;
        collectedText.text = $"Collected: {currentTreasures}/{maxTreasures}";
    }

    public void GameOver()
    {
        endGamePanel.SetActive(true);
        Time.timeScale = 0;
        score = (int)(currentTreasures * 10) + (int)(Mathf.InverseLerp(0, maxTime, time) * 10);
        CalculateScore(score);

        scoreText.text = $"Score: {score}\n\nBest Score: {CalculateScore(score)}";
    }

    private void OnRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    private void OnPause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
    private void OnMainMenu()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(0);
    }
    private void OnResume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
