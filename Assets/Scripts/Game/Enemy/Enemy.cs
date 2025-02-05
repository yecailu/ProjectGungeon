using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IEnemy
{
    bool IsBoss{get;}

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

        PowerUpFactory.GeneratePowerUp(this);



        Destroy(gameObject);
    }

    public Room Room { get; set; }

    public GameObject GameObject => gameObject;

    public abstract void Hurt(float damage, Vector2 hitDirection);

    protected abstract Rigidbody2D GetRigidbody2D { get; }

    public virtual bool IsBoss => false;

    Vector2? posToMove = null;

    public List<PathFindingHelper.NodeBase<Vector3Int>> MovementPath = new List<PathFindingHelper.NodeBase<Vector3Int>>();

    protected void TryInitMovementPath()
    {
        if (MovementPath.Count == 0)
        {
            var grid = LevelController.Default.WallTilemap.layoutGrid;
            var myCellPos = grid.WorldToCell(transform.position);
            var playerCellPos = grid.WorldToCell(Player.Default.Position());
            PathFindingHelper.FindPath(Room.PathFindingGrid[myCellPos.x, myCellPos.y],
                Room.PathFindingGrid[playerCellPos.x, playerCellPos.y], MovementPath);
        }
    }

    protected Vector2 Move()
    {
        if (posToMove == null)
        {
            if (MovementPath.Count > 0)
            {
                var pathPos = MovementPath.Last().Coords.Pos;
                posToMove = new Vector2(pathPos.x + 0.5f, pathPos.y + 0.5f);
                MovementPath.RemoveAt(MovementPath.Count - 1);
            }
        }

        var directionToPlayer = Player.Default.NormalizedDirectionTo(transform);
        if (posToMove == null)
        {
            GetRigidbody2D.velocity = directionToPlayer;
        }
        else
        {
            var direction = posToMove.Value - transform.Position2D();
            GetRigidbody2D.velocity = direction.normalized;

            if (direction.magnitude < 0.2f)
            {
                posToMove = null;
            }
        }

        return directionToPlayer;
    }

}
