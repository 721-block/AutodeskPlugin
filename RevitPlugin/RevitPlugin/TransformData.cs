using AreaRoomsAPI;
using Autodesk.Revit.DB;
using System.Collections.Generic;


namespace RevitPlugin
{
    public class TransformData
    {
        public static IList<AreaRoomsAPI.Info.Wall> TransformAutodeskWallsToApi(Curve balconyWall, Curve entranceWall, List<Curve> walls)
        {
            var result = new List<AreaRoomsAPI.Info.Wall>();
            var balconyStartPoint = new PointD(balconyWall.GetEndPoint(0).X, balconyWall.GetEndPoint(0).Y);
            var balconyEndPoint = new PointD(balconyWall.GetEndPoint(1).X, balconyWall.GetEndPoint(1).Y);

            var entranceStartPoint = new PointD(entranceWall.GetEndPoint(0).X, entranceWall.GetEndPoint(0).Y);
            var entranceEndPoint = new PointD(entranceWall.GetEndPoint(1).X, entranceWall.GetEndPoint(1).Y);

            foreach (var wall in walls)
            {
                var startPoint = new PointD(wall.GetEndPoint(0).X, wall.GetEndPoint(0).Y);
                var endPoint = new PointD(wall.GetEndPoint(1).X, wall.GetEndPoint(1).Y);
                var wallType = AreaRoomsAPI.Info.WallType.Standart;

                if (startPoint == balconyStartPoint && endPoint == balconyEndPoint)
                {
                    wallType = AreaRoomsAPI.Info.WallType.Balcony;
                }
                else if (startPoint == entranceStartPoint && endPoint == entranceEndPoint)
                {
                    wallType = AreaRoomsAPI.Info.WallType.Enter;
                }

                result.Add(new AreaRoomsAPI.Info.Wall(startPoint, endPoint, wallType));
            }

            return result;
        }

        public static List<Curve> ParseCurveIterator(CurveLoopIterator curves)
        {
            var result = new List<Curve>();

            var isHaveNextElement = curves.MoveNext();
            while (isHaveNextElement)
            {
                result.Add(curves.Current);
                isHaveNextElement = curves.MoveNext();
            }

            return result;
        }
    }
}