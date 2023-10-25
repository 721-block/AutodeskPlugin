using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class RoomsGenerator
    {
        private readonly IList<PointD> areaPoints;

        private readonly double areaSquare;

        private readonly IList<RoomType> rooms;

        private readonly AreaRoomsFormatsInfo formats;

        public RoomsGenerator(AreaInfo areaInfo, IList<RoomType> roomTypes, AreaRoomsFormatsInfo formats)
        {
            areaPoints = areaInfo.Points;
            areaSquare = areaInfo.Square;
            rooms = roomTypes;
            this.formats = formats;
        }

        public GeneratedArea GenerateArea()
        {
            var dictionary = new Dictionary<RoomType, IList<PointD>>();

            return new GeneratedArea(dictionary);
        }
    }
}
