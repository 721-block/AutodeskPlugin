using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class RoomFormat
    {
        public readonly double MinWidth;
        public readonly double MaxWidth = double.MaxValue;
        public readonly double MinSquare;
        public readonly double MaxSquare = double.MaxValue;
        public readonly double RecommendedWidth;

        public RoomFormat(double minWidth = 0, double maxWidth = double.MaxValue, double recommendedWidth = 0, double minSquare = 0, double maxSquare = double.MaxValue)
        {
            MinWidth = minWidth;
            MaxWidth = maxWidth;
            MinSquare = minSquare;
            MaxSquare = maxSquare;
            RecommendedWidth = recommendedWidth;
        }
    }
}
