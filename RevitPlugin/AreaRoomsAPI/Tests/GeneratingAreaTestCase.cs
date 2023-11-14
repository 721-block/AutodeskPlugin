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
                new Wall(new PointD(0, 0), new PointD(20, 0), WallType.Standart),
                new Wall(new PointD(20, 0), new PointD(20, 20), WallType.Standart),
                new Wall(new PointD(20, 20), new PointD(0, 20), WallType.Standart),
                new Wall(new PointD(0, 20), new PointD(0, 0), WallType.Standart),
            };

            var areaInfo = new AreaInfo(walls, 0, new List<RoomType> { RoomType.Default, RoomType.Corridor, RoomType.Bathroom, RoomType.Kitchen});

            var roomGenerator = new RoomsGenerator(areaInfo, AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            roomGenerator.GenerateArea();
        }
    }
}
