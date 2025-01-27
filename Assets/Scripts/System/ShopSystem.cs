using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QFramework.ProjectGungeon
{
    public class ShopSystem
    {
        public static List<Tuple<IPowerUp, int>> CalculateNormalShopItems()
        {
            var normalShopItem = new List<Tuple<IPowerUp, int>>()
            {
                new(PowerUpFactory.Default.Armor1, Random.Range(3, 6 + 1)),
                new(PowerUpFactory.Default.HP1, Random.Range(3, 6 + 1)),
                new(PowerUpFactory.Default.SingleGunFullBullet, Random.Range(15, 20 + 1)),
                new(PowerUpFactory.Default.AllGunHalfBullet, Random.Range(15, 25 + 1)),
                new(PowerUpFactory.Default.Key, Random.Range(5, 10 + 1)),
            };
            return normalShopItem;

        }
    }
}
