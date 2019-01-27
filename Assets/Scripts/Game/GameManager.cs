using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Ready, InGame, Paused, GameOver
    }

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI levelNameText;
    public GameObject tutorialUI;
    public GameObject newHighscoreUI;
    public GameObject gameOverUI;
    public GameObject player;
    public AudioClip loadingAudio;

    private GameState gameState = GameState.Ready;
    private int levelIdx = 1;
    private int score = 0;
    private LevelManager levelManager;
    private AudioSource audioSource;
    private Level level;
    private HealthController health;
    private int highscore = 0;
    private bool newHighscoreShown = false;

    void Start()
    {
        levelManager = GetComponent<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        health = player.GetComponent<HealthController>();

        highscore = PlayerPrefs.GetInt("highscore", highscore);
        levelIdx = PlayerPrefs.GetInt("level", levelIdx);

        level = levelManager.LoadLevel(levelIdx);
        levelNameText.text = level.name;

        StartCoroutine(ShowLevelIntro());
    }

    void Update()
    {
        float motionSpeed = 0.05f;
        switch (gameState)
        {
            case GameState.Ready:
                if (Input.GetMouseButton(0))
                {
                    gameState = GameState.InGame;
                    audioSource.clip = levelManager.getMusic();
                    audioSource.Play();
                }

                break;

            case GameState.InGame:
                if (!newHighscoreShown && score > highscore)
                {
                    newHighscoreShown = true;
                    StartCoroutine(ShowNewHighscore());
                }
                if (health.CurrentHealth <= 0)
                {
                    gameState = GameState.GameOver;
                    levelNameText.gameObject.SetActive(false);
                    tutorialUI.SetActive(false);
                    gameOverUI.SetActive(true);
                }

                if (Input.GetMouseButton(0))
                {
                    motionSpeed = 1f;
                }

                break;

            case GameState.GameOver:
                motionSpeed = 0.005f;

                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene("Menu");
                }

                break;
        }

        Time.timeScale = motionSpeed;
        Time.fixedDeltaTime = motionSpeed * 0.02f;

        scoreText.text = "Score: " + score;
    }

    IEnumerator ShowLevelIntro()
    {
        levelNameText.gameObject.SetActive(true);
        tutorialUI.SetActive(true);

        yield return new WaitForSeconds(5);

        levelNameText.gameObject.SetActive(false);
        tutorialUI.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    IEnumerator ShowNewHighscore()
    {

        for (int i = 0; i < 5; i++)
        {
            newHighscoreUI.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            newHighscoreUI.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
