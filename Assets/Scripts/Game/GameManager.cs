using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        InGame, Paused, GameOver
    }

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI levelNameText;
    public GameObject tutorial;
    public GameObject newHighscore;
    private GameState gameState = GameState.InGame;
    private int levelIdx = 1;
    private int score = 0;
    private LevelManager levelManager;
    private Level level;
    private int highscore = 0;
    private bool newHighscoreShown = false;

    void Start()
    {
        levelManager = GetComponent<LevelManager>();

        highscore = PlayerPrefs.GetInt("highscore", highscore);
        levelIdx = PlayerPrefs.GetInt("level", levelIdx);

        level = levelManager.LoadLevel(levelIdx);
        levelNameText.text = level.name;

        StartCoroutine(ShowLevelIntro());
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

        for (int i=0; i<5; i++)
        {
            newHighscore.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            newHighscore.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

    }

    void Update()
    {
        if (!newHighscoreShown && score > highscore)
        {
            newHighscoreShown = true;
            StartCoroutine(ShowNewHighscore());
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
}
