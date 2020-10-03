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
    class Quad : Transform
    {
        public Vector2[] verts;
        public Texture2D quadTexture;
        public Color[,] textureData;

        public Quad(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight, Texture2D quadTexture)
        {
            verts = new[] { topLeft, topRight, bottomLeft, bottomRight };
            this.quadTexture = quadTexture;
            Color[] textureDataRaw = new Color[this.quadTexture.Width * this.quadTexture.Height];
            quadTexture.GetData<Color>(textureDataRaw);

            textureData = new Color[quadTexture.Width, quadTexture.Height];
            for (int y = 0; y < quadTexture.Height; y++)
            {
                for (int x = 0; x < quadTexture.Width; x++)
                {
                    // Assumes row major ordering of the array.
                    textureData[x, y] = textureDataRaw[y * quadTexture.Height + x];
                }
            }
        }

        public void DrawVerts(SpriteBatch spriteBatch, Texture2D texture)
        {
            for(int i = 0; i < verts.Length; i++)
            {
                spriteBatch.Draw(texture, new Vector2(verts[i].X - texture.Width/2, verts[i].Y - texture.Height/2), Color.White);
            }
        }

        public void DrawEdges(SpriteBatch spriteBatch, Texture2D texture)
        {
            DrawLine(spriteBatch, verts[0], verts[1], texture);
            DrawLine(spriteBatch, verts[0], verts[2], texture);
            DrawLine(spriteBatch, verts[1], verts[3], texture);
            DrawLine(spriteBatch, verts[2], verts[3], texture);
        }

        public void DrawQuad(SpriteBatch spriteBatch, Texture2D square)
        {
            //float i1 = 0;
            //float j1 = 0;
            //int slopeX1 = (int) InterpSlope(Slope(verts[0], verts[1]), Slope(verts[2], verts[3]), i1);
            //int slopeY1 = (int) InterpSlope(Slope(verts[0], verts[2]), Slope(verts[1], verts[3]), j1);
            //Game1.debug = ((Slope(verts[0], verts[1]))).ToString() + ", " + (slopeY1).ToString();
            for (float i = 0; i <= 1; i += .002f)
            {
                Vector2 leftLine = verts[2] - verts[0];
                Vector2 rightLine = verts[3] - verts[1];
                Vector2 leftPoint = leftLine * i + verts[0];
                Vector2 rightPoint = rightLine * i + verts[1];
                Vector2 finalLine = rightPoint - leftPoint;
                for (float j = 0; j <= 1; j += .001f)
                {
                    spriteBatch.Draw(square, new Vector2((int)(finalLine.X * j + leftPoint.X),
                        (int)(finalLine.Y * j + leftPoint.Y)), textureData[(int)(j*(quadTexture.Width-1)), (int)(i * quadTexture.Height)]);
                }
            }
        }

        private float InterpSlope(float slope1, float slope2, float interp)
        {
            return (slope1 * interp) + (slope2 * (1 - interp));
        }

        private float Slope(Vector2 start, Vector2 end)
        {
            return (end.Y - start.Y)/(end.X - start.X);
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Texture2D texture)
        {
            Vector2 lineToStart = end - start;

            for (float i = 0; i < 1; i += .005f)
            {
                //increment throug the line and draw small sprites to generate a visual line
                spriteBatch.Draw(texture, new Vector2((int)(lineToStart.X * i + start.X),
                    (int)(lineToStart.Y * i + start.Y)), Color.White);
            }
        }

    }
}
