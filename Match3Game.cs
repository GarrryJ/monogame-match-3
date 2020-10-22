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
        private MouseState currentMouseState;

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
            texture = Content.Load<Texture2D>("assets_candy");
            board = new Board(texture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!board.IsInit)
                board.Init();

            MouseState lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton == ButtonState.Released)
                board.Init();

            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                board.MouseLeftClick(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
                board.MouseRightClick();

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