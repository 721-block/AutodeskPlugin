using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
