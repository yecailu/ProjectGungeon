﻿using System.Collections.Generic;
using UnityEngine.Rendering;

namespace QFramework.ProjectGungeon
{
    public class RoomConfig
    {
        public RoomTypes RoomType;

        public List<string> Codes = new List<string>();

        public RoomConfig Type(RoomTypes type)
        {
            RoomType = type;
            return this;
        }

        public RoomConfig L(string code)
        {
            Codes.Add(code);
            return this;
        }
    }
}
