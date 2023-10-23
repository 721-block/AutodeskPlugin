using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AreaRoomsAPI
{
    public class Wall
    {
        public readonly Point startPoint;
        public readonly Point endPoint;
        public readonly WallType wallType;

        public Wall(Point startPoint, Point endPoint, WallType wallType)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.wallType = wallType;
        }
    }
}
