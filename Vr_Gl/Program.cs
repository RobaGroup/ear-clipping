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

namespace Vr_Gl
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow("D:/nokia.obj", "D:/ipad.obj"))
            {
                window.Run(30);
            }
        }
    }
}
