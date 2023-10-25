﻿using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;


namespace RevitPlugin
{
    public class TransformData
    {
        public static IList<AreaRoomsAPI.Info.Wall> TransformAutodeskWallsToApi(Curve balconyWall, Curve entranceWall, CurveLoopIterator walls)
        {
            var result = new List<AreaRoomsAPI.Info.Wall>();
            var isHaveNextElement = true;

            while (isHaveNextElement)
            {
                var wall = walls.Current;
                var startPoint = new PointD(wall.GetEndPoint(0).X, wall.GetEndPoint(0).Y);
                var endPoint = new PointD(wall.GetEndPoint(1).X, wall.GetEndPoint(1).Y);
                var wallType = AreaRoomsAPI.Info.WallType.Standart;

                if (wall.Id == balconyWall.Id)
                {
                    wallType = AreaRoomsAPI.Info.WallType.Balcony;
                }
                else if (wall.Id == entranceWall.Id)
                {
                    wallType = AreaRoomsAPI.Info.WallType.Enter;
                }

                result.Add(new AreaRoomsAPI.Info.Wall(startPoint, endPoint, wallType));

                isHaveNextElement = walls.MoveNext();
            }

            return result;
        }
    }
}
