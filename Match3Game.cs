using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace match_3
{
    public class Match3Game : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Texture2D background;
        private Texture2D textureExplotion;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private SpriteFont font;
        private SpriteFont fontSmall;

        private Board board;

        public Match3Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Match 3";
            graphics.PreferredBackBufferHeight = 1000;
			graphics.PreferredBackBufferWidth = 1000;
            graphics.IsFullScreen = false;
			graphics.ApplyChanges();
            currentMouseState = Mouse.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("background_blur");
            texture = Content.Load<Texture2D>("assets_candy");
            textureExplotion = Content.Load<Texture2D>("explotion");
            font = Content.Load<SpriteFont>("font"); 
            fontSmall = Content.Load<SpriteFont>("font_small"); 
            board = new Board(texture, textureExplotion, background, font, fontSmall);
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (!board.IsInit)
                board.Init();

            KeyboardState lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (lastKeyboardState.IsKeyDown(Keys.F) && currentKeyboardState.IsKeyUp(Keys.F))
            {
                board.ScreenResize();
                if (board.SmallScreen)
                {
                    
                    graphics.PreferredBackBufferHeight = 600;
                    graphics.PreferredBackBufferWidth = 600;
                }
                else
                {
                    graphics.PreferredBackBufferHeight = 1000;
                    graphics.PreferredBackBufferWidth = 1000;
                }
                graphics.ApplyChanges();
            }

            MouseState lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    board.Init();

            if (board.GameMode == Mode.Game)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    board.MouseLeftClick(currentMouseState.X, currentMouseState.Y);
                if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    board.MouseRightClick();
            } else {
                if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    board.NotGameMouseLeftClick();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();

            board.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
