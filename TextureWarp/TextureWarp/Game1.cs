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
        public static string debug = "";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private int nudgeAmount = 60;

        private Texture2D whiteSquare;

        private Texture2D vertHandleTexture;

        private Texture2D edgeTexture;

        private ShapeDrawer drawer;
        private Quad quad;

        private KeyboardState kbState;
        private KeyboardState previouskbState;
        private SpriteFont arial18;

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
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            finalRenderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080);
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

            quad = new Quad(new Vector2(200, 30), new Vector2(1200, 70), new Vector2(250, 800), new Vector2(1200, 600), checkerTexture);
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

            if (kbState.IsKeyDown(Keys.W) && previouskbState.IsKeyUp(Keys.W))
                quad.verts[0].Y -= nudgeAmount;
            if (kbState.IsKeyDown(Keys.S) && previouskbState.IsKeyUp(Keys.S))
                quad.verts[0].Y += nudgeAmount;
            if (kbState.IsKeyDown(Keys.A) && previouskbState.IsKeyUp(Keys.A))
                quad.verts[0].X -= nudgeAmount;
            if (kbState.IsKeyDown(Keys.D) && previouskbState.IsKeyUp(Keys.D))
                quad.verts[0].X += nudgeAmount;


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

            quad.DrawQuad(spriteBatch, whiteSquare);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();

            spriteBatch.Draw(finalRenderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.DrawString(arial18, debug, Vector2.Zero, Color.White);
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
    }
}
