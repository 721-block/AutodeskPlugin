﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class AreaInfo : Shape
    {
        public readonly IList<Wall> Walls;

        public readonly double Margin;

        public readonly IList<RoomType> RoomTypes;

        public AreaInfo(IList<Wall> walls, double margin, IList<RoomType> roomTypes) : base(walls)
        {
            Walls = walls;
            Margin = margin;
            RoomTypes = roomTypes;
        }
    }
}
