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
            //�������ķ�ʽ�ܹ��������ܿ�����������ָ��ѡ��Ķ���
            GameObject[] games = GameObject.FindGameObjectsWithTag("Player");
            //���õ�ǰ�����Ҫ׷�ٶ���ľ��룬������������ֵû�б�˵����Χ��û�е���
            float distance = this.distance;
            foreach (GameObject game in games)
            {
                //�����ʵ�Ƕ���ģ���ΪĿǰ���Գ����пɱ�׷�ٵĵ��˶��������ǩ������Ҳ���Ա���
                //�����ڴ˻����Ϸ�չ׷�����ȼ����ı���ͬ������������BOSS�����߲��ܱ�׷�ٵĵ��˶���

                //�ҵ�������ָ��Ϊ���˵Ķ��󣬽��о�����ֵ����
                float dis = Vector2.Distance(new(game.transform.position.x, game.transform.position.y),
                    new(gameObject.transform.position.x, gameObject.transform.position.y));
                if (distance > dis)
                {
                    //����С�����GameObject����ֵ����Ҫ׷�ٵĶ���enemy���᲻�ϵ�ѭ�����棬���ս���ѭ����ʱ��
                    //ɸѡ������enemy���Ǿ��������Ļ����ĵ���
                    distance = dis;
                    player = game.GetComponent<Player>();

                }
                if (distance == this.distance)
                {
                    //���û���ҵ����߷��й��������������׷�پ��룬ɾ��׷�ٶ���
                    player = null;
                }
            }

            //�����ٶ�
            transform.Translate(4 * Time.deltaTime, 0, 0);

            //��⵽����ʱ��ִ��׷��
            if (player != null)
            {
                //����Ҫ׷������������ĽǶ��������
                Vector2 row = (player.transform.position - transform.position).normalized;
                //��ȡ�������н�
                float angle1 = Vector3.SignedAngle(Vector3.up, row, Vector3.forward);
                //���н���������������ȡֵ�ͷ�Χ����
                angle1 = (angle1 + 270) % 360;
                //��ȡ��Ļ������������
                float angle2 = transform.eulerAngles.z % 360;
                //��ȡ�����Ƕȼ�Ĳ���ֵ����׼��
                float angle3 = ((angle1 - angle2) + 360) % 360;


                //������ĽǶ���������ʹ������x��ָ����Ҫ׷�ٵ�Ŀ��
                //������Ҫ׷�ٶ���ķ�������Ƕȣ������趨��ֵ���е���

                float angle_differ = 10f;//����Ĳ���Ƕ�
                float angle_fix = 2f;//ÿ�������ĽǶ�

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
            //׷�ٷ���
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