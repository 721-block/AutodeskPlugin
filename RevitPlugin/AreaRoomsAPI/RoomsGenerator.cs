using AreaRoomsAPI.Info;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            areaSquare = areaInfo.GetSquare();
            rooms = roomTypes;
            this.formats = formats;
        }

        public GeneratedArea GenerateArea()
        {
            var dictionary = new Dictionary<RoomType, IList<PointD>>();
            


            return new GeneratedArea(dictionary);
        }

        private class RoomsTreeNode
        {
            public int roomsCount;

            public readonly int depth;

            private IList<RoomInfo> leftRooms;

            private IList<RoomInfo> rightRooms;

            public RoomsTreeNode leftNext;

            public RoomsTreeNode rightNext;

            public Shape shape;

            public RoomsTreeNode(Shape shape, int depth)
            {
                this.shape = shape;
                this.depth = depth;
            }

            public int GetRoomCount()
            {
                if (roomsCount != 0)
                {
                    return roomsCount;
                }
                
                if (shape is RoomInfo)
                {
                    return 1;
                }

                return leftNext.GetRoomCount() + rightNext.GetRoomCount();
            }

            private IList<RoomInfo> GetLeftRooms()
            {
                if (shape is RoomInfo || leftRooms != null)
                {
                    return leftRooms;
                }

                return leftNext.leftRooms.Concat(leftNext.rightRooms).ToList();
            }

            private IList<RoomInfo> GetRightRooms()
            {
                if (shape is RoomInfo || rightRooms != null)
                {
                    return rightRooms;
                }

                return rightNext.leftRooms.Concat(rightNext.rightRooms).ToList();
            }
        }
    }
}
