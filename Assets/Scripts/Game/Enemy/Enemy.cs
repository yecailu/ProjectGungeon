using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEnemy
{
    Room Room { get; set; }

    GameObject GameObject { get; }

    void Hurt(float damage, Vector2 hitDirection);
}
public class Enemy : MonoBehaviour
{

    protected void OnDeath(Vector2 hitDirection, string dieBodyName, float dieBodyScale)
    {
        //����ʬ��
        FxFactory.PlayEnemyDieBody(transform.Position2D(), hitDirection, dieBodyName, dieBodyScale);
        //����������Ч
        AudioKit.PlaySound("resources://EnemyDie");
        //������
        PowerUpFactory.Default.Coin.Instantiate()
            .Position2D(gameObject.Position2D())
            .Show();

        Destroy(gameObject);
    }

}
