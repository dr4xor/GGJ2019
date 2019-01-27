using System.Collections;
using System.Collections.Generic;
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
	private int score => Mathf.FloorToInt(scoreFloat);
    private LevelManager levelManager;
    private AudioSource audioSource;
    private Level level;
    private HealthController health;
    private int highscore = 0;
    private bool newHighscoreShown = false;

    private float scoreFloat = 0;
	[SerializeField] private int _scorePerSurvivedSecond;

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

	private float targetTimeScale = 0.05f;

    void Update()
    {
		targetTimeScale = 0.05f;

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
				scoreFloat += _scorePerSurvivedSecond * Time.deltaTime;
				
				if (!newHighscoreShown && score > highscore && highscore > 0)
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
					targetTimeScale = 1f;
                }

                break;

            case GameState.GameOver:
                if (score > highscore)
                {
                    PlayerPrefs.GetInt("highscore", score);
                }
                targetTimeScale = 0.005f;

                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadScene("Menu");
                }

                break;
        }

		float lerpedTimeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * 10);
		
        Time.timeScale = lerpedTimeScale;
        Time.fixedDeltaTime = lerpedTimeScale * 0.02f;

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
