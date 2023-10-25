using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class AreaInfo : Shape
    {
        public readonly IList<Wall> Walls;


        public AreaInfo(IList<Wall> walls) : base(walls)
        {
            Walls = walls;
        }
    }
}
