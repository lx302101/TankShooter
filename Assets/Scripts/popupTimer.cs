using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupTimer : MonoBehaviour
{
    private int time;
    private int currTime;
    public bool reset;
    public GameObject TankTop;
    public GameObject TankBottom;


    // Start is called before the first frame update
    void Start()
    {
        time = 300;
        currTime = 0;
        reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            ++currTime;
            Time.timeScale = 0;
        }
        if (currTime >= time)
        {
            this.gameObject.SetActive(false);
            reset = true;
            currTime = 0;
            deleteAllBullets();
            TankTop.GetComponent<TankMultiplayor>().resetTanks();
            TankBottom.GetComponent<TankMultiplayor>().resetTanks();
            Time.timeScale = 1;
        }
    }

    public void deleteAllBullets()
    {
        GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bul in Bullets)
        {
            Destroy(bul);
        }
    }
}
