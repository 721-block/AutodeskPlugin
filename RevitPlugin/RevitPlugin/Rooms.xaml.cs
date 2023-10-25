using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevitPlugin
{
    public partial class Rooms : Window
    {
        private Curve balconyWall;
        private Curve entranceWall;
        private CurveLoopIterator walls;
        private XYZ leftTopPoint;
        private GeneratedArea rooms;

        public Rooms(GeometryObject balconyWall, GeometryObject entranceWall, CurveLoop walls)
        {
            InitializeComponent();
            this.balconyWall = balconyWall as Curve;
            this.entranceWall = entranceWall as Curve;
            this.walls = walls.GetCurveLoopIterator();
            leftTopPoint = this.walls.Current.GetEndPoint(0);
            rooms = GenerateRooms();
            DrawLines();
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
    }
}