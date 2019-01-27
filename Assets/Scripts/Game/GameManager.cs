using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        Ready, InGame, Paused, GameOver
    }

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI levelNameText;
    public GameObject tutorial;
    public GameObject newHighscore;
    public AudioClip loadingAudio;

    private GameState gameState = GameState.Ready;
    private int levelIdx = 1;
    private int score = 0;
    private LevelManager levelManager;
    private AudioSource audioSource;
    private Level level;
    private int highscore = 0;
    private bool newHighscoreShown = false;

    void Start()
    {
        levelManager = GetComponent<LevelManager>();
        audioSource = GetComponent<AudioSource>();

        highscore = PlayerPrefs.GetInt("highscore", highscore);
        levelIdx = PlayerPrefs.GetInt("level", levelIdx);

        level = levelManager.LoadLevel(levelIdx);
        levelNameText.text = level.name;

        StartCoroutine(ShowLevelIntro());
    }

    void Update()
    {
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

                break;
        }

        float motionSpeed = 1;
        if (!Input.GetMouseButton(0))
        {
            motionSpeed = 0.05f;
        }
        Time.timeScale = motionSpeed;
        Time.fixedDeltaTime = motionSpeed * 0.02f;

        scoreText.text = "Score: " + score;
    }

    IEnumerator ShowLevelIntro()
    {
        levelNameText.gameObject.SetActive(true);
        tutorial.SetActive(true);

        yield return new WaitForSeconds(5);

        levelNameText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        tutorial.SetActive(false);
    }

    IEnumerator ShowNewHighscore()
    {

        for (int i = 0; i < 5; i++)
        {
            newHighscore.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            newHighscore.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
