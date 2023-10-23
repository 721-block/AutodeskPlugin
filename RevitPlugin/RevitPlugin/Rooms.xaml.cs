using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;

namespace RevitPlugin
{
    /// <summary>
    /// Логика взаимодействия для Rooms.xaml
    /// </summary>
    public partial class Rooms : Window
    {
        private Element pickedBox;
        private Element entranceWall;
        private Element balconyWall;

        public Rooms(Element pickedBox, Element entranceWall, Element balconyWall)
        {
            InitializeComponent();
            this.pickedBox = pickedBox;
            this.entranceWall = entranceWall;
            this.balconyWall = balconyWall;
            var newList = new List<object> { pickedBox, entranceWall, balconyWall };
            RoomsView.ItemsSource = newList;
        }
    }
}
