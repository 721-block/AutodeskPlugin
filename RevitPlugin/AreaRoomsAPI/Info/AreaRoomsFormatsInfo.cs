using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class AreaRoomsFormatsInfo
    {
        public readonly double AspectRatio = 0.5; 

        private readonly ReadOnlyDictionary<RoomType, RoomFormat> formats;

        private static AreaRoomsFormatsInfo economyAreaRoomsFormatsInfo => new AreaRoomsFormatsInfo(new Dictionary<RoomType, RoomFormat>
        {
            {RoomType.Default, new RoomFormat(minWidth: 3, minSquare: 12) },
            {RoomType.Bathroom, new RoomFormat(minWidth: 1.65, maxWidth: 1.85, minSquare: 4) },
            {RoomType.Toilet, new RoomFormat(minWidth: 0.8, minSquare: 1) },
            {RoomType.Kitchen, new RoomFormat(minWidth: 2.8, minSquare: 10) },
            {RoomType.Loggia, new RoomFormat(minWidth: 0.8, maxWidth: 1.5, maxSquare: 3) },
            {RoomType.Corridor, new RoomFormat(recommendedWidth: 1.1, maxSquare: 5) },
            {RoomType.Wardrobe, new RoomFormat(minWidth: 0.5, maxWidth: 1, minSquare: 0.25) }
        });

        public RoomFormat this[RoomType roomType]
        {
            get
            {
                return formats[roomType];
            }
        }

        public AreaRoomsFormatsInfo(IDictionary<RoomType, RoomFormat> formats) 
        { 
            this.formats = new ReadOnlyDictionary<RoomType, RoomFormat>(formats);
        }

        public static AreaRoomsFormatsInfo GetAreaFormatsInfo(AreaType areaType)
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
