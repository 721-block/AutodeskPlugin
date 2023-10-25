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
        //private readonly Dictionary<RoomType, int> roomsPriority = new Dictionary<RoomType, int>() 
        //{
        //    {RoomType.Kitchen, 3 },
        //    {RoomType.Default, 3 },
        //    {RoomType.Wardrobe, 1 },
        //    {RoomType.Toilet, 1 },
        //    {RoomType.Bathroom, 2 },
        //    {RoomType.Loggia, 2 }
        //};

        private readonly IList<PointD> areaPoints;

        private readonly double areaSquare;

        private readonly int priorityRoomsCount;

        private readonly HashSet<RoomType> priorityRoomsType = new HashSet<RoomType>() { RoomType.Default, RoomType.Kitchen, RoomType.Loggia };

        private readonly IList<RoomType> rooms;

        private IList<RoomType> roomsRemaining;

        private readonly AreaRoomsFormatsInfo formats;

        public RoomsGenerator(AreaInfo areaInfo, IList<RoomType> roomTypes, AreaRoomsFormatsInfo formats)
        {
            areaPoints = areaInfo.Points;
            areaSquare = areaInfo.GetSquare();
            rooms = roomTypes;
            roomsRemaining = rooms;
            this.formats = formats;

            foreach (RoomType roomType in roomTypes)
            {
                if (priorityRoomsType.Contains(roomType))
                {
                    priorityRoomsCount++;
                }
            }
        }

        public GeneratedArea GenerateArea()
        {
            var list = new List<(RoomType, IList<PointD>)>();

            var tree = CreateTree();

            



            return new GeneratedArea(list);
        }

        private RoomsTreeNode CreateTree()
        {
            var tree = new RoomsTreeNode(new Shape(areaPoints), 0);
            var isDivisionByY = tree.shape.Height > tree.shape.Width;

            var leftShape = 

            if (CanInsertRoom(tree.shape, 1))
            {

            }
            else
            {
                
            }

            return tree;
        }

        private bool CanInsertRoom(Shape shape, int depth)
        {
            if (depth ==)
        }

        private RoomsTreeNode CreateTree(RoomsTreeNode tree)
        {

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
