using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerBullet PlayerBullet;//子弹

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
        Global.HPChangedEvent();//调用HP改变方法   
    }

    void Update()
    {

        //获取鼠标点击位置
        var mouseScreenPosition = Input.mousePosition;
        //将屏幕坐标转为世界坐标
        var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        //得到主角朝向鼠标的方向
        var shootDirection = (mouseWorldPoint - transform.position).normalized;

        //弧度
        var radius = Mathf.Atan2(shootDirection.y, shootDirection.x);
        //欧拉角
        var eulerAngles = radius * Mathf.Rad2Deg;
        //设置给Weapon
        Weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);


        //武器翻转
        if (shootDirection.x > 0)
        {
            Weapon.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Weapon.transform.localScale = new Vector3(1, -1, 1);
        }



        //移动逻辑
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

        if (Input.GetMouseButtonDown(0))//按左键发射子弹
        {
            CurrentGun.ShootDown(shootDirection);//调用子弹发射方法

        }

        if (Input.GetMouseButton(0))//按左键发射子弹
        {
            CurrentGun.Shooting(shootDirection);//调用子弹发射方法

        }

        if (Input.GetMouseButtonUp(0))//按左键发射子弹
        {
            CurrentGun.ShootUp(shootDirection);//调用子弹发射方法

        }
    }
}
