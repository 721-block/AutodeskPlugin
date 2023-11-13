﻿using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class RoomsGenerator
    {
        private readonly IList<PointD> areaPoints;

        private readonly IList<RoomType> rooms;

        private readonly AreaRoomsFormatsInfo formats;
        private readonly AreaInfo areaInfo;

        public RoomsGenerator(AreaInfo areaInfo, IList<RoomType> roomTypes, AreaRoomsFormatsInfo formats)
        {
            areaPoints = areaInfo.Points;
            rooms = roomTypes;
            this.formats = formats;
            this.areaInfo = areaInfo;
        }

        public GeneratedArea GenerateArea()
        {
            var list = new List<(RoomType, IList<PointD>)>();
            var startPoint = areaPoints.OrderBy(x => x.X).ThenBy(y => y.Y).First();
            var root = new Leaf(0, 0, areaInfo.Width, areaInfo.Height);
            Leaf.SplitArea(root, new Random());
            var leafs = Leaf.GetLeafsList(root);
            foreach (Leaf leaf in leafs)
            {
                list.Add((RoomType.Toilet, leaf.GetPoints(startPoint)));
            }
            return new GeneratedArea(list);
        }
    }
}
