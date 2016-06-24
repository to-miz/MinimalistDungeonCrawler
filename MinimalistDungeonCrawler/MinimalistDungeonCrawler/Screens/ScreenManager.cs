using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MinimalisticDungeonCrawler.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToAdd = new List<GameScreen>();

        Input.InputState input = new Input.InputState();

        bool isInitialized = false;

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;
        Texture2D spriteSheet;

        public SoundEffect powerup;

        public Random RNG = new Random();

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public Texture2D BlankTexture
        {
            get { return blankTexture; }
        }

        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

        public ScreenManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            isInitialized = true;

            base.Initialize();
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            input = new Input.InputState();
            input.update();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("data\\BitmapFont");
            blankTexture = content.Load<Texture2D>("data\\Blank");
            spriteSheet = content.Load<Texture2D>("data\\Spritesheet");
            powerup = content.Load<SoundEffect>(@"sounds\Powerup");

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            input.update();

            foreach (GameScreen screen in screens)
            {
                screen.HandleInput(input);
            }

            foreach (GameScreen screen in screens)
            {
                screen.Update(gameTime);
            }

            for (int i = screens.Count - 1; i >= 0; i--)
            {
                if (screens[i].Done)
                    screens.RemoveAt(i);
            }

            foreach (GameScreen screen in screensToAdd)
            {
                screens.Add(screen);
            }

            screensToAdd.Clear();
            base.Update(gameTime);
        }

        public void addScreen(GameScreen screen)
        {
            screen.screenManager = this;

            if (isInitialized)
            {
                screen.LoadContent();
            }

            if (screens.Count == 0)
                screens.Add(screen);
            else
                screensToAdd.Add(screen);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                screen.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
