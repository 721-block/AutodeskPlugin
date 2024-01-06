using AreaRoomsAPI;
using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugin
{
    public class TestData
    {
        public static GeneratedArea FirstTest()
        {
            var data = new List<(RoomType, IList<PointD>)>
            {
                (RoomType.Default,
                    new List<PointD> { new PointD(0, 0), new PointD(0, 100), new PointD(10, 100), new PointD(10, 0) })
            };
            return new GeneratedArea(data);
        }
    }
}