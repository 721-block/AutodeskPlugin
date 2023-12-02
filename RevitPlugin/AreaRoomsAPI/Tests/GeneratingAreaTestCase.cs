using NUnit.Framework;
using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Tests
{
    [TestFixture]
    public class GeneratingAreaTestCase
    {
        [TestCase()]
        public void TestingRectangleArea()
        {
            var walls = new List<Wall>()
            {
                new Wall(new PointD(0, 0), new PointD(24.942546945512731, 0), WallType.Standart),
                new Wall(new PointD(24.942546945512731, 0), new PointD(24.942546945512731, 15.716982738649051), WallType.Standart),
                new Wall(new PointD(24.942546945512731, 15.716982738649051), new PointD(0, 15.716982738649051), WallType.Standart),
                new Wall(new PointD(0, 15.716982738649051), new PointD(0, 0), WallType.Standart),
            };

            var areaInfo = new AreaInfo(walls, 0, new List<RoomType> { RoomType.Default, RoomType.Corridor, RoomType.Bathroom, RoomType.Kitchen}, 0);

            var roomGenerator = new RoomsGenerator(areaInfo, AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            var area = roomGenerator.GenerateArea();
        }
    }
}
