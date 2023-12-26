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
        private readonly Curve balconyWall;
        private readonly Curve entranceWall;
        private readonly List<Curve> walls;
        private readonly List<RoomType> rooms;
        private readonly AreaRoomsFormatsInfo roomsFormats;
        private readonly XYZ leftTopPoint;
        private readonly GeneratedArea generatedRooms;
        private readonly Document document;

        public Rooms(GeometryObject balconyWall, GeometryObject entranceWall, CurveLoop walls,
            Document document, List<RoomType> rooms, AreaRoomsFormatsInfo roomsFormats)
        {
            InitializeComponent();
            this.balconyWall = balconyWall as Curve;
            this.entranceWall = entranceWall as Curve;
            this.walls = TransformData.ParseCurveIterator(walls.GetCurveLoopIterator());
            this.document = document;
            this.rooms = rooms;
            this.roomsFormats = roomsFormats;
            leftTopPoint = GetLeftAndRightPoints(this.walls).Item1;
            generatedRooms = GenerateRooms();
            DrawLines();
        }

        public GeneratedArea GenerateRooms()
        {

            var areaInfo = new AreaInfo(
                TransformData.TransformAutodeskWallsToApi(balconyWall, entranceWall, walls, new XYZ(), new XYZ()),
                0.0, rooms
                );

            var roomsGenerator = new RoomsGenerator(areaInfo, roomsFormats);

            return roomsGenerator.GenerateArea();
        }

        public void DrawLines()
        {
            // stack to debug with different rooms colors
            var stack = new Stack<SolidColorBrush>(new List<SolidColorBrush>{Brushes.Black, Brushes.Blue, Brushes.Red, Brushes.Gold});
            
            foreach (var pair in generatedRooms.Rooms)
            {
                var stroke = stack.Pop();
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
            foreach (var pair in generatedRooms.Rooms)
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

        private void CanvasPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Generate_Appartment(object sender, RoutedEventArgs e)
        {
            CreateAppartment();
        }
    }
}