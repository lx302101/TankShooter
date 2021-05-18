using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{
    public Button exit;
    public Button retry;
    public Text gameOver;

    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(exitOnClick);
        retry.onClick.AddListener(retryOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        loadCanvas();
    }

    private void loadCanvas()
    {
        if (this.gameObject.activeSelf)
        {
            Time.timeScale = 0;
            gameOver.gameObject.SetActive(true);
            exit.gameObject.SetActive(true);
            retry.gameObject.SetActive(true);
        }
    }

    private void exitOnClick()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    private void retryOnClick()
    {
        SceneManager.LoadScene("Infinite Crates");
        Time.timeScale = 1;
    }
}
