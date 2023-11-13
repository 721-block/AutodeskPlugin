using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI.Info
{
    public class Leaf
    {
        private const double MIN_LEAF_SIZE = 15;

        public double y, x, width, height; // положение и размер этого листа
        public int depth;

        public Leaf leftChild; // левый дочерний Leaf нашего листа
        public Leaf rightChild; // правый дочерний Leaf нашего листа
                                //public RoomInfo room; // комната, находящаяся внутри листа
                                //public Vector halls; // коридоры, соединяющие этот лист с другими листьями

        public Leaf(double x, double y, double width, double height, int depth = 0)
        {
            // инициализация листа
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        public Boolean Split(Random rnd)
        {
            // начинаем разрезать лист на два дочерних листа
            if (depth > 3)
                return false; // мы уже его разрезали! прекращаем!

            // определяем направление разрезания
            // если ширина более чем на 25% больше высоты, то разрезаем вертикально
            // если высота более чем на 25% больше ширины, то разрезаем горизонтально
            // иначе выбираем направление разрезания случайным образом
            Boolean splitH = rnd.NextDouble() > 0.5;
            if (width > height && width / height >= 1.25)
                splitH = false;
            else if (height > width && height / width >= 1.25)
                splitH = true;

            var max = (splitH ? height : width);// - MIN_LEAF_SIZE; // определяем максимальную высоту или ширину
            if (max <= MIN_LEAF_SIZE)
                return false; // область слишком мала, больше её делить нельзя...

            var split = rnd.NextDouble() * max;// (max - MIN_LEAF_SIZE) + MIN_LEAF_SIZE; // определяемся, где будем разрезать

            // создаём левый и правый дочерние листы на основании направления разрезания
            if (splitH)
            {
                leftChild = new Leaf(x, y, width, split, depth + 1);
                rightChild = new Leaf(x, y + split, width, height - split, depth + 1);
            }
            else
            {
                leftChild = new Leaf(x, y, split, height, depth + 1);
                rightChild = new Leaf(x + split, y, width - split, height, depth + 1);
            }
            return true; // разрезание выполнено!
        }

        public static void SplitArea(Leaf root, Random rnd)
        {
            const double MIN_LEAF_AREA = 40;

            var _leafs = new Queue<Leaf>();

            //Leaf l; // вспомогательный лист

            // сначала создаём лист, который будет "корнем" для всех остальных листьев.
            _leafs.Enqueue(root);

            while (_leafs.Count != 0)
            {
                var leaf = _leafs.Dequeue();
                // если этот лист слишком велик, или есть вероятность 75%...
                if (leaf.width * leaf.height > MIN_LEAF_AREA ||
                    rnd.NextDouble() > 0.25)
                {
                    if (leaf.Split(rnd)) // разрезаем лист!
                    {
                        // если мы выполнили разрезание, передаём дочерние листья в Vector, чтобы в дальнейшем можно было в цикле обойти и их
                        _leafs.Enqueue(leaf.leftChild);
                        _leafs.Enqueue(leaf.rightChild);
                    }
                }
            }
        }

        public static List<Leaf> GetLeafsList(Leaf root)
        {
            var stack = new Stack<Leaf>();
            stack.Push(root);
            var leafs = new List<Leaf>();

            while (stack.Count != 0)
            {
                var leaf = stack.Pop();
                if (leaf.leftChild == null || leaf.rightChild == null)
                {
                    leafs.Add(leaf);
                    continue;
                }
                stack.Push(leaf.leftChild);
                stack.Push(leaf.rightChild);
            }

            return leafs;
        }

        public IList<PointD> GetPoints(PointD startPoint)
        {
            x += startPoint.X;
            y += startPoint.Y;
            return new List<PointD>
            {
                new PointD(x, y),
                new PointD(x, y + height),
                new PointD(x+width, y+height),
                new PointD(x+width, y),
            };
        }
    }
}
