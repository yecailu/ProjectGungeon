using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    public EnemyBullet EnemyBullet;

    public SpriteRenderer Sprite;

    public List<AudioClip> ShootSounds = new List<AudioClip>();

    public AudioSource ShootSoundPlayer;

    public Rigidbody2D Rigidbody2D;

    public float HP { get; set; } = 5;


    public void hurt(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    //����״̬
    public enum States
    {
        FollowPlayer,
        Shoot,
    }

    public States state = States.FollowPlayer;

    public float FollowPlayerSeconds = 3.0f;//�������ʱ��

    public float CurrenSeconds = 0; //��ʱ��

    private void Start()
    {
        //����֡��60֡ÿ��
        Application.targetFrameRate = 60;
    }


    void Update()
    {
        //����������������
        if (state == States.FollowPlayer)
        {
            
            if (CurrenSeconds >= FollowPlayerSeconds)
            {
                state = States.Shoot;
                CurrenSeconds = 0;
            }

            if (Global.Player)
            {
                var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;//���˵���ҵķ���

                Rigidbody2D.velocity = directionToPlayer; //�����ٶ�

                //���˳�������
                if (directionToPlayer.x > 0)
                {
                    Sprite.flipX = false;
                }
                else
                { 
                    Sprite.flipX = true;
                }
            }

                CurrenSeconds += Time.deltaTime;//��ʱ

        }
        else if (state == States.Shoot)
        {
            CurrenSeconds += Time.deltaTime;//��ʱ

            if (CurrenSeconds >= 1.0f)//ʱ�����1��ʼ��������Ҹ�������������ʱ��
            {
                state = States.FollowPlayer;
                FollowPlayerSeconds = Random.Range(2, 4f);
                CurrenSeconds = 0;
            }

            if (Time.frameCount % 20 == 0)//ÿ20֡���һ��
            {
                if (Global.Player)
                {
                    //���˵���ҵķ���
                    var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;
                    //�����ӵ��߼�
                    var enemyBullet = Instantiate(EnemyBullet);
                    enemyBullet.transform.position = transform.position;
                    enemyBullet.Direction = directionToPlayer;
                    enemyBullet.gameObject.SetActive(true);

                    //���������Ч
                    var soundIndex = Random.Range(0, ShootSounds.Count);
                    ShootSoundPlayer.clip = ShootSounds[soundIndex];
                    ShootSoundPlayer.Play();
                }

            }

        }
    }
}
