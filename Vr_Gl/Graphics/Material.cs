using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Vr_Gl
{
    class Material
    {
        float ambient_r;
        float ambient_g;
        float ambient_b;

        float diffuse_r;
        float diffuse_g;
        float diffuse_b;

        float specular_r;
        float specular_g;
        float specular_b;

        float shiness = 128.0f;

        public Material(float ar, float ag, float ab, float dr, float dg, float db, float sr, float sg, float sb, float s)
        {
            ambient_r = ar; ambient_g = ag; ambient_b = ab;
            diffuse_r = dr; diffuse_g = dg; diffuse_b = db;
            specular_r = sr; specular_g = sg; specular_b = sb;
            shiness *= s;
        }

        public void Activate()
        {
            GL.Enable(EnableCap.ColorMaterial);
		    float []ambient = { ambient_r, ambient_g, ambient_b, 1.0f };
            GL.GetMaterial(MaterialFace.FrontAndBack, MaterialParameter.Ambient, ambient);
		    float []diffuse = { diffuse_r, diffuse_g, diffuse_b };
            GL.GetMaterial(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, diffuse);
		    float []specular = { specular_r, specular_g, specular_b };
            GL.GetMaterial(MaterialFace.FrontAndBack, MaterialParameter.Specular, specular);
		    float []shines = { shiness };
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, shines);
            GL.Enable(EnableCap.Lighting);

	    }
    }
}
