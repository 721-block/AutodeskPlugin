using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
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
            CreateAppartment();
        }

        public GeneratedArea GenerateRooms()
        {

            var areaInfo = new AreaInfo(
                TransformData.TransformAutodeskWallsToApi(balconyWall, entranceWall, walls),
                0.0, new List<RoomType> { RoomType.Corridor, RoomType.Kitchen, RoomType.Bathroom, RoomType.Default }
                );

            var roomsGenerator = new RoomsGenerator(areaInfo, AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            return roomsGenerator.GenerateArea();
        }

        public void DrawLines()
        {
            // stack to debug with different rooms colors
            var stack = new Stack<SolidColorBrush>(new List<SolidColorBrush>{Brushes.Black, Brushes.Blue, Brushes.Red, Brushes.Gold});
            
            foreach (var pair in rooms.Rooms)
            {
                var stroke = stack.Pop();
                for (var i = 0; i < pair.Item2.Count; i++)
                {
                    var startPoint = pair.Item2[i];
                    var endPoint = pair.Item2[(i + 1) % pair.Item2.Count];

                    var line = new System.Windows.Shapes.Line
                    {
                        X1 = (startPoint.X - leftTopPoint.X) * 25,
                        Y1 = (startPoint.Y - leftTopPoint.Y) * 25,
                        X2 = (endPoint.X - leftTopPoint.X) * 25,
                        Y2 = (endPoint.Y - leftTopPoint.Y) * 25,
                        //Stroke = Brushes.Black,
                        Stroke = stroke
                    };

                    RoomCanvas.Children.Add(line);
                }
            }
        }

        public void CreateAppartment()
        {
            var curves = new List<List<Curve>>();
            foreach (var pair in rooms.Rooms)
            {
                curves.Add(AutodeskAPICreator.GetCurvesByPoints(pair.Item2, document));
            }

            AutodeskAPICreator.CreateRooms(curves, document);
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