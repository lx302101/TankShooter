using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public Button infinite;
    public Button multiplayer;

    // Start is called before the first frame update
    void Start()
    {
        infinite.onClick.AddListener(infiniteOnClick);
        multiplayer.onClick.AddListener(multiplayerOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void infiniteOnClick()
    {
        SceneManager.LoadScene("Infinite Crates");
    }

    private void multiplayerOnClick()
    {
        SceneManager.LoadScene("Multiplayer");
    }
}
