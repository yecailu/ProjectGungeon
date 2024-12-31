using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 Velocity;

    private Rigidbody2D mSelfRigidbody2D;

    public float Damage { get; set; } = 1;

    private void Awake()
    {
        mSelfRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }



    void FixedUpdate()
    {
        mSelfRigidbody2D.velocity = Velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.hurt(Damage);//敌人受伤方法
            Destroy(gameObject);//销毁子弹
        }
        else
        { 
            Destroy(gameObject);
        }
    }
}
