using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace match_3
{
    public class Match3Game : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D textureLightning;
        private Texture2D textureLightningHor;
        private Texture2D texture;
        private Texture2D background;
        private Texture2D textureExplotion;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private SpriteFont font;
        private SpriteFont fontSmall;
        private bool ok;

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
            textureLightning = Content.Load<Texture2D>("lightning");
            textureLightningHor = Content.Load<Texture2D>("lightning_hor");
            font = Content.Load<SpriteFont>("font"); 
            fontSmall = Content.Load<SpriteFont>("font_small"); 
            board = new Board(texture, textureExplotion, background, textureLightning, textureLightningHor, font, fontSmall);
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

            if (board.SmallScreen)
            {
                if (board.GameMode == Mode.Score && currentMouseState.X >= 0 && currentMouseState.X <= 60 && currentMouseState.Y >= 520)
                    ok = true;
                else
                    ok = false;    
            }
            else
            {
                if (board.GameMode == Mode.Score && currentMouseState.X >= 0 && currentMouseState.X <= 100 && currentMouseState.Y >= 900)
                    ok = true;
                else
                    ok = false;    
            }

            if (lastKeyboardState.IsKeyDown(Keys.Escape) && currentKeyboardState.IsKeyUp(Keys.Escape))
                board.Init();

            if (board.GameMode == Mode.Game)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                    board.MouseLeftClick(currentMouseState.X, currentMouseState.Y);
                if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                    board.MouseRightClick();
            } else {
                if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && board.GameMode == Mode.Menu)
                    board.NotGameMouseLeftClick();
            }

            if (ok && currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                board.GameMode = Mode.Menu;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();

            board.Draw(gameTime, spriteBatch);
            
            if (ok)
            {
                if (board.SmallScreen)
                    spriteBatch.DrawString(fontSmall, "ok", new Vector2(10, 520), Color.Green, 0, new Vector2 (0, 0), 1, new SpriteEffects(), 0);
                else
                    spriteBatch.DrawString(font, "ok", new Vector2(20, 900), Color.Green, 0, new Vector2 (0, 0), 1, new SpriteEffects(), 0);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
