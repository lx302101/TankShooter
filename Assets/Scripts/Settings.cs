using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public bool isOpen;
    public bool isVibrate;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        isVibrate = canvas.GetComponent<SettingsCanvas>().isVibrate;
    }

    public void updateCanvas()
    {
        if (!canvas.activeSelf)
        {
            canvas.SetActive(true);
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }

}
