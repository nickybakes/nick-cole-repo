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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        public static string[] debugText = new string[5];
        public static float mapBoundary = 3;
        public static int resHeight = 576;
        public static int resWidth = 1024;
        Vector3[] corners = new[] { new Vector3(-mapBoundary, mapBoundary,  mapBoundary), new Vector3(mapBoundary, mapBoundary,  mapBoundary), new Vector3(-mapBoundary, mapBoundary, - mapBoundary), new Vector3(mapBoundary, mapBoundary, - mapBoundary), new Vector3(-mapBoundary, -mapBoundary,  mapBoundary), new Vector3(mapBoundary, -mapBoundary,  mapBoundary), new Vector3(-mapBoundary, -mapBoundary, - mapBoundary), new Vector3(mapBoundary, -mapBoundary, - mapBoundary) };

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int nudgeAmount = 60;

        private Texture2D whiteSquare;

        private Texture2D vertHandleTexture;

        private Texture2D edgeTexture;

        private ShapeDrawer drawer;
        private Quad quad;
        private Camera activeCamera;

        private KeyboardState kbState;
        private KeyboardState previouskbState;
        private SpriteFont arial18;

        private Texture2D[] skyboxTextures;
        private Quad[] skyboxQuads;

        private Texture2D checkerTexture;

        private RenderTarget2D finalRenderTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            //setting window properties. it can be resized, but at first we want it at a predetermined size
            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferHeight = resHeight;
            graphics.PreferredBackBufferWidth = resWidth;
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            finalRenderTarget = new RenderTarget2D(GraphicsDevice, resWidth, resHeight);
            vertHandleTexture = CreateTexture(GraphicsDevice, 32, 32, Color.White);
            edgeTexture = CreateTexture(GraphicsDevice, 8, 8, Color.Yellow);
            whiteSquare = CreateTexture(GraphicsDevice, 4, 4, Color.White);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial18 = Content.Load<SpriteFont>("Arial18");
            checkerTexture = Content.Load<Texture2D>("checker");

            skyboxTextures = new Texture2D[6];
            skyboxTextures[0] = Content.Load<Texture2D>("sky_trainyard_01bk");
            skyboxTextures[1] = Content.Load<Texture2D>("sky_trainyard_01dn");
            skyboxTextures[2] = Content.Load<Texture2D>("sky_trainyard_01ft");
            skyboxTextures[3] = Content.Load<Texture2D>("sky_trainyard_01lf");
            skyboxTextures[4] = Content.Load<Texture2D>("sky_trainyard_01rt");
            skyboxTextures[5] = Content.Load<Texture2D>("sky_trainyard_01up");

            //Vector3[] corners = new[] { new Vector3(-mapBoundary, mapBoundary, 5*mapBoundary), new Vector3(mapBoundary, mapBoundary, 5 * mapBoundary), new Vector3(-mapBoundary, mapBoundary, -5 * mapBoundary), new Vector3(mapBoundary, mapBoundary, -5 * mapBoundary), new Vector3(-mapBoundary, -mapBoundary, 5 * mapBoundary), new Vector3(mapBoundary, -mapBoundary, 5 * mapBoundary), new Vector3(-mapBoundary, -mapBoundary, -5 * mapBoundary), new Vector3(mapBoundary, -mapBoundary, -5 * mapBoundary) };
            skyboxQuads = new Quad[6];
            skyboxQuads[0] = new Quad(corners[3], corners[2], corners[7], corners[6], skyboxTextures[0]);
            skyboxQuads[1] = new Quad(corners[4], corners[5], corners[6], corners[7], skyboxTextures[1]);
            skyboxQuads[2] = new Quad(corners[6], corners[7], corners[2], corners[3], skyboxTextures[2]);
            skyboxQuads[3] = new Quad(corners[2], corners[0], corners[6], corners[4], skyboxTextures[3]);
            skyboxQuads[4] = new Quad(corners[1], corners[3], corners[5], corners[7], skyboxTextures[4]);
            skyboxQuads[5] = new Quad(corners[3], corners[2], corners[1], corners[0], skyboxTextures[5]);

            quad = new Quad(corners[3], corners[2], corners[1], corners[0], checkerTexture);

            activeCamera = new Camera();
            //quad = new Quad(new Vector2(200, 30), new Vector2(1200, 70), new Vector2(250, 800), new Vector2(1200, 600), checkerTexture);
            //quad = new Quad(new Vector2(0, 0), new Vector2(1920, 0), new Vector2(0, 1080), new Vector2(1920, 1080), checkerTexture);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            previouskbState = kbState;
            kbState = Keyboard.GetState();

            //if (kbState.IsKeyDown(Keys.W) && previouskbState.IsKeyUp(Keys.W))
            //    quad.verts[0].Y -= nudgeAmount;
            //if (kbState.IsKeyDown(Keys.S) && previouskbState.IsKeyUp(Keys.S))
            //    quad.verts[0].Y += nudgeAmount;
            //if (kbState.IsKeyDown(Keys.A) && previouskbState.IsKeyUp(Keys.A))
            //    quad.verts[0].X -= nudgeAmount;
            //if (kbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
            //    quad.verts[0].X += nudgeAmount;

            if (kbState.IsKeyDown(Keys.W) && previouskbState.IsKeyUp(Keys.W))
                activeCamera.MoveForward(1);
            if (kbState.IsKeyDown(Keys.S) && previouskbState.IsKeyUp(Keys.S))
                activeCamera.MoveForward(-1);
            if (kbState.IsKeyDown(Keys.A) && previouskbState.IsKeyUp(Keys.A))
                activeCamera.MoveSide(-1);
            if (kbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
                activeCamera.MoveSide(1);
            if (kbState.IsKeyDown(Keys.T) && previouskbState.IsKeyUp(Keys.T))
                activeCamera.MoveUp(-1);
            if (kbState.IsKeyDown(Keys.G) && previouskbState.IsKeyUp(Keys.G))
                activeCamera.MoveUp(1);

            //rotating camera with arrow keys
            if (kbState.IsKeyDown(Keys.Up) && previouskbState.IsKeyUp(Keys.Up))
                activeCamera.RotatePitch(1);
            if (kbState.IsKeyDown(Keys.Down) && previouskbState.IsKeyUp(Keys.Down))
                activeCamera.RotatePitch(-1);
            if (kbState.IsKeyDown(Keys.Right) && previouskbState.IsKeyUp(Keys.Right))
                activeCamera.RotateYaw(1);
            if (kbState.IsKeyDown(Keys.Left) && previouskbState.IsKeyUp(Keys.Left))
                activeCamera.RotateYaw(-1);

            //increasing and decreasing camera fov
            if (kbState.IsKeyDown(Keys.F) && previouskbState.IsKeyUp(Keys.F))
                activeCamera.fov += 5;
            if (kbState.IsKeyDown(Keys.V) && previouskbState.IsKeyUp(Keys.V))
                activeCamera.fov -= 5;

            //Quaternion.CreateFromYawPitchRoll();

            //Quaternion q = ToQuaternion(MathHelper.ToRadians(45), MathHelper.ToRadians(2), MathHelper.ToRadians(50));
            //debug = q.ToString();
            //Vector3 e = ToEulerAngles(q);
            //debug2 = MathHelper.ToDegrees(e.X) + ", " + MathHelper.ToDegrees(e.Y) + ", " + MathHelper.ToDegrees(e.Z);

            //debug = activeCamera.Project3DPointToScreen(new Vector3(200, 300, 30)).ToString();
            debugText[0] = "Point (0, 0, 30) projected on screen: " + activeCamera.PerspectiveProjection(new Vector3(0, 0, 30)).ToString();
            debugText[1] = "Camera rotation: " + ToDegrees(ToEulerAngles(activeCamera.rot)).ToString();
            debugText[2] = "Camera position: " + activeCamera.pos.ToString();
            debugText[3] = "Camera FoV: " + activeCamera.fov.ToString();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(finalRenderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //quad.DrawVerts(spriteBatch, vertHandleTexture);

            //quad.DrawEdges(spriteBatch, edgeTexture);

            //quad.DrawQuad(spriteBatch, whiteSquare);
            //spriteBatch.Draw(whiteSquare, new Vector2(960, 540) + activeCamera.Project3DPointToScreen(new Vector3(200, 300, 30)), Color.DeepPink);

            //for(int i = 0; i < skyboxQuads.Length; i++)
            //{
            //    skyboxQuads[i].DrawQuad3D(spriteBatch, activeCamera, whiteSquare);
            //}
            quad.DrawQuad3D(spriteBatch, activeCamera, whiteSquare);

            for (int i = 0; i < corners.Length; i++)
            {
                Vector2 pos = activeCamera.PerspectiveProjection(corners[i]);
                spriteBatch.Draw(vertHandleTexture, pos, Color.Black);
                spriteBatch.DrawString(arial18, i.ToString(), pos, Color.White);
            }

            skyboxQuads[2].DrawQuad3D(spriteBatch, activeCamera, whiteSquare);
            //skyboxQuads[3].DrawQuad3D(spriteBatch, activeCamera, whiteSquare);

            spriteBatch.Draw(vertHandleTexture, activeCamera.PerspectiveProjection(new Vector3(0, 0, 30)), Color.DeepPink);
            spriteBatch.Draw(vertHandleTexture, activeCamera.PerspectiveProjection(new Vector3(2, 0, 60)), Color.DeepPink);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();

            spriteBatch.Draw(finalRenderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            for(int i = 0; i < debugText.Length; i++)
            {
                if(debugText[i] != null)
                    spriteBatch.DrawString(arial18, debugText[i], new Vector2(0, i*25), Color.White);
            }
            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Color color)
        {
            //initialize a texture
            Texture2D texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = color;
            }

            //set the color
            texture.SetData(data);

            return texture;
        }

        public static Vector3 ToDegrees(Vector3 e)
        {
            return new Vector3(MathHelper.ToDegrees(e.X), MathHelper.ToDegrees(e.Y), MathHelper.ToDegrees(e.Z));
        }


        public static Vector3 ToEulerAngles(Quaternion q)
        {
            Vector3 angles;

            // roll (x-axis rotation)
            float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            angles.X = (float) Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            float sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                angles.Y = (float)(Math.PI / 2) * Math.Sign(sinp); // use 90 degrees if out of range
            else
                angles.Y = (float) Math.Asin(sinp);

            // yaw (z-axis rotation)
            float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            angles.Z = (float) Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        public static Quaternion ToQuaternion(float yaw, float pitch, float roll) // yaw (Z), pitch (Y), roll (X)
        {
            // Abbreviations for the various angular functions
            float cy = (float) Math.Cos(yaw * 0.5f);
            float sy = (float) Math.Sin(yaw * 0.5f);
            float cp = (float) Math.Cos(pitch * 0.5);
            float sp = (float) Math.Sin(pitch * 0.5);
            float cr = (float) Math.Cos(roll * 0.5);
            float sr = (float) Math.Sin(roll * 0.5);

            Quaternion q = new Quaternion();
            q.W = cr * cp * cy + sr * sp * sy;
            q.X = sr * cp * cy - cr * sp * sy;
            q.Y = cr * sp * cy + sr * cp * sy;
            q.Z = cr * cp * sy - sr * sp * cy;

            return q;
        }
    }
}
