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
                collision.gameObject.GetComponent<Player>().Hurt((int)Damage);//����������˷���
                Destroy(gameObject);//�����ӵ�
            }
            else
            {
                Destroy(gameObject);//�����ӵ�
            }

        }
    }

    public class Bullet: MonoBehaviour
    {
        public Vector2 Velocity;
        public float Damage = 1;
    }
}