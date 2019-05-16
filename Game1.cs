using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Shards.ContentManager;
#pragma warning disable 612, 618
namespace Shards
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D[] ShardTextures;
        Grid grid;
        public static Rectangle Bounds;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.ToggleFullScreen();
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        
        protected override void Initialize()
        {
            ContentManager.Initialize(this);
            Bounds = graphics.GraphicsDevice.Viewport.Bounds;
            TouchPanel.EnabledGestures = GestureType.DragComplete | GestureType.FreeDrag;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ShardTextures = new[] { Content.Load<Texture2D>("Jewels/jewel_blue"), Content.Load<Texture2D>("Jewels/jewel_green"), Content.Load<Texture2D>("Jewels/jewel_red"), Content.Load<Texture2D>("Jewels/jewel_violet"), Content.Load<Texture2D>("Jewels/jewel_yellow") };
            LoadTextures(("combo", "combo/combo"));
            for (int i = 0; i < 10; i++)
            {
                LoadTextures(($"number{i.ToString()}",$"combo/{i.ToString()}"));
            }

            grid = new Grid(7, 11);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)  Exit();


            if (TouchPanel.IsGestureAvailable)
            {
                var sample = TouchPanel.ReadGesture();
                Vector2 touchPosition = sample.Position;
                Vector2 fling = sample.Delta;
                grid.SwapJewels(touchPosition, fling.Fling());
            }

            TouchLocation location =TouchPanel.GetState().FirstOrDefault();
            if(location != null)
            {
                if (location.Position.Y > 1600) grid.PopulateGrid();
            }
            grid.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);
            spriteBatch.Begin();

            grid.Draw(spriteBatch);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
