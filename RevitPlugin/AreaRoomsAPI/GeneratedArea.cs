using AreaRoomsAPI.Info;
using System.Collections.Generic;

namespace AreaRoomsAPI
{
    public class GeneratedArea
    {
        public IReadOnlyDictionary<RoomType, IList<PointD>> Rooms { get; set; }

        public GeneratedArea(IReadOnlyDictionary<RoomType, IList<PointD>> rooms)
        {
            Rooms = rooms;
        }
    }
}