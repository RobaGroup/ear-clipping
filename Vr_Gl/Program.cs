using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using Intersection;
using Triangulation;
using Vr_Gl.Model;
using Tree;
using Vr_Gl.Graphics;
using Vr_Gl.Simulation;
//using g3;

namespace Vr_Gl
{
    class Program
    {
        static void Main(string[] args)
        {
            //Triangle3f tri1 = new Triangle3f(new Vector3f(0, 0, 0), new Vector3f(1, 0, 0), new Vector3f(0.5, 0.5, 0));
            //Triangle3f tri2 = new Triangle3f(new Vector3f(0, 0.25, -0.2), new Vector3f(0.5, 0.5, 1), new Vector3f(1, 0.25, -0.2));
            //var inter = new IntrTriangle3Triangle3(tri1, tri2);
            //inter = inter.Compute();
            //if(inter.Result == IntersectionResult.Intersects && inter.Type == IntersectionType.Segment)
            //{
            //    Console.WriteLine(inter.Points[0].ToString());
            //    Console.WriteLine(inter.Points[1].ToString());
            //}
            //Triangle tri = new Triangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.5, 0.5, 0));
            //List<Triangle> tris = new Triangle[] {new Triangle(new Vector3(0, 0.25, -0.2), new Vector3(0.5, 0.5, 1), new Vector3(1, 0.25, -0.2))}.ToList();
            //var res = IntersectionDetector.Test(tri, tris);
            //Console.WriteLine(res[0].Item1);
            //Console.ReadKey();
            //using (MainWindow window = new MainWindow(@"D:/pyramid.3ds", @"D:/cube.obj"))
            //{
            //    window.Run(30);
            //}
            //List<Triangle> cutterTris = new Loader(10).Load(@"D:/pyramid.3ds");
            //EarClipping clip = new EarClipping();
            //List<Vector3> vertices = new Vector3[] { new Vector3(0, 0, 0) , new Vector3(1, 0, 0) , new Vector3(0.5, 0.5, 0) }.ToList();
            //List<Vector3> temp = new Vector3[] {new Vector3(0, 0.2, 0), new Vector3(0.5, 2, 0), new Vector3(1, 0.2, 0)}.ToList();
            //List<List<Vector3>> holes = new List<List<Vector3>>();
            //holes.Add(temp);
            //clip.SetPoints(vertices, holes);
            //clip.Triangulate();
            //var res = clip.Result;
            //for (int i = 0; i < res.Count - 2; i += 3)
            //{
            //    Console.WriteLine(res[i].ToString());
            //    Console.WriteLine(res[i + 1].ToString());
            //    Console.WriteLine(res[i + 2].ToString());
            //}
            //Console.ReadKey();
            List<Triangle> cutterTris = new Triangle[]
            {
                new Triangle(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(0, 0, 0), new Vector3(0, 0.6, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(1, 0.6, 0), new Vector3(0.3, 0.3, -0.5)),
                new Triangle(new Vector3(0, 0.6, 0), new Vector3(1, 0.6, 0), new Vector3(0.3, 0.3, -0.5)),
            }.ToList();
            List<Triangle> cuttedTris = new Triangle[]
            {
                new Triangle(new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(1, 3, 0))
                //new Triangle(new Vector3(0, 0, 0), new Vector3(0, 0, -2), new Vector3(1, 3, -1)),
                //new Triangle(new Vector3(2, 0, 0), new Vector3(2, 0, -2), new Vector3(1, 3, -1)),
                //new Triangle(new Vector3(0, 0, -2), new Vector3(2, 0, -2), new Vector3(1, 3, -1)),
                //new Triangle(new Vector3(0, 0, 0), new Vector3(2, 0, 0), new Vector3(0, 0, -2)),
                //new Triangle(new Vector3(2, 0, 0), new Vector3(2, 0, -2), new Vector3(0, 0, -2)),
            }.ToList();
            using (MainWindow window = new MainWindow(cutterTris, cuttedTris))
            {
                window.Run(30);
            }

        }
    }
}
