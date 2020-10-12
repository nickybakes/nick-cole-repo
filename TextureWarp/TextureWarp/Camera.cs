using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TextureWarp
{
    class Camera : Transform
    {
        public float fov = 90;
        public float nearZ = .01f;
        public float farZ = 100;
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
                return new Vector3(pos.X, pos.Y, 1 / (float)(Math.Tan(MathHelper.ToRadians(fov) / 2)));
                //return new Vector3(pos.X - (float)(Math.Sin(MathHelper.ToRadians(eulerAngles.X))*fov), pos.Y + (float)(Math.Cos(MathHelper.ToRadians(eulerAngles.X)) * fov), 1 / (float)(Math.Tan(MathHelper.ToRadians(fov)/2)));
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
            
            rot.X += MathHelper.ToRadians(degrees);
            while (rot.X < 0)
            {
                rot.X += MathHelper.ToRadians(360);
            }
            while (rot.X >= MathHelper.ToRadians(360))
            {
                rot.X -= MathHelper.ToRadians(360);
            }
        }

        public void RotateYaw(float degrees)
        {
            rot.Y += MathHelper.ToRadians(degrees);
            while (rot.Y < 0)
            {
                rot.Y += MathHelper.ToRadians(360);
            }
            while (rot.Y >= MathHelper.ToRadians(360))
            {
                rot.Y -= MathHelper.ToRadians(360);
            }
        }

        public void RotateRoll(float degrees)
        {
            rot.Z += MathHelper.ToRadians(degrees);
            while (rot.Z < 0)
            {
                rot.Z += MathHelper.ToRadians(360);
            }
            while (rot.Z >= MathHelper.ToRadians(360))
            {
                rot.Z -= MathHelper.ToRadians(360);
            }
        }

        public void MoveForward(float amount)
        {
            Vector3 movement = rot.Length() * Vector3.Forward;
            pos.Z += amount;
        }

        public void MoveSide(float amount)
        {
            pos.X += amount;
        }

        public void MoveUp(float amount)
        {
            pos.Y += amount;
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

        public Vector2 NickPerspectiveProjection(Vector3 worldPoint)
        {
            //move point so that it is orientated about the origin 0,0,0
            Vector3 pointInCameraSpace = worldPoint - pos;

            //get rotating matrices ready (we negate them because we rotate everying around the camera
            //the opposite direction that the camera is currently rotated in to give the illusion of rotation)
            Matrix yawRotMatrix = Matrix.CreateRotationY(-rot.Y);
            Matrix pitchRotMatrix = Matrix.CreateRotationX(-rot.X);
            Matrix rollRotMatrix = Matrix.CreateRotationZ(-rot.Z);

            //rotate everything around our camera
            Vector3 pointRotatedInCameraSpace = Vector3.Transform(pointInCameraSpace, yawRotMatrix);
            pointRotatedInCameraSpace = Vector3.Transform(pointRotatedInCameraSpace, pitchRotMatrix);
            pointRotatedInCameraSpace = Vector3.Transform(pointRotatedInCameraSpace, rollRotMatrix);

            //finding where the line is when Z is a certain amount in front of our camera.
            //(finding where the line intersects the camera plane)
            float f = ScreenPoint.Z;
            float s = f / pointRotatedInCameraSpace.Z;
            float x3d = pointRotatedInCameraSpace.X * s;
            float y3d = pointRotatedInCameraSpace.Y * s;

            //convert from 3d camera space to 2d screen space, adjusting for depth, aspect ratio, and fov
            float aspectRatio = (float)Game1.resWidth / Game1.resHeight;
            float x2d = x3d - (f / pointRotatedInCameraSpace.Z/2) + .5f;
            float y2d = (y3d * aspectRatio) - (f / pointRotatedInCameraSpace.Z/2) + .5f;

            //if something is behind the camera, move it off screen
            if (pointRotatedInCameraSpace.Z <= 0)
            {
                if(x2d >= 0)
                    x2d = -MathHelper.Clamp(x2d, .5f, 2048);
                else
                    x2d = -MathHelper.Clamp(x2d, 2048, -.5f);
                if (y2d >= 0)
                    y2d = -MathHelper.Clamp(y2d, .5f, 2048);
                else
                    y2d = -MathHelper.Clamp(y2d, 2048, -.5f);
                //x2d *= (float)(Math.Sign(x2d) * (.5) - (1 + Math.Abs(pointRotatedInCameraSpace.Z)));
                //y2d *= -(1 + Math.Abs(pointRotatedInCameraSpace.Z));
            }

            return new Vector2(Game1.resWidth * x2d, Game1.resHeight * y2d);
        }


        public Vector2 MatrixPerspectiveProjection(Vector3 point)
        {
            Matrix lookAtMatrix = Matrix.CreateLookAt(pos, new Vector3(rot.Y, rot.X, rot.Z), Vector3.Up);
            Matrix persp = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), (float)Game1.resWidth / Game1.resHeight, nearZ, farZ);

            //Vector4 d = new Vector4(point, 1) - lookAtMatrix * persp;

            Vector3 d = Vector3.Transform(point, lookAtMatrix * persp);
            //Vector3 ndc = new Vector3(d.X / d.W, d.Y / d.W, d.Z / d.W);

            //Matrix screenSpace = Matrix.Invert(Matrix.CreateOrthographicOffCenter(0, Game1.resWidth, Game1.resHeight, 0, -1, 1));
            //return Vector2.Transform(new Vector2(d.X, d.Y), screenSpace);
            //float x = d.X * Game1.resWidth;
            //float y = d.Y * Game1.resHeight;

            float x = Game1.resWidth * d.X + (Game1.resWidth / 2);
            float y = -Game1.resHeight * d.Y + (Game1.resHeight / 2);
            return new Vector2(x, y);
        }

        public Vector2 PerspectiveProjection(Vector3 point)
        {
            Vector3 a = point;
            Vector3 c = pos;
            //Vector3 theta = Game1.ToEulerAngles(Game1.ToQuaternion(rot.Z, rot.Y, rot.X));
            Vector3 theta = rot;
            Vector3 e = ScreenPoint;

            Vector3 d = new Vector3();
            d.X = (float)(Math.Cos(theta.Y) * (Math.Sin(theta.Z)*(a.Y - c.Y) + Math.Cos(theta.Z)*(a.X-c.X)) - Math.Sin(theta.Y)*(a.Z - c.Z));

            d.Y = (float)(Math.Sin(theta.X) * (Math.Cos(theta.Y)*(a.Z - c.Z) + Math.Sin(theta.Y) * (Math.Sin(theta.Z)*(a.Y-c.Y)+Math.Cos(theta.Z) *(a.X-c.X))) + Math.Cos(theta.X)*(Math.Cos(theta.Z)*(a.Y-c.Y)-Math.Sin(theta.Z)*(a.X-c.X)));

            d.Z = (float)(Math.Cos(theta.X) * (Math.Cos(theta.Y) * (a.Z - c.Z) + Math.Sin(theta.Y) * (Math.Sin(theta.Z) * (a.Y - c.Y) + Math.Cos(theta.Z) * (a.X - c.X))) - Math.Sin(theta.X) * (Math.Cos(theta.Z) * (a.Y - c.Y) - Math.Sin(theta.Z) * (a.X - c.X)));

            Vector2 b = new Vector2();
            b.X = ((d.X - e.X) * (e.Z / d.Z)) +.5f;
            b.Y = ((d.Y - e.Y) * (e.Z / d.Z)) +.5f;

            return new Vector2(Game1.resWidth * b.X, -Game1.resHeight * b.Y);
        }

        

        public void RenderScene(SpriteBatch spriteBatch, List<Transform> objects)
        {

        }

        
    }
}
