using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ElementNS
{
    public struct Tuple3D<T>
    {
        public T a, b, c;

        public Tuple3D(T A, T B, T C)
        {
            this.a = A;
            this.b = B;
            this.c = C;
        }

        public List<T> flatten()
        {
            return new List<T> { a, b, c };
        }


        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }


            var left = flatten();
            left.Sort();
            var right = ((Tuple3D<T>)obj).flatten();
            right.Sort();

            return left.SequenceEqual(right);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return new HashSet<T> { a, b, c }.GetHashCode();
        }
    }

    public struct Tuple4D<T>
    {
        public T a, b, c, d;

        public Tuple4D(T A, T B, T C, T D)
        {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
        }

        public List<T> flatten()
        {
            return new List<T> { a, b, c, d };
        }

        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var left = flatten();
            left.Sort();
            var right = ((Tuple4D<T>)obj).flatten();
            right.Sort();

            return left.SequenceEqual(right);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return new HashSet<T> { a, b, c, d }.GetHashCode();
        }
    }

    public struct Tuple6D<T>
    {
        public T a, b, c, d, e, f;

        public Tuple6D(T A, T B, T C, T D, T E, T F)
        {
            this.a = A;
            this.b = B;
            this.c = C;
            this.d = D;
            this.e = E;
            this.f = F;
        }

        public List<T> flatten()
        {
            return new List<T> { a, b, c, d, e, f };
        }

        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var left = flatten();
            left.Sort();
            var right = ((Tuple6D<T>)obj).flatten();
            right.Sort();

            return left.SequenceEqual(right);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return new HashSet<T> { a, b, c, d, e, f }.GetHashCode();
        }
    }
}