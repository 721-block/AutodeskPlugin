using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class AreaRoomsFormatsInfo
    {
        public readonly double AspectRatio = 0.5; 

        public readonly ReadOnlyDictionary<RoomType, RoomFormat> formats;

        private static AreaRoomsFormatsInfo economyAreaRoomsFormatsInfo => new AreaRoomsFormatsInfo(new Dictionary<RoomType, RoomFormat>
        {
            {RoomType.Default, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Bathroom, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Toilet, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Kitchen, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Loggia, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Corridor, new RoomFormat(minWidth: 3, minSquare: 10) },
            {RoomType.Wardrobe, new RoomFormat(minWidth: 3, minSquare: 10) }
        });

        public AreaRoomsFormatsInfo(IDictionary<RoomType, RoomFormat> formats) 
        { 
            this.formats = new ReadOnlyDictionary<RoomType, RoomFormat>(formats);
        }

        public static AreaRoomsFormatsInfo GetTypeInfo(AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.Economy:
                    return economyAreaRoomsFormatsInfo;

                default:
                    return economyAreaRoomsFormatsInfo;
            }
        }
    }
}
