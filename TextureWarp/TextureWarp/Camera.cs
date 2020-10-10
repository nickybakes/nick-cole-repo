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

        public Camera()
        {
            //rot = Game1.ToQuaternion(MathHelper.ToRadians(90), MathHelper.ToRadians(2), MathHelper.ToRadians(50));
        }

        public Vector3 ScreenPoint
        {
            get
            {
                return new Vector3(pos.X, pos.Y, pos.Z + 1 / (float)(Math.Tan(MathHelper.ToRadians(fov)/2)));
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

        public void RotatePitch(float degrees)
        {
            Vector3 e = Game1.ToEulerAngles(rot);
            e.X += MathHelper.ToRadians(degrees);
            rot = Game1.ToQuaternion(e.Z, e.Y, e.X);
        }

        public void RotateYaw(float degrees)
        {
            Vector3 e = Game1.ToEulerAngles(rot);
            e.Y += MathHelper.ToRadians(degrees);
            rot = Game1.ToQuaternion(e.Z, e.Y, e.X);
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

        public Vector2 PerspectiveProjection(Vector3 point)
        {
            Vector3 a = point;
            Vector3 c = pos;
            Vector3 theta = Game1.ToEulerAngles(rot);
            Vector3 e = ScreenPoint;

            Vector3 d = new Vector3();
            d.X = (float)(Math.Cos(theta.Y) * (Math.Sin(theta.Z)*(a.Y - c.Y) + Math.Cos(theta.Z)*(a.X-c.X)) - Math.Sin(theta.Y)*(a.Z - c.Z));

            d.Y = (float)(Math.Sin(theta.X) * (Math.Cos(theta.Y)*(a.Z - c.Z) + Math.Sin(theta.Y) * (Math.Sin(theta.Z)*(a.Y-c.Y)+Math.Cos(theta.Z) *(a.X-c.X))) + Math.Cos(theta.X)*(Math.Cos(theta.Z)*(a.Y-c.Y)-Math.Sin(theta.Z)*(a.X-c.X)));

            d.Z = (float)(Math.Cos(theta.X) * (Math.Cos(theta.Y) * (a.Z - c.Z) + Math.Sin(theta.Y) * (Math.Sin(theta.Z) * (a.Y - c.Y) + Math.Cos(theta.Z) * (a.X - c.X))) - Math.Sin(theta.X) * (Math.Cos(theta.Z) * (a.Y - c.Y) - Math.Sin(theta.Z) * (a.X - c.X)));

            Vector2 b = new Vector2();

            b.X = (d.X + .5f) * (e.Z / d.Z);
            b.Y = (d.Y + .5f) * (e.Z / d.Z);

            return new Vector2(1920 * b.X, 1080 * b.Y);
        }

        

        public void RenderScene(SpriteBatch spriteBatch, List<Transform> objects)
        {

        }

        
    }
}
