using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 Direction;

    public float ShootSpeed = 15;


    void Start()
    {

    }



    void Update()
    {
        transform.Translate(Direction * Time.deltaTime * ShootSpeed);//×Óµ¯ËÙ¶È
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
