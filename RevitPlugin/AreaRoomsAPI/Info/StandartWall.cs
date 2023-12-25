using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class StandartWall : Wall
    {
        public StandartWall(PointD startPoint, PointD endPoint) : base(startPoint, endPoint, WallType.Standart)
        {
        }
    }
}
