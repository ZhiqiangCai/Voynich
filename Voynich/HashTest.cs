using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voynich
{
    class Point : IEquatable<Point>
    {
        
        int X
        {
            get { return x; }
        }
        int Y
        {
            get { return y; }
        }
        int x;
        int y;
        public Point(int x,int y)
        {
            this.x = x;this.y = y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point);
        }

        public bool Equals(Point other)
        {
            return other != null &&
                   x == other.x &&
                   y == other.y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return EqualityComparer<Point>.Default.Equals(point1, point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }
    }
    class Triplet : IEquatable<Triplet>
    {
        public int x, y, z;

        public override bool Equals(object obj)
        {
            return Equals(obj as Triplet);
        }

        public bool Equals(Triplet other)
        {
            return other != null &&
                   x == other.x &&
                   y == other.y &&
                   z == other.z;
        }

        public override int GetHashCode()
        {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }
    }
}
