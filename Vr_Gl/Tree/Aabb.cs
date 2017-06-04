using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace Tree
{
    public class Aabb
    {
        double MinX { get; set; }
        double MinY { get; set; }
        double MinZ { get; set; }
        double MaxX { get; set; }
        double MaxY { get; set; }
        double MaxZ { get; set; }

        public bool Intersect(Aabb b)
        {
            return (MinX <= b.MaxX && MaxX >= b.MinX) &&
                   (MinY <= b.MaxY && MaxY >= b.MinY) &&
                   (MinZ <= b.MaxZ && MaxZ >= b.MinZ);
        }

        public Aabb(List<Vector3> vertices)
        {
            MinX = MaxX = vertices[0].X;
            MinY = MaxY = vertices[0].Y;
            MinZ = MaxY = vertices[0].Z;
            foreach (var item in vertices)
            {
                MinX = Math.Min(MinX, item.X);
                MaxX = Math.Max(MaxX, item.X);
                MinY = Math.Min(MinY, item.Y);
                MaxY = Math.Max(MaxY, item.Y);
                MinZ = Math.Min(MinZ, item.Z);
                MaxZ = Math.Max(MaxZ, item.Z);
            }
        }

        public Aabb(List<Triangle> tris)
        {
            MinX = MaxX = tris[0].V1.X;
            MinY = MaxY = tris[0].V1.Y;
            MinZ = MaxY = tris[0].V1.Z;
            foreach (var tri in tris)
            {
                MinX = Math.Min(MinX, tri.V1.X);
                MaxX = Math.Max(MaxX, tri.V1.X);
                MinX = Math.Min(MinX, tri.V2.X);
                MaxX = Math.Max(MaxX, tri.V2.X);
                MinX = Math.Min(MinX, tri.V3.X);
                MaxX = Math.Max(MaxX, tri.V3.X);


                MinY = Math.Min(MinY, tri.V1.Y);
                MaxY = Math.Max(MaxY, tri.V1.Y);
                MinY = Math.Min(MinY, tri.V2.Y);
                MaxY = Math.Max(MaxY, tri.V2.Y);
                MinY = Math.Min(MinY, tri.V3.Y);
                MaxY = Math.Max(MaxY, tri.V3.Y);

                MinZ = Math.Min(MinZ, tri.V1.Z);
                MaxZ = Math.Max(MaxZ, tri.V1.Z);
                MinZ = Math.Min(MinZ, tri.V2.Z);
                MaxZ = Math.Max(MaxZ, tri.V2.Z);
                MinZ = Math.Min(MinZ, tri.V3.Z);
                MaxZ = Math.Max(MaxZ, tri.V3.Z);
            }
        }

        public Aabb(Triangle tri)
        {
            MinX = MinY = MinZ = Double.MaxValue;
            MaxX = MaxY = MaxZ = Double.MinValue;
            MinX = Math.Min(MinX, tri.V1.X);
            MaxX = Math.Max(MaxX, tri.V1.X);
            MinX = Math.Min(MinX, tri.V2.X);
            MaxX = Math.Max(MaxX, tri.V2.X);
            MinX = Math.Min(MinX, tri.V3.X);
            MaxX = Math.Max(MaxX, tri.V3.X);


            MinY = Math.Min(MinY, tri.V1.Y);
            MaxY = Math.Max(MaxY, tri.V1.Y);
            MinY = Math.Min(MinY, tri.V2.Y);
            MaxY = Math.Max(MaxY, tri.V2.Y);
            MinY = Math.Min(MinY, tri.V3.Y);
            MaxY = Math.Max(MaxY, tri.V3.Y);

            MinZ = Math.Min(MinZ, tri.V1.Z);
            MaxZ = Math.Max(MaxZ, tri.V1.Z);
            MinZ = Math.Min(MinZ, tri.V2.Z);
            MaxZ = Math.Max(MaxZ, tri.V2.Z);
            MinZ = Math.Min(MinZ, tri.V3.Z);
            MaxZ = Math.Max(MaxZ, tri.V3.Z);
        }

        public void Move(Vector3 trans)
        {
            this.MinX += trans.X;
            this.MaxX += trans.X;
            this.MinY += trans.Y;
            this.MaxY += trans.Y;
            this.MinZ += trans.Z;
            this.MaxZ += trans.Z;
        }
    }
}
