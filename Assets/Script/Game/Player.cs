using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerBullet PlayerBullet;//�ӵ�

    public Rigidbody2D Rigidbody2D;

    public SpriteRenderer Sprite;

    public Transform Weapon;

    public Gun CurrentGun;

    void Start()
    {

    }

    public void Hurt(int damage)
    {
        Global.HP -= damage;
        if (Global.HP <= 0)
        {
            Global.HP = 0;
            GameUI.Default.GameOver.SetActive(true);
            Time.timeScale = 0;

        }
        Global.HPChangedEvent();//����HP�ı䷽��   
    }

    void Update()
    {

        //��ȡ�����λ��
        var mouseScreenPosition = Input.mousePosition;
        //����Ļ����תΪ��������
        var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        //�õ����ǳ������ķ���
        var shootDirection = (mouseWorldPoint - transform.position).normalized;

        //����
        var radius = Mathf.Atan2(shootDirection.y, shootDirection.x);
        //ŷ����
        var eulerAngles = radius * Mathf.Rad2Deg;
        //���ø�Weapon
        Weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);


        //������ת
        if (shootDirection.x > 0)
        {
            Weapon.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Weapon.transform.localScale = new Vector3(1, -1, 1);
        }



        //�ƶ��߼�
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        Rigidbody2D.velocity = new Vector2(horizontal, vertical).normalized * 5;

        if (horizontal < 0)
        {
            Sprite.flipX = true;
        }
        else if (horizontal > 0)
        {
            Sprite.flipX = false;
        }

        if (Input.GetMouseButtonDown(0))//����������ӵ�
        {
            CurrentGun.ShootDown(shootDirection);//�����ӵ����䷽��

        }

        if (Input.GetMouseButton(0))//����������ӵ�
        {
            CurrentGun.Shooting(shootDirection);//�����ӵ����䷽��

        }

        if (Input.GetMouseButtonUp(0))//����������ӵ�
        {
            CurrentGun.ShootUp(shootDirection);//�����ӵ����䷽��

        }
    }
}
