using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Level");
        }
    }
}
