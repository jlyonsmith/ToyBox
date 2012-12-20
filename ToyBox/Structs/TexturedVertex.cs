using System;
using System.Runtime.InteropServices;

namespace ToyBox
{
    [StructLayout(LayoutKind.Sequential)]
    struct TexturedVertex
    {
        public TexturedVertex(int x, int y, int z, int r, int g, int b, int a, int s, int t)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
            S = (float)s;
            T = (float)t;
        }
        
        public float X;
        public float Y;
        public float Z;
        public float S;
        public float T;
    }
}

