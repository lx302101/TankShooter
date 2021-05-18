using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankMultiplayor : MonoBehaviour
{
    public GameObject projectile;
    public Button left;
    public Button right;
    public Button settings;
    public bool top;
    public Text score;
    public Text popupScore;

#if !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    private bool isVibrate;
#endif

    Rigidbody2D rb;
    private float tankSpeed;
    private int enemyScore;
    private int bulletRenderTime;
    private int currBulletTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyScore = 0;
        tankSpeed = 2;
        bulletRenderTime = 180;
        currBulletTime = 0;

#if !UNITY_EDITOR
        isVibrate = false;
#endif

        Button btn = settings.GetComponent<Button>();
        btn.onClick.AddListener(settingsOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
#if !UNITY_EDITOR
        isVibrate = settings.gameObject.getComponent<Settings>().isVibrate;
#endif
        moveTank();
        renderBullet();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            addScore();
            score.text = Convert.ToString(enemyScore);
#if !UNITY_EDITOR
            if (vibrate)
                vibrator.Call("vibrate", 500)
#endif
        }
    }

    private void settingsOnClick()
    {
        settings.gameObject.GetComponent<Settings>().updateCanvas();
    }

    private void moveTank()
    {

        if (right.gameObject.GetComponent<MoveButton>().isPressed() && !left.gameObject.GetComponent<MoveButton>().isPressed())
        {
            if (transform.position.x > 2.4)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = transform.right * tankSpeed;
            }
        }
        else if (!right.gameObject.GetComponent<MoveButton>().isPressed() && left.gameObject.GetComponent<MoveButton>().isPressed())
        {
            if (transform.position.x < -2.4)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.velocity = transform.right * -tankSpeed;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    private void renderBullet()
    {
        if (Time.timeScale == 0)
            return;

#if !UNITY_EDITOR

        Touch touch = Input.GetTouch(Input.touchCount-1).rawPosition;
        if (top && touch.position.y < 960)
            return;
        else if (!top && touch.position.y > 960)
            return;

        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(Input.touchCount-1).position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == left.gameObject || hit.collider.gameObject == right.gameObject
                || hit.collider.gameObject == settings.gameObject)
                    return;
        }

#elif UNITY_EDITOR

        float mousey = Input.mousePosition.y;

        if (top && mousey < 960)
            return;
        else if (!top && mousey > 960)
            return;
        
        if (isMouseInGameObject(left.gameObject) || isMouseInGameObject(right.gameObject) || isMouseInGameObject(settings.gameObject))
            return;

#endif

        ++currBulletTime;


        if (currBulletTime > bulletRenderTime)
        {
#if !UNITY_EDITOR
            if (Input.TouchCount > 0)
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
        float updatedy = transform.position.y;
        if (top)
        {
            updatedy = (float)(updatedy - 1);
        }
        else
        {
            updatedy = (float)(updatedy + 1);
        }
        Vector3 bulletPosition = new Vector3(transform.position.x, updatedy, 0);
        GameObject bullet = Instantiate(projectile, bulletPosition, transform.rotation);
        Destroy(bullet, 4f);
    }

    public void addScore()
    {
        ++enemyScore;
        popupScore.gameObject.SetActive(true);
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

    public void resetTanks()
    {
        Vector3 tempPosition = transform.position;
        transform.position = new Vector3(0f, tempPosition.y, 0f);

        if (top)
        {
            transform.eulerAngles = new Vector3(180, 0, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }

}
