using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public struct RoomFormat
    {
        public readonly double MinWidth;
        public readonly double MaxWidth;
        public readonly double MinSquare;
        public readonly double MaxSquare;
        public readonly double RecommendedWidth;

        public RoomFormat(double minWidth = 0, double maxWidth = double.MaxValue, double recommendedWidth = 0, double minSquare = 0, double maxSquare = double.MaxValue)
        {
            MinWidth = minWidth;
            MaxWidth = maxWidth;
            MinSquare = minSquare;
            MaxSquare = maxSquare;
            RecommendedWidth = recommendedWidth;
        }

        public static RoomFormat operator*(RoomFormat roomFormat, double number)
        {
            return new RoomFormat(roomFormat.MinWidth * number,
                roomFormat.MaxWidth * number,
                roomFormat.RecommendedWidth * number,
                roomFormat.MinSquare * number,
                roomFormat.MaxSquare * number
                );
        }
    }
}
