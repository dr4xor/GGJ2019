using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject flickerUI;

    void Start()
    {
        StartCoroutine(Flickering());
    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Level");
        }
    }

    IEnumerator Flickering()
    {

        do
        {
            flickerUI.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.2f, 1.5f));
            flickerUI.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        } while (true);

    }
}
