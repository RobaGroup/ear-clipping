using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vr_Gl
{
    static class CustomMaterial
    {
        public static Material Obsidian = new Material(0.05375f, 0.05f, 0.06625f, 0.18275f, 0.17f, 0.22525f, 0.332741f,
            0.328634f, 0.346435f, 0.3f);

        public static Material Brass = new Material(0.329412f, 0.223529f, 0.027451f, 0.780392f, 0.568627f, 0.113725f,
            0.992157f, 0.941176f, 0.807843f, 0.21794872f);

        public static Material Chrome = new Material(0.25f, 0.25f, 0.25f, 0.4f, 0.4f, 0.4f, 0.774597f, 0.774597f,
            0.774597f, 0.6f);

        public static Material Bronze = new Material(0.2125f, 0.1275f, 0.054f, 0.714f, 0.4284f, 0.18144f, 0.393548f,
            0.271906f, 0.166721f, 0.2f);

        public static Material Copper = new Material(0.19125f, 0.0735f, 0.0225f, 0.7038f, 0.27048f, 0.0828f, 0.256777f,
            0.137622f, 0.086014f, 0.1f);

        public static Material Ruby = new Material(0.1745f, 0.01175f, 0.01175f, 0.61424f, 0.04136f, 0.04136f, 0.727811f,
            0.626959f, 0.626959f, 0.6f);

    }
}
