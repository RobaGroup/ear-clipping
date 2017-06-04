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
        public Vector3m V1 { private set; get; }
        public Vector3m V2 { private set; get; }

        public double Length { get { return (V2 - V1).Length(); } }

        public Segment(Vector3m v1, Vector3m v2)
        {
            V1 = v1;
            V2 = v2;
        }
    }
}
