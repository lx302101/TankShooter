using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
    public Button exit;
    public Button resume;
    public Button vibrate;
    public bool isVibrate;

#if !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif

    // Start is called before the first frame update
    void Start()
    {
        exit.onClick.AddListener(exitOnClick);
        resume.onClick.AddListener(resumeOnClick);
        vibrate.onClick.AddListener(vibrateSetting);
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
            exit.gameObject.SetActive(true);
            resume.gameObject.SetActive(true);
            vibrate.gameObject.SetActive(true);
        }
    }

    private void exitOnClick()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }
    
    private void resumeOnClick()
    {
        Time.timeScale = 1;
        exit.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        vibrate.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void vibrateSetting()
    {
        if (isVibrate)
            isVibrate = false;
        else
        {
            isVibrate = true;
            callVibrate();
        }
    }

    private void callVibrate()
    {
#if !UNITY_EDITOR
            if (vibrate)
                vibrator.Call("vibrate", 500)
#endif
    }
}
