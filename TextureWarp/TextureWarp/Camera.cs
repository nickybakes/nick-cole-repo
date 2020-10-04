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
    class Camera : Transform
    {
        public float fov = 45;
        public float clipWidth = 1600;
        public float clipHeight = 900;

        public Vector3 ScreenPoint
        {
            get
            {
                return new Vector3(fov, 0, 0);
            }
        }

        public Vector3 Normal
        {
            get
            {
                Vector3 normal = ScreenPoint;
                normal.Normalize();
                return normal;
            }
        }


        public Vector2 Project3DPointToScreen(Vector3 point)
        {
            Vector3 origin = ScreenPoint;
            Vector3 v = point - origin;
            origin.Normalize();
            float dist = v.X * origin.X + v.Y * origin.Y + v.Y * origin.Y;
            Vector3 projectedPoint = point - dist * origin;
            float x = Vector3.Dot(point - ScreenPoint, new Vector3(0, 0, 1));
            float y = Vector3.Dot(point - ScreenPoint, Vector3.Cross(Normal, new Vector3(0, 0, 1)));
            return new Vector2(x, y);
        }

        

        public void RenderScene(SpriteBatch spriteBatch, List<Transform> objects)
        {

        }
    }
}
