using Intersection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vr_Gl.Model;
using OpenTK.Input;
using Vr_Gl.Simulation;
using Triangulation;
using Vr_Gl.Properties;

namespace Vr_Gl.Graphics
{
    public class MainWindow : GameWindow
    {
        double x = 0, y = 0, z =-10;
        double rotx = 60, roty = 0, rotz = 0;
        bool clicked = false;
        Triangulation.Vector3 move = new Triangulation.Vector3(0, 0, 0);
        public Cutter Cutter { get; set; }
        public Cutted Cutted { get; set; }

        private Table _table;
        private Room _room;
        private Light _light;
        public MainWindow(string cutterFileName, string cuttedFileName)
        {
            Cutter = new Cutter(cutterFileName);
            Cutted = new Cutted(cuttedFileName, Cutter, new Triangulation.Vector3(-2, 0, -1));
        }

        public MainWindow(List<Triangle> cutterTris, List<Triangle> cuttedTris)
        {
            Cutter = new Cutter(cutterTris, @"D:\University\4th-AI\VR\VR-project\VR-Project\VR-Project\Resources\Metal3.bmp");
            Cutted = new Cutted(cuttedTris, Cutter, new Triangulation.Vector3(-3, 0, -1.5), @"D:\University\4th-AI\VR\VR-project\VR-Project\VR-Project\Resources\Base.bmp");
            _room=new Room();
            _table=new Table();
            _light=new Light();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Test";
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(x, y, z);
            GL.Rotate(rotx, 1, 0, 0);
            GL.Rotate(roty, 0, 1, 0);
            GL.Rotate(rotz, 0, 0, 1);
            if (clicked)
            {
                Cutter.Move(move);
                Cutted.Update();
            }
            _light.Activate();
            CustomMaterial.Ruby.Activate();

            _room.Draw();
            _table.Draw();
            GL.Translate(2,-1,1);
            GL.Rotate(-90,1,0,0);

            //_light.updatePosition();
            ////Cutter.Draw(new Triangulation.Vector3(1, 1, 1));
            Cutter.Draw();
            ////Cutted.Draw(new Triangulation.Vector3(0.2, 0.6, 0.4));
            Cutted.Draw();
           

            SwapBuffers();
            clicked = false;
            move = new Triangulation.Vector3(0, 0, 0);
        }

        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);

            GL.MatrixMode(MatrixMode.Projection);

            GL.LoadMatrix(ref projection);
        }
        
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            string key = e.Key.ToString().ToLower();
            float delta = 0.15f;
            switch (key)
            {
                case "k":
                    z -= 1;
                    break;
                case "i":
                    z += 1;
                    break;
                case "j":
                    x -= 1;
                    break;
                case "l":
                    x += 1;
                    break;
                case ",":
                    y += 1;
                    break;
                case "m":
                    y -= 1;
                    break;
                case "o":
                    rotx += 2;
                    break;
                case "u":
                    rotx -= 2;
                    break;
                case "p":
                    roty += 2;
                    break;
                case "[":
                    rotz += 2;
                    break;
                case "y":
                    roty -= 2;
                    break;
                case "t":
                    rotz -= 2;
                    break;
                case "w":
                    move.Z = move.Z - delta * 2;
                    clicked = true;
                    break;
                case "s":
                    move.Z = move.Z + delta * 2;
                    clicked = true;
                    break;
                case "e":
                    move.Y = move.Y + delta;
                    clicked = true;
                    break;
                case "q":
                    move.Y = move.Y - delta;
                    clicked = true;
                    break;
                case "a":
                    move.X = move.X - delta;
                    clicked = true;
                    break;
                case "d":
                    move.X = move.X + delta;
                    clicked = true;
                    break;
            }
        }
    }
}
