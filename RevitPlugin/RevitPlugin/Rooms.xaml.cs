using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RevitPlugin
{
    /// <summary>
    /// Логика взаимодействия для Rooms.xaml
    /// </summary>
    public partial class Rooms : Window
    {
        private Curve balconyWall;
        private Curve entranceWall;
        private CurveLoopIterator walls;
        private XYZ leftTopPoint;

        public Rooms(GeometryObject balconyWall, GeometryObject entranceWall, CurveLoop walls)
        {
            InitializeComponent();
            this.balconyWall = balconyWall as Curve;
            this.entranceWall = entranceWall as Curve;
            this.walls = walls.GetCurveLoopIterator();
            leftTopPoint = this.walls.Current.GetEndPoint(0);

            DrawLines();
        }

        public void DrawLines()
        {
            var isHaveNextElement = true;
            while (isHaveNextElement)
            {
                var wall = walls.Current;
                var startPoint = wall.GetEndPoint(0);
                var endPoint = wall.GetEndPoint(1);

                var line = new System.Windows.Shapes.Line
                {
                    X1 = (leftTopPoint.X - startPoint.X) * 10,
                    Y1 = (leftTopPoint.Y - startPoint.Y) * 10,
                    X2 = (leftTopPoint.X - endPoint.X) * 10,
                    Y2 = (leftTopPoint.Y - endPoint.Y) * 10,
                    Stroke = Brushes.Black
                };

                RoomCanvas.Children.Add(line);

                isHaveNextElement = walls.MoveNext();
            }
        }
    }
}
