using AreaRoomsAPI.Info;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RevitPlugin
{
    /// <summary>
    /// Логика взаимодействия для ApartmentParameters.xaml
    /// </summary>
    public partial class ApartmentParameters : Window
    {
        private Curve balconyWall;
        private Curve entranceWall;
        private CurveLoop walls;
        private readonly Document document;

        public ApartmentParameters(GeometryObject balconyWall, GeometryObject entranceWall, CurveLoop walls,
            Document document)
        {
            InitializeComponent();
            this.document = document;
            this.balconyWall = balconyWall as Curve;
            this.entranceWall = entranceWall as Curve;
            this.walls = walls;
        }

        public List<RoomType> GetRoomTypesByRoomsCount(int roomCount, bool isSeparatedToilet, bool isHaveLoggia,
            bool isHaveWardrobe)
        {
            var result = new List<RoomType>
            {
                RoomType.Corridor,
                RoomType.Kitchen,
                RoomType.Bathroom
            };

            for (var i = 0; i < roomCount; i++)
            {
                result.Add(RoomType.Default);
            }

            if (isSeparatedToilet) result.Add(RoomType.Toilet);
            if (isHaveLoggia) result.Add(RoomType.Loggia);
            if (isHaveWardrobe) result.Add(RoomType.Wardrobe);

            return result;
        }

        public AreaRoomsFormatsInfo GetAreaRoomFormatsInfo()
        {
            var formatsInfo = new Dictionary<RoomType, RoomFormat>();

            var types = Enum.GetValues(typeof(RoomType));

            foreach (var type in types)
            {
                formatsInfo.Add((RoomType)type, GetRoomFormatByRoomType((RoomType)type));
            }

            return new AreaRoomsFormatsInfo(formatsInfo);
        }

        public RoomFormat GetRoomFormatByRoomType(RoomType roomType)
        {
            var parameters = GetParametersByRoomType(roomType);

            var format = new RoomFormat(minWidth: parameters["minWidth"], minSquare: parameters["minArea"]);

            return format;
        }

        public Dictionary<string, double> GetParametersByRoomType(RoomType roomType)
        {
            var parameters = new Dictionary<string, double>();
            (string, string) textBoxes;
            switch (roomType)
            {
                case RoomType.Default:
                    textBoxes.Item1 = RoomWidth.Text;
                    textBoxes.Item2 = AreaRoom.Text;
                    break;
                case RoomType.Bathroom:
                    textBoxes.Item1 = GetDataFromRadioButtons(new List<RadioButton>
                        { FirstWidthBath, SecondWidthBath, ThirdBathWidth });
                    textBoxes.Item2 = AreaBath.Text;
                    break;
                case RoomType.Kitchen:
                    textBoxes.Item1 = KitchenWidth.Text;
                    textBoxes.Item2 = AreaKitchen.Text;
                    break;
                case RoomType.Toilet:
                    textBoxes.Item1 = ToiletteWidth.Text;
                    textBoxes.Item2 = AreaToilet.Text;
                    break;
                case RoomType.Corridor:
                    textBoxes.Item1 = CorridorWidth.Text;
                    textBoxes.Item2 = AreaCorridor.Text;
                    break;
                case RoomType.Wardrobe:
                    textBoxes.Item1 = "0";
                    textBoxes.Item2 = "0";
                    break;
                case RoomType.Loggia:
                    textBoxes.Item1 = LoggiaWidth.Text;
                    textBoxes.Item2 = AreaLoggia.Text;
                    break;
                default:
                    textBoxes.Item1 = "0";
                    textBoxes.Item2 = "0";
                    break;
            }

            if (!double.TryParse(textBoxes.Item1, out _))
                textBoxes.Item1 = "0,0";
            parameters.Add("minWidth", double.Parse(textBoxes.Item1));
            if (!double.TryParse(textBoxes.Item2, out _))
                textBoxes.Item2 = "0,0";
            parameters.Add("minArea", double.Parse(textBoxes.Item2));

            return parameters;
        }

        public string GetDataFromRadioButtons(IList<RadioButton> buttons)
        {
            var result = "";
            foreach (RadioButton button in buttons)
            {
                if (button.IsChecked == true)
                {
                    result = (string)button.Content;
                }
            }

            return result;
        }

        public void CreateTextBlock()
        {
        }

        private void Generate_Room(object sender, RoutedEventArgs e)
        {
            var rooms = GetRoomTypesByRoomsCount(int.Parse(RoomsCount.Text), false, false, false);
            var roomFormats = GetAreaRoomFormatsInfo();
            var roomsWindow = new Rooms(balconyWall, entranceWall, walls, document, rooms, roomFormats);
            roomsWindow.ShowDialog();
            Close();
        }
    }
}