using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Vr_Gl;
using Vr_Gl.Graphics;
using Vr_Gl.Properties;

namespace Vr_Gl
{
     class Room : DrawingObject
    {
        int floor;
         public Room()
         {

             //Texture = VR_Project.Texture.GiveMeID(Resources.Room);
             //GL.Enable(EnableCap.Texture2D);

             //GL.BindTexture(TextureTarget.ProxyTexture2D, Texture);

             Texture = AssetsLoader.LoadTexture(Resources.Room);
             Material = new Material(0.1745f, 0.01175f, 0.01175f, 0.61424f, 0.04136f, 0.04136f, 0.727811f,
            0.626959f, 0.626959f, 0.6f);
             floor= AssetsLoader.LoadTexture(Resources.WoodenFloor);
         }
         public override void Draw()
         {
             Material.Activate();
            GL.PushMatrix();
            GL.Scale(10f, 8f,10f);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.Begin(BeginMode.Quads);
            // Front Face
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            // Back Face
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            // Top Face
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            // Right face
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            // Left Face
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, floor);
            GL.Begin(BeginMode.Quads);
                        // Bottom Face
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.End();
             GL.PopMatrix();
            // GL.ActiveTexture(TextureUnit.Texture0);

            //GL.Enable(EnableCap.Texture2D);
            //GL.PushMatrix();
            // Material.Activate();

            ////GL.LoadIdentity();

            ////GL.Color3(Color.Aqua);

            // GL.BindTexture(TextureTarget.ProxyTexture2D, Texture);
            //GL.Translate(0.0f, 0.0f, -0.0f);
            //GL.Rotate(180,0,0,1);
            //GL.Scale(8f, .8f, 8f);

            //GL.Begin(mode: BeginMode.Quads);
            //// Front Face
            //GL.Normal3(0.0f, 0.0f, 1.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            //// Back Face
            //GL.Normal3(0.0f, 0.0f, -1.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            //// Top Face
            //GL.Normal3(0.0f, 1.0f, 0.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            //// Bottom Face
            //GL.Normal3(0.0f, -1.0f, 0.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            //// Right face
            //GL.Normal3(1.0f, 0.0f, 0.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            //// Left Face
            //GL.Normal3(-1.0f, 0.0f, 0.0f);
            //GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            //GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
            //GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);
            //GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);
            //GL.End();
            //GL.PopMatrix();
            //GL.Disable(EnableCap.Texture2D);
        }
    }
}
