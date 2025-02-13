using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

namespace QFramework.ProjectGungeon
{
    public class Boss_Fist : Bullet
    {
        public Player player;

        public float distance = 2000;

        public static Boss_Fist Default;

        private void Awake()
        {
            Default = this;
        }
        private void OnDestroy()
        {
            Default = null;
        }

        private void OnArea()
        {
            //相对上面的方式能够减少性能开销，而且能指定选择的对象
            GameObject[] games = GameObject.FindGameObjectsWithTag("Player");
            //设置当前最近需要追踪对象的距离，如果结束后这个值没有变说明范围内没有敌人
            float distance = this.distance;
            foreach (GameObject game in games)
            {
                //这句其实是多余的，因为目前测试场景中可被追踪的敌人都是这个标签，但是也可以保留
                //可以在此基础上发展追踪优先级，的比如同距离优先锁定BOSS，或者不能被追踪的敌人对象

                //找到场景中指定为敌人的对象，进行距离求值运算
                float dis = Vector2.Distance(new(game.transform.position.x, game.transform.position.y),
                    new(gameObject.transform.position.x, gameObject.transform.position.y));
                if (distance > dis)
                {
                    //将最小距离的GameObject对象赋值给需要追踪的对象enemy，会不断的循环更替，最终结束循环的时候
                    //筛选出来的enemy就是距离这个弹幕最近的敌人
                    distance = dis;
                    player = game.GetComponent<Player>();

                }
                if (distance == this.distance)
                {
                    //如果没有找到或者飞行过程中脱离了最大追踪距离，删除追踪对象
                    player = null;
                }
            }

            //赋予速度
            transform.Translate(4 * Time.deltaTime, 0, 0);

            //检测到敌人时则执行追踪
            if (player != null)
            {
                //对需要追踪物体和自身间的角度求解运算
                Vector2 row = (player.transform.position - transform.position).normalized;
                //获取两物体间夹角
                float angle1 = Vector3.SignedAngle(Vector3.up, row, Vector3.forward);
                //将夹角坐标和世界坐标的取值和范围对齐
                angle1 = (angle1 + 270) % 360;
                //获取弹幕自身世界坐标
                float angle2 = transform.eulerAngles.z % 360;
                //获取两个角度间的差异值并标准化
                float angle3 = ((angle1 - angle2) + 360) % 360;


                //对物体的角度做修正，使得物体x轴指向需要追踪的目标
                //朝向需要追踪对象的方向调整角度，按照设定的值进行调整

                float angle_differ = 10f;//允许的差异角度
                float angle_fix = 2f;//每次修正的角度

                if (angle3 < 180 - angle_differ)
                {
                    Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z - angle_fix);
                    transform.rotation = reAngle;
                }
                else if (angle3 > 180 + angle_differ)
                {
                    Quaternion reAngle = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle_fix);
                    transform.rotation = reAngle;
                }

            }
        }



        private void Update()
        {
            //追踪方法
            OnArea();

        }

        public List<AudioClip> HitWallSfxs = new List<AudioClip>();
        public List<AudioClip> HitPlayerSfx = new List<AudioClip>();


        public void PlayerSound()
        {
            var hitWallSfx = HitWallSfxs.GetRandomItem();
            var audioPlayer = AudioKit.PlaySound(hitWallSfx, callBack: (_) =>
            {
                this.DestroyGameObjGracefully();
            });
            audioPlayer?.SetVolume(0.5f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            BulletFactory.Default.Explosion
                .Instantiate()
                .Position2D(transform.Position2D())
                .Show();


            if (collision.gameObject.CompareTag("Player"))
            {
                this.Hide();

                player.Hurt(2);

                if (HitPlayerSfx.Count > 0)
                {
                    var hitPlayerSfx = HitPlayerSfx.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitPlayerSfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.5f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                this.Hide();
                if (HitWallSfxs.Count > 0)
                {
                    var hitWallSfx = HitWallSfxs.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitWallSfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.5f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }

            }

        }
    }
}