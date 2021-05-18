using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    public GameObject projectile;
    public GameObject obstacle;
    public Camera cam;
    public Text textScore;
    public Button settings;
    public GameObject gameOverCanvas;
    public Texture texture;

#if !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    private bool isVibrate;
#endif

    Rigidbody2D rb;
    // Start is called before the first frame update
    private float tankSpeed;
    private int score;
    private int bulletRenderTime;
    private int crateRenderTime;
    private int currBulletTime;
    private int currCrateTime;
    private AudioSource crateBreak;

    // Crate count and speed
    private float obstacleStartSpeed;

    private void Awake()
    {
        crateBreak = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        Time.timeScale = 1;
    }

    void Start()
    {
        obstacleStartSpeed = 1;
        score = 0;
        bulletRenderTime = 180;
        crateRenderTime = 600;
        currBulletTime = 0;
        currCrateTime = 0;
        tankSpeed = 2;

#if !UNITY_EDITOR
        isVibrate = false;
#endif

        Button btn = settings.GetComponent<Button>();
        btn.onClick.AddListener(settingsOnClick);
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        isVibrate = settings.gameObject.getComponent<Settings>().isVibrate;
#endif
        if (Time.timeScale == 0) return;

        textScore.text = Convert.ToString(score);

        obstacleStartSpeed = (float)((1 + 0.05 * score));
        tankSpeed = (float)(2 + 0.05*score);

        renderBullet();
        renderCrate();
        moveTank();
        
    }

    private void FixedUpdate()
    {
        // settings page and sound manipulation stuff
    }

    private void moveTank()
    {


    // if on mobile, screen tilt
 
#if !UNITY_EDITOR
        if (Input.acceleration.x > 0)
#elif UNITY_EDITOR
        if (Input.GetKey(KeyCode.D))
#endif
        {  
            if (transform.position.x > 2.4)
            {
                rb.velocity = Vector2.zero;
            } else
            {
                rb.velocity = transform.right * tankSpeed;
            }
        }
#if !UNITY_EDITOR
        else if (Input.acceleration.x < 0)
#elif UNITY_EDITOR
        else if (Input.GetKey(KeyCode.A))
#endif
        {
            if (transform.position.x < -2.4)
            {
                rb.velocity = Vector2.zero;
            } else
            {
                rb.velocity = transform.right * -tankSpeed;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log("Hit");
            gameOverCanvas.SetActive(true);
#if !UNITY_EDITOR
            if (isVibrate)
                vibrator.Call("vibrate", 500)
#endif
        }
    }

    private void renderCrate()
    {
        if (Time.timeScale == 0)
            return;

        ++currCrateTime;

        if (currCrateTime > crateRenderTime)
        {
            loadCrate();
            currCrateTime = 0;
            crateRenderTime -= 10;
            if (crateRenderTime < 120)
            {
                crateRenderTime = 120;
            }
        }
    }

    private void renderBullet()
    {
        if (Time.timeScale == 0)
            return;

#if !UNITY_EDITOR

        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(Input.touchCount-1).position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if ( hit.collider.gameObject == settings.gameObject)
                return;
        }

#elif UNITY_EDITOR

        if (isMouseInGameObject(settings.gameObject))
            return;

#endif

        ++currBulletTime;

        if (currBulletTime > bulletRenderTime)
        {
#if !UNITY_EDITOR
            if (Input.touchCount > 0)
#elif UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
#endif
            {
                loadBullet();
                currBulletTime = 0;
                bulletRenderTime -= 5;
                if (bulletRenderTime < 60)
                {
                    bulletRenderTime = 60;
                }
            }
        }
    }
    private void loadBullet()
    {
        Debug.Log("Clicked");
        float updatedy = (float)(transform.position.y + 0.5);
        Vector3 bulletPosition = new Vector3(transform.position.x, updatedy, 0);
        GameObject bullet = Instantiate(projectile, bulletPosition, transform.rotation);
        Destroy(bullet, 4f);
    }

    private void loadCrate()
    {
        Vector3 cratePosition = new Vector3(UnityEngine.Random.Range(-2.12f, 2.12f), 5.6f, 0);
        GameObject crate = Instantiate(obstacle, cratePosition, transform.rotation);
        Obstacle test = crate.gameObject.GetComponent<Obstacle>();
        test.Tank1 = this;
        test.obstacleSpeed = obstacleStartSpeed;
    }

    public void addScore()
    {
        crateBreak.Play();
        score += 1;
    }

    private void settingsOnClick()
    {
        settings.gameObject.GetComponent<Settings>().updateCanvas();
    }

    private bool isMouseInGameObject(GameObject obj)
    {
        Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousepos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(obj.GetComponent<RectTransform>(), mouse, Camera.main, out mousepos);
        if (obj.GetComponent<RectTransform>().rect.Contains(mousepos))
        {
            return true;
        }
        return false;
    }

}
