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
public abstract class Enemy : MonoBehaviour,IEnemy
{
   

    protected void OnDeath(Vector2 hitDirection, string dieBodyName, float dieBodyScale)
    {
        //生成尸体
        FxFactory.PlayEnemyDieBody(transform.Position2D(), hitDirection, dieBodyName, dieBodyScale);
        //敌人死亡音效
        AudioKit.PlaySound("resources://EnemyDie");
        //掉落金币
        PowerUpFactory.Default.Coin.Instantiate()
            .Position2D(gameObject.Position2D())
            .Show();


        Destroy(gameObject);
    }

    public Room Room { get; set; }

    public GameObject GameObject => gameObject;

    public abstract void Hurt(float damage, Vector2 hitDirection);

}
