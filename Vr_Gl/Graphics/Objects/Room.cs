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
         public Room()
         {

             //Texture = VR_Project.Texture.GiveMeID(Resources.Room);
             //GL.Enable(EnableCap.Texture2D);

             //GL.BindTexture(TextureTarget.ProxyTexture2D, Texture);

             Texture = AssetsLoader.LoadTexture(@"D:\University\4th-AI\VR\VR-project\VR-Project\VR-Project\Resources\Back1.bmp");
             Material = CustomMaterial.Ruby;
         }
         public override void Draw()
        {				
	         GL.ActiveTexture(TextureUnit.Texture0);

            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();
             Material.Activate();

            //GL.LoadIdentity();

            //GL.Color3(Color.Aqua);

             GL.BindTexture(TextureTarget.ProxyTexture2D, Texture);
            GL.Translate(0.0f, 0.0f, -0.0f);
            GL.Rotate(180,0,0,1);
            GL.Scale(8f, .8f, 8f);

            GL.Begin(mode: BeginMode.Quads);
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
            // Bottom Face
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);
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
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
