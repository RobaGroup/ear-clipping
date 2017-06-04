using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;
using Intersection;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Vr_Gl.Tree;

namespace Tree
{
    public class AabbTree
    {

        static int LeafSize = 1;
        private Node _root;
        public Node Root { get { return _root; } set { _root = value; } }
        public List<Triangle> Triangles { get; set; }
        public AabbTree(List<Triangle> triangles)
        {    
            Triangles = triangles;
            ParallelSort.QuicksortParallel(Triangles);
            _create(ref _root, 0, Triangles.Count - 1);
        }

        public AabbTree(List<Vector3m> points)
        {
            Triangles = new List<Triangle>();
            for (int i = 0; i < points.Count - 2; i += 3)
            {
                Triangles.Add(new Triangle(points[i], points[i + 1], points[i + 2]));
            }
            ParallelSort.QuicksortParallel(Triangles);
            _create(ref _root, 0, Triangles.Count - 1);
        }

        public List<List<Triangle>> IntersectTriangles(List<Triangle> tris)
        {
            List<List<Triangle>> res = new List<List<Triangle>>();
            foreach (var tri in tris)
            {
                List<Triangle> temp = new List<Triangle>();
                Root.Intersect(tri, ref temp);
                res.Add(temp);
            }
            return res;
        }

        private void _create(ref Node node, int leftIndex, int rightIndex)
        {
            node = new Node(leftIndex, rightIndex, null, null, this);
            if ((rightIndex - leftIndex + 1) == LeafSize)
                return;
            int len = (rightIndex - leftIndex);
            _create(ref node._left, leftIndex, leftIndex + len / 2);
            _create(ref node._right, leftIndex + len / 2 + 1, rightIndex);
        }
        
        public void Move(Vector3m trans)
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                Triangles[i].Move(trans);
            }
            Root.Move(trans);
        }
    }
}
