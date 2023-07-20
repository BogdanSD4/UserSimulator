using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EyesSimulator.PhotoEditor.Settings
{
    public class Photo
    {
        public Photo(string data, Box vector)
        {
            Data = data;
            Box = vector;
            Extansion = ".png";
        }
        public Photo(string data, Box vector, string extansion)
        {
            Data = data;
            Box = vector;
            Extansion = extansion;
        }

        public string Name { get; set; }
        public string Extansion { get; set; }
        public string Data { get; set; }
        public Box Box { get; set; }
    }
}
