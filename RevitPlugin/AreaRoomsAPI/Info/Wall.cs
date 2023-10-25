using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class Wall
    {
        public readonly PointD startPoint;
        public readonly PointD endPoint;
        public readonly WallType wallType;

        public Wall(PointD startPoint, PointD endPoint, WallType wallType)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.wallType = wallType;
        }
    }
}
