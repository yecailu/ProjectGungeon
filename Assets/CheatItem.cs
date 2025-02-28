using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class CheatItem : MonoBehaviour
    {
        public void HPUnlimited()
        {
            Global.HP.Value += 9999;
        }

        public void ArmorAdd()
        {
            Global.Armor.Value += 1;
        }

        public void CoinAdd()
        {
            Global.Coin.Value += 100;
        }

        public void KeyAdd()
        {
            Global.Key.Value += 100;
        }

        public void DiamondAdd()
        {
            Global.Color.Value += 100;
        }

        public void AimSwitch()
        {
            Aim.isAimingEnabled = !Aim.isAimingEnabled;
            AimHelper.AutoAim = !AimHelper.AutoAim;
        }



    }
}