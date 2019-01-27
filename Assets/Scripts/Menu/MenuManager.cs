using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
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
