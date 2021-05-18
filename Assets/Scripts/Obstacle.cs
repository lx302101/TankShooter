using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D rb;
    public float obstacleSpeed;
    public Tank Tank1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7)
        {
            rb.velocity = Vector2.zero;
        } else
        {
            rb.velocity = -transform.up * obstacleSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Tank1.addScore();
            Destroy(collision.gameObject);
        }
    }
}
