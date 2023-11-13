using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public class NewGenerator
    {
        private readonly Dictionary<RoomType, int> roomsPriority = new Dictionary<RoomType, int>()
        {
            {RoomType.Kitchen, 3 },
            {RoomType.Default, 3 },
            {RoomType.Wardrobe, 1 },
            {RoomType.Toilet, 1 },
            {RoomType.Bathroom, 2 },
            {RoomType.Loggia, 2 }
        };

        private readonly Queue<RoomType> roomsPriorityQueue;

        private readonly IList<PointD> areaPoints;

        private readonly IList<RoomType> rooms;

        private IList<RoomType> roomsRemaining;

        private readonly AreaRoomsFormatsInfo formats;
        private readonly AreaInfo areaInfo;

        public NewGenerator(AreaInfo areaInfo, IList<RoomType> roomTypes, AreaRoomsFormatsInfo formats)
        {
            areaPoints = areaInfo.Points;
            rooms = roomTypes;
            roomsRemaining = rooms;
            this.formats = formats;
            this.areaInfo = areaInfo;
        }

        public GeneratedArea GenerateArea()
        {
            var list = new List<(RoomType, IList<PointD>)>();
            var startPoint = areaPoints.OrderBy(x => x.X).ThenBy(y => y.Y).First();
            var root = new Leaf(startPoint.X, startPoint.Y, areaInfo.Width, areaInfo.Height);
            Leaf.SplitArea(root, new Random());
            var leafs = Leaf.GetLeafsList(root);
            foreach (Leaf leaf in leafs)
            {
                list.Add((RoomType.Toilet, leaf.GetPoints(startPoint)));
            }

            return new GeneratedArea(list);
        }
    }
}
