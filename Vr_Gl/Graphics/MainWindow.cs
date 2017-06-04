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

namespace Vr_Gl.Graphics
{
    class MainWindow : GameWindow
    {
        double z = 0;
        bool clicked = false;
        Vector3m move = new Vector3m(0, 0, 0);
        public Cutter Cutter { get; set; }
        public Cutted Cutted { get; set; }
        public MainWindow(string cutterFileName, string cuttedFileName)
        {
            Cutter = new Cutter(cutterFileName);
            Cutted = new Cutted(cuttedFileName, Cutter, new Vector3m(-5, 0, -5));
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Test";
            GL.ClearColor(0, 0, 0, 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(0, 0, z);
            if (clicked)
            {
                Cutter.Move(move);
                Cutted.Update();
            }
            Cutter.Draw();
            Cutted.Draw();
            SwapBuffers();
            clicked = false;
            move = new Vector3m(0, 0, 0);
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
            float delta = 0.5f;
            switch (key)
            {
                case "k":
                    z -= 1;
                    break;
                case "i":
                    z += 1;
                    break;
                case "w":
                    move.Z = move.Z - delta;
                    clicked = true;
                    break;
                case "s":
                    move.Z = move.Z + delta;
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
