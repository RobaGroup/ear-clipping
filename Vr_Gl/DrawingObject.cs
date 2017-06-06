using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Vr_Gl
{
    abstract class DrawingObject
    {
        protected int Texture;
        protected Material Material;
        protected Vector3[] Vertdata = new Vector3[] { new Vector3(-1f, -1f,  -1f),
                new Vector3(-1f, -1f,  1f),
                new Vector3(1f, -1f,  1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, 1f,  1f),

                
                new Vector3(-1f, -1f,  -1f),
                new Vector3(-1f, 1f,  -1f),

                new Vector3(1f, -1f,  -1f),
                new Vector3(1f, 1f,  -1f),
                new Vector3(-1f, 1f,  -1f),
                new Vector3(-1f, -1f,  1f),
                new Vector3(1f, -1f,  1f),
                new Vector3(1f, 1f,  1f),
                new Vector3(-1f, 1f,  1f)
            };

        public abstract void Draw()

            //GL.PushMatrix();
            //GL.Enable(EnableCap.Texture2D);
            //// _material.Activate();
            //GL.LoadIdentity();

            ////GL.Color3(Color.Aqua);

            //GL.Translate(0.0f, 0.0f, -6.0f);
            //GL.Rotate(180, 0, 0, 1);
            //GL.Scale(1, .8f, 1);
            //GL.BindTexture(TextureTarget.ProxyTexture2D, Texture);
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
            //GL.Disable(EnableCap.Texture2D);
            //GL.PopMatrix();
            ;
    }
}
