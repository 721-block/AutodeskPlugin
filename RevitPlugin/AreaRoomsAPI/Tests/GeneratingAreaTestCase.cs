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
        [TestCase(24.942546945512731, 15.716982738649051)]
        [TestCase(20, 16)]
        [TestCase(30.356950254188632, 16.574111909812757)]
        public void TestingRectangleArea(double width, double height)
        {
            var walls = new List<Wall>()
            {
                new Wall(new PointD(0, 0), new PointD(width, 0), WallType.Standart),
                new Wall(new PointD(width, 0), new PointD(width, height), WallType.Standart),
                new Wall(new PointD(width, height), new PointD(0, height), WallType.Standart),
                new Wall(new PointD(0, height), new PointD(0, 0), WallType.Standart),
            };

            var areaInfo = new AreaInfo(walls, 0, new List<RoomType> { RoomType.Default, RoomType.Corridor, RoomType.Bathroom, RoomType.Kitchen}, 0);

            var roomGenerator = new RoomsGenerator(areaInfo, AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            var area = roomGenerator.GenerateArea();
        }
    }
}
