using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class EnemyBullet : Bullet
    {
        public Rigidbody2D Rigidbody2D;

        void Start()
        {

        }

        private void FixedUpdate()
        {
            Rigidbody2D.velocity = Velocity;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Player>())
            {
                collision.gameObject.GetComponent<Player>().Hurt((int)Damage);//调用玩家受伤方法
                Destroy(gameObject);//销毁子弹
            }
            else
            {
                Destroy(gameObject);//销毁子弹
            }

        }
    }

    public class Bullet: MonoBehaviour
    {
        public Vector2 Velocity;
        public float Damage = 1;
    }
}