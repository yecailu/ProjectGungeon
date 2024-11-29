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

    //敌人状态
    public enum States
    {
        FollowPlayer,
        Shoot,
    }

    public States state = States.FollowPlayer;

    public float FollowPlayerSeconds = 3.0f;//随机跟随时间

    public float CurrenSeconds = 0; //计时器

    private void Start()
    {
        //设置帧数60帧每秒
        Application.targetFrameRate = 60;
    }


    void Update()
    {
        //敌人射击、跟随玩家
        if (state == States.FollowPlayer)
        {
            
            if (CurrenSeconds >= FollowPlayerSeconds)
            {
                state = States.Shoot;
                CurrenSeconds = 0;
            }

            if (Global.Player)
            {
                var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;//敌人到玩家的方向

                Rigidbody2D.velocity = directionToPlayer; //敌人速度

                //敌人朝向主角
                if (directionToPlayer.x > 0)
                {
                    Sprite.flipX = false;
                }
                else
                { 
                    Sprite.flipX = true;
                }
            }

                CurrenSeconds += Time.deltaTime;//计时

        }
        else if (state == States.Shoot)
        {
            CurrenSeconds += Time.deltaTime;//计时

            if (CurrenSeconds >= 1.0f)//时间大于1后开始射击，并且赋予敌人随机跟随时间
            {
                state = States.FollowPlayer;
                FollowPlayerSeconds = Random.Range(2, 4f);
                CurrenSeconds = 0;
            }

            if (Time.frameCount % 20 == 0)//每20帧射击一次
            {
                if (Global.Player)
                {
                    //敌人到玩家的方向
                    var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;
                    //敌人子弹逻辑
                    var enemyBullet = Instantiate(EnemyBullet);
                    enemyBullet.transform.position = transform.position;
                    enemyBullet.Direction = directionToPlayer;
                    enemyBullet.gameObject.SetActive(true);

                    //播放射击音效
                    var soundIndex = Random.Range(0, ShootSounds.Count);
                    ShootSoundPlayer.clip = ShootSounds[soundIndex];
                    ShootSoundPlayer.Play();
                }

            }

        }
    }
}
