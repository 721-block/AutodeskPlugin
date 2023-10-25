using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    internal class RoomInfo : Shape
    {
        
        public readonly RoomType roomType;

        public RoomInfo(IList<PointD> points, RoomType roomType) : base(points)
        {
            this.roomType = roomType;
        }
    }
}
