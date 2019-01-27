using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI scoreText;
    private int score;

    void Start()
    {
        
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
    }
}
