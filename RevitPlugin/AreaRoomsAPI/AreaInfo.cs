using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class AreaInfo
    {
        public readonly IList<WallType> walls;

        public AreaInfo(IList<WallType> walls)
        {
            this.walls = walls;
        }

        public int GetSquare()
        {
            return 0;
        }
    }
}
