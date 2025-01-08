using UnityEditor;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public interface IPowerUp
    {
        public Room Room { get; set; }

        public SpriteRenderer SprirerRenderer { get; }


    }
}
