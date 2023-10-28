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
    public class RoomsGenerator
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

        private readonly double areaSquare;

        private readonly IList<RoomType> rooms;

        private IList<RoomType> roomsRemaining;

        private readonly AreaRoomsFormatsInfo formats;

        public RoomsGenerator(AreaInfo areaInfo, IList<RoomType> roomTypes, AreaRoomsFormatsInfo formats)
        {
            areaPoints = areaInfo.Points;
            areaSquare = areaInfo.GetSquare();
            rooms = roomTypes;
            var roomsPriorityList = new List<(int, RoomType)>();
            roomsRemaining = rooms;
            this.formats = formats;

            foreach (RoomType roomType in roomTypes)
            {
                if (roomType != RoomType.Corridor)
                {
                    roomsPriorityList.Add((roomsPriority[roomType], roomType));
                }
            }

            roomsPriorityQueue = new Queue<RoomType>(roomsPriorityList.OrderBy(x => x.Item1).Select(x => x.Item2));
        }

        public GeneratedArea GenerateArea()
        {
            var list = new List<(RoomType, IList<PointD>)>();

            var tree = CreateTree();
            ResizeRooms(tree);
            list.Add((RoomType.Corridor, AddCorridor(tree)));
            AddRooms(tree, list);

            return new GeneratedArea(list);
        }

        private IList<PointD> AddCorridor(RoomsTreeNode tree)
        {
            var firstConnection = tree.Connection;
            var maxLength = tree.leftNext.Connection.Select(x => x.X).Concat(tree.rightNext.Connection.Select(x => x.X)).Max();
            tree.rightNext.ResizeForCorridor(Side.Bottom, formats.formats[RoomType.Corridor].MinWidth);
            return new PointD[4] { 
                firstConnection[0], 
                new PointD(firstConnection[0].X - maxLength, firstConnection[0].Y), 
                new PointD(firstConnection[0].X - maxLength, firstConnection[0].Y + formats.formats[RoomType.Corridor].MinWidth), 
                new PointD(firstConnection[0].X, firstConnection[0].Y + formats.formats[RoomType.Corridor].MinWidth) 
            }; 
        }

        private void ResizeRooms(RoomsTreeNode tree)
        {
            return;
        }

        private void AddRooms(RoomsTreeNode tree, List<(RoomType, IList<PointD>)> result)
        {
            if (tree.Shape is RoomInfo info)
            {
                result.Add((info.roomType, info.Points));
                return;
            }

            AddRooms(tree.leftNext, result);
            AddRooms(tree.rightNext, result);
        }

        private RoomsTreeNode CreateTree()
        {
            var resultTree = new RoomsTreeNode(new Shape(areaPoints), 0);
            var count = 1;
            var queue = new Queue<RoomsTreeNode>();
            queue.Enqueue(resultTree);

            while (queue.Count > 0)
            {
                var tree = queue.Dequeue();
                var isDivisionByY = tree.Shape.Height > tree.Shape.Width;
                

                if (count == rooms.Count - 1)
                {
                    tree.Shape = new RoomInfo(tree.Shape.Points.ToArray(), roomsPriorityQueue.Dequeue());
                    continue;
                }

                var shapes = tree.Shape.SplitInHalf(isDivisionByY);
                tree.Connection = shapes.connection;
                tree.leftNext = new RoomsTreeNode(shapes.Item1[0], tree.depth + 1);
                tree.rightNext = new RoomsTreeNode(shapes.Item1[1], tree.depth + 1);
                queue.Enqueue(tree.leftNext);
                queue.Enqueue(tree.rightNext);
                count++;
            }

            return resultTree;
        }

        private RoomsTreeNode CreateTree(RoomsTreeNode tree)
        {
            return null;
        }

        private class RoomsTreeNode
        {
            public int roomsCount;

            public readonly int depth;

            private IList<RoomInfo> leftRooms;

            private IList<RoomInfo> rightRooms;

            public RoomsTreeNode leftNext;

            public RoomsTreeNode rightNext;

            public IList<PointD> Connection;

            public Shape Shape;

            public RoomsTreeNode(Shape shape, int depth)
            {
                Shape = shape;
                this.depth = depth;
            }

            public void ResizeForCorridor(Side side, double value)
            {
                List<PointD> sidePoints = null;
                var points = Shape.Points;
                (int index1, int index2) indexes;

                switch (side)
                {
                    case Side.Left:
                        sidePoints = Shape.Points.OrderBy(point => point.X).Take(2).ToList();
                        indexes = GetIndexes(sidePoints);
                        Shape.Points[indexes.index1] = new PointD(points[indexes.index1].X + value, points[indexes.index1].Y);
                        Shape.Points[indexes.index2] = new PointD(points[indexes.index2].X + value, points[indexes.index2].Y);
                        break;
                    case Side.Right:
                        sidePoints = Shape.Points.OrderByDescending(point => point.X).Take(2).ToList();
                        indexes = GetIndexes(sidePoints);
                        Shape.Points[indexes.index1] = new PointD(points[indexes.index1].X - value, points[indexes.index1].Y);
                        Shape.Points[indexes.index2] = new PointD(points[indexes.index2].X - value, points[indexes.index2].Y);
                        break;
                    case Side.Top:
                        sidePoints = Shape.Points.OrderByDescending(point => point.Y).Take(2).ToList();
                        indexes = GetIndexes(sidePoints);
                        Shape.Points[indexes.index1] = new PointD(points[indexes.index1].X, points[indexes.index1].Y - value);
                        Shape.Points[indexes.index2] = new PointD(points[indexes.index2].X, points[indexes.index2].Y - value);
                        break;
                    case Side.Bottom:
                        sidePoints = Shape.Points.OrderBy(point => point.X).Take(2).ToList();
                        indexes = GetIndexes(sidePoints);
                        Shape.Points[indexes.index1] = new PointD(points[indexes.index1].X, points[indexes.index1].Y + value);
                        Shape.Points[indexes.index2] = new PointD(points[indexes.index2].X, points[indexes.index2].Y + value);
                        break;
                }

                ResizeRoomsInShape();
            }

            private void ResizeRoomsInShape()
            {
                if (Shape is RoomInfo)
                    return;
            }

            private (int index1, int index2) GetIndexes(IList<PointD> sidePoints)
            {
                var index1 = Shape.Points.IndexOf(sidePoints[0]);
                var index2 = Shape.Points.IndexOf(sidePoints[1]);

                return (index1, index2);
            }

            public int GetRoomCount()
            {
                if (roomsCount != 0)
                {
                    return roomsCount;
                }
                
                if (Shape is RoomInfo)
                {
                    return 1;
                }

                return leftNext.GetRoomCount() + rightNext.GetRoomCount();
            }

            private IList<RoomInfo> GetLeftRooms()
            {
                if (Shape is RoomInfo || leftRooms != null)
                {
                    return leftRooms;
                }

                return leftNext.leftRooms.Concat(leftNext.rightRooms).ToList();
            }

            private IList<RoomInfo> GetRightRooms()
            {
                if (Shape is RoomInfo || rightRooms != null)
                {
                    return rightRooms;
                }

                return rightNext.leftRooms.Concat(rightNext.rightRooms).ToList();
            }
        }
    }
}
