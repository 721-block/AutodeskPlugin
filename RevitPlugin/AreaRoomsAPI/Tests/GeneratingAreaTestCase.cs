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
        [TestCase(22, 22)]
        [TestCase(10, 8)]
        [TestCase(30.356950254188632, 16.574111909812757)]
        public void TestingRectangleArea(double width, double height)
        {
            var walls = new List<Wall>()
            {
                new StandartWall(new PointD(0, 0), new PointD(width, 0)),
                new BalconyWall(new PointD(width, 0), new PointD(width, height), new List<(double, double)>{(0, height/2)}),
                new StandartWall(new PointD(width, height), new PointD(0, height)),
                new EnterWall(new PointD(0, height), new PointD(0, 0), (height/2 - 1, height/2 + 1)),
            };

            var areaInfo = new AreaInfo(
                walls, 
                0, 
                new List<RoomType> { RoomType.Default, RoomType.Corridor, RoomType.Bathroom, RoomType.Kitchen});

            var roomGenerator = new RoomsGenerator(areaInfo, AreaRoomsFormatsInfo.GetAreaFormatsInfo(AreaType.Economy));

            var area = roomGenerator.GenerateArea();
        }
    }
}
