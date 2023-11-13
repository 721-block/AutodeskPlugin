using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AreaRoomsAPI
{
    public struct PointD
    {
        public double X;

        public double Y;

        public PointD(double x, double y)
        {
            X = x; 
            Y = y;
        }

        public static bool operator ==(PointD left, PointD right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(PointD left, PointD right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PointD))
            {
                return false;
            }

            PointD pointD = (PointD)obj;
            if (this == pointD)
            {
                return pointD.GetType().Equals(GetType());
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
