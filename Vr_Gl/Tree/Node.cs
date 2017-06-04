using Intersection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triangulation;

namespace Tree
{
    public class Node
    {
        public Aabb Aabb { get; set; }
        public Node _left;
        public Node _right;
        public int LeftIndex { get; set; }
        public int RightIndex { get; set; }
        public AabbTree Tree { get; set; }
        public Node(int left, int right, Node l, Node r, AabbTree tree)
        {
            this._left = l;
            this._right = r;
            this.LeftIndex = left;
            this.RightIndex = right;
            this.Tree = tree;
            this.Aabb = new Aabb(tree.Triangles.GetRange(left, (right - left + 1)));
        }

        public void Intersect(Triangle tri, ref List<Triangle> tris)
        {
            Aabb aabb = new Aabb(tri);
            Intersect(aabb, ref tris);
        }

        public void Intersect(Aabb aabb, ref List<Triangle> tris)
        {
            if (!this.Aabb.Intersect(aabb))
                return;
            if(_left == null && _right == null)
            {
                for (int i = LeftIndex; i <= RightIndex; ++i)
                {
                    tris.Add(Tree.Triangles[i]);
                }
                return;
            }
            if (_left != null)
            {
                _left.Intersect(aabb, ref tris);
            }
            if (_right != null)
            {
                _right.Intersect(aabb, ref tris);
            }
        }

        public void Move(Vector3m trans)
        {
            this.Aabb.Move(trans);
            if (_left != null)
                _left.Move(trans);
            if (_right != null)
                _right.Move(trans);
        }
    }
}
