using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevitPlugin
{
    public partial class Rooms : Window
    {
        private Curve balconyWall;
        private Curve entranceWall;
        private List<Curve> walls;
        private XYZ leftTopPoint;
        private GeneratedArea rooms;
        private readonly Document document;

        public Rooms(GeometryObject balconyWall, GeometryObject entranceWall, CurveLoop walls, Document document)
        {
            InitializeComponent();
            this.balconyWall = balconyWall as Curve;
            this.entranceWall = entranceWall as Curve;
            this.walls = TransformData.ParseCurveIterator(walls.GetCurveLoopIterator());
            this.document = document;
            leftTopPoint = GetLeftAndRightPoints(this.walls).Item1;
            rooms = GenerateRooms();
            DrawLines();
            //CreateAppartment();
        }

        public GeneratedArea GenerateRooms()
        {

            var areaInfo = new AreaInfo(TransformData.TransformAutodeskWallsToApi(balconyWall, entranceWall, walls));

            var roomsGenerator = new RoomsGenerator(areaInfo,
                new List<RoomType> { RoomType.Corridor, RoomType.Kitchen, RoomType.Bathroom, RoomType.Default },
                AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            return roomsGenerator.GenerateArea();
        }

        public void DrawLines()
        {
            foreach (var pair in rooms.Rooms)
            {
                for (var i = 0; i < pair.Item2.Count; i++)
                {
                    var startPoint = pair.Item2[i];
                    var endPoint = pair.Item2[(i + 1) % pair.Item2.Count];

                    var line = new System.Windows.Shapes.Line
                    {
                        X1 = (startPoint.X - leftTopPoint.X) * 10,
                        Y1 = (startPoint.Y - leftTopPoint.Y) * 10,
                        X2 = (endPoint.X - leftTopPoint.X) * 10,
                        Y2 = (endPoint.Y - leftTopPoint.Y) * 10,
                        Stroke = Brushes.Black
                    };

                    RoomCanvas.Children.Add(line);
                }
            }
        }

        public void CreateAppartment()
        {
            using (var transaction = new Transaction(document, "Create appartment"))
            {
                foreach (var pair in rooms.Rooms)
                {
                    var curves = new List<Curve>();
                    var level = document.ActiveView.GenLevel;

                    for (var i = 0; i < pair.Item2.Count; i++)
                    {
                        var line = pair.Item2[i];
                        var nextLine = pair.Item2[(i + 1) % pair.Item2.Count];
                        var startPoint = new XYZ(line.X, line.Y, leftTopPoint.Z);
                        var endPoint = new XYZ(nextLine.X, nextLine.Y, leftTopPoint.Z);
                        curves.Add(Line.CreateBound(startPoint, endPoint));
                    }

                    var (leftPoint, rightPoint) = GetLeftAndRightPoints(curves);

                    transaction.Start();
                    Autodesk.Revit.DB.Wall.Create(document, Line.CreateBound(leftPoint, rightPoint), level.Id, false);
                    transaction.Commit();

                    // document.Create.NewFamilyInstance();
                }
            }
        }

        public (XYZ, XYZ) GetLeftAndRightPoints(List<Curve> curves)
        {
            var leftCurve = curves[0];
            var rightCurve = curves[0];

            for (var i = 0; i < curves.Count; i++)
            {
                var point = curves[i].GetEndPoint(0);
                var leftPoint = leftCurve.GetEndPoint(0);
                var rightPoint = rightCurve.GetEndPoint(0);

                if (point.X < leftPoint.X && point.Y < leftPoint.Y)
                {
                    leftCurve = curves[i];
                }

                if (point.X > rightPoint.X && point.Y > rightPoint.Y)
                {
                    rightCurve = curves[i];
                }
            }

            return (leftCurve.GetEndPoint(0), rightCurve.GetEndPoint(0));
        }
    }
}