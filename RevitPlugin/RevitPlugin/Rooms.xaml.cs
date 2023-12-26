using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private readonly List<Canvas> allCanvas;
        private readonly double appartmentWidth;
        private readonly double appartmentHeight;

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
            (appartmentWidth, appartmentHeight) = GetWidthAndHeight();
            allCanvas = GetAllCanvs();
            generatedRooms = GenerateRooms();
            DrawAllCanvas();
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

        public void DrawAllCanvas()
        {
            foreach (var canvas in allCanvas)
            {
                var mulitpier = GetMultipier(canvas, appartmentWidth, appartmentHeight);
                var withDelta = GetDeltaCoordinates(mulitpier, canvas.Width, appartmentWidth);
                var heightDelta = GetDeltaCoordinates(mulitpier, canvas.Height, appartmentHeight);
                DrawAppartment(canvas, mulitpier, withDelta, heightDelta);
            }
        }

        public void DrawAppartment(Canvas canvas, double multipier, double widthDelta, double heightDelta)
        {
            //var stack = new Stack<SolidColorBrush>(new List<SolidColorBrush>{Brushes.Black, Brushes.Blue, Brushes.Red, Brushes.Gold});
            
            foreach (var pair in generatedRooms.Rooms)
            {
                for (var i = 0; i < pair.Item2.Count; i++)
                {
                    var startPoint = pair.Item2[i];
                    var endPoint = pair.Item2[(i + 1) % pair.Item2.Count];

                    var line = new System.Windows.Shapes.Line
                    {
                        X1 = (startPoint.X - leftTopPoint.X) * multipier + widthDelta,
                        Y1 = (startPoint.Y - leftTopPoint.Y) * multipier + heightDelta,
                        X2 = (endPoint.X - leftTopPoint.X) * multipier + widthDelta,
                        Y2 = (endPoint.Y - leftTopPoint.Y) * multipier + heightDelta,
                        Stroke = Brushes.Black,
                    };

                    canvas.Children.Add(line);
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

        private (double, double) GetWidthAndHeight()
        {
            var widthAndHeight = (0.0, 0.0);
            foreach (var curve in walls)
            {
                var startPoint = curve.GetEndPoint(0);
                var endPoint = curve.GetEndPoint(1);
                if (startPoint.X == endPoint.X && (IsXYZEquals(startPoint, leftTopPoint) || IsXYZEquals(endPoint, leftTopPoint)))
                {
                    widthAndHeight.Item2 = curve.ApproximateLength;
                }
                else if (startPoint.Y == endPoint.Y && (IsXYZEquals(startPoint, leftTopPoint) || IsXYZEquals(endPoint, leftTopPoint)))
                {
                    widthAndHeight.Item1 = curve.ApproximateLength;
                }
            }
            return widthAndHeight;
        }

        private bool IsXYZEquals(XYZ first, XYZ second)
        {
            return first.X == second.X && first.Y == second.Y && first.Z == second.Z;
        } 

        private double GetMultipier(Canvas canvas, double width, double height)
        {
            var canvasWidth = canvas.Width;
            var canvasHeight = canvas.Height;
            var multipier = 0.0;
            do
            {
                multipier += 0.5;
            }
            while (width * multipier < canvasWidth && height * multipier < canvasHeight);

            return multipier - 0.5;
        }

        private double GetDeltaCoordinates(double multipier, double canvasLength, double length)
        {
            return (canvasLength - multipier * length) / 2;
        }

        private List<Canvas> GetAllCanvs()
        {
            return new List<Canvas> { RoomCanvas, FirstPreview, SecondPreview, ThirdPreview, FourthPreview, FifthPreview, SixthPreview};
        }

        private int GetCanvasIndex(Canvas currentCanvas)
        {
            for (var i = 0; i < allCanvas.Count; i++) 
            {
                if (allCanvas[i].Equals(currentCanvas))
                    return i;
            }
            throw new ArgumentException($"canvas with name {currentCanvas.Name} doesnt exist");
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