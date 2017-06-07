using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Vr_Gl
{
    class Light
    {
        float[] Ambient = { 0.45f, 0.45f, 0.45f, 1.0f };//{ 0.5f, 0.5f, 0.5f, 1.0f };
        float []Diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float []Position = {1.0f, 1.0f, -0.5f};//{ 0.0f, 4.0f, 2.0f, 1.0f };//{ 0.0f, 0.0f, 2.0f, 1.0f };

        private bool state = true;

        public Light()
        {
            //Enable Light 0 and set its parameters.
            GL.Light(LightName.Light0, LightParameter.Position, Position);
            GL.Light(LightName.Light0, LightParameter.Ambient, Ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, Diffuse);
            //GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Light(LightName.Light0, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelColorControl, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);

        }

        public void Activate()
        {
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Lighting);
            state = true;
        }

        public void DeActivate()
        {
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Lighting);
            state = false;
        }

        public void ChangeState()
        {
            if(state)
                DeActivate();
            else
                Activate();
            
        }
    }
    
}
