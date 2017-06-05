using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace Intersection
{
     public class Segment
    {
        public Vector3 V1 { private set; get; }
        public Vector3 V2 { private set; get; }

        public double Length { get { return (V2 - V1).Length(); } }

        public Segment(Vector3 v1, Vector3 v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public bool IsParallel(Segment other)
        {
            return Math.Abs(Math.Cos((this.V1 - this.V2).Dot(other.V1 - other.V2) / ((this.V1 - this.V2).Length() * (other.V1 - other.V2).Length()))) <= 0.001;
        }
    }
}
