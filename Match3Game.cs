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
        private MouseState currentMouseState;
        private SpriteFont font;

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
            font = Content.Load<SpriteFont>("score"); 
            board = new Board(texture, textureExplotion);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!board.IsInit)
                board.Init();

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

            spriteBatch.Draw(background, new Vector2(0, 0) , new Rectangle(0, 0, 1000, 1000), Color.White);

            board.Draw(gameTime, spriteBatch);
            if (board.GameMode == Mode.Game)
                spriteBatch.DrawString(font, board.GameScore.ToString(), new Vector2(10, 10), Color.Black);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
