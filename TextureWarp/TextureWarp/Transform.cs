using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TextureWarp
{
    class Transform
    {
        public Vector3 pos;
        public Vector3 rot;

        public Vector3 axisX
        {
            get
            {
                return new Vector3(1, 0, 0);
            }
        }
    }
}
