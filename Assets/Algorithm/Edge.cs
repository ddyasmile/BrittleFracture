using System.Collections;
using System.Collections.Generic;

namespace EdgeNS
{
    public class Edge
    {
        public int from, to;

        public Edge(int From, int To)
        {
            this.from = From;
            this.to = To;
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

            var another = (Edge)obj;
            if (this.from == another.from && this.to == another.to)
            {
                return true;
            }
            else if (this.from == another.to && this.to == another.from)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return new HashSet<int> { from, to }.GetHashCode();
        }
    }
}
