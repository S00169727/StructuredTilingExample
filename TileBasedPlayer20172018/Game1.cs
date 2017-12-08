using AnimatedSprite;
using CameraNS;
using Engine.Engines;
using Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TileBasedPlayer20172018;
using Tiler;
using Tiling;

namespace Tiler
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        int health = 100;
        TilePlayer player;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sentry sentryTurret;
        HealthBar hbar;
        int tileWidth = 64;
        int tileHeight = 64;
        List<Sentry> sentryList = new List<Sentry>();
        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();
        string[] backTileNames = { "blue box", "pavement", "blue steel", "green box", "home", "end" };
        public enum TileType { BLUEBOX, PAVEMENT, BLUESTEEL, GREENBOX, HOME, END };
        int[,] tileMap = new int[,]
    {
        {4,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {1,2,2,2,2,2,1,1,2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,1,1,0,0,0,0,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,0,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,0,0,0,3,0,0,0,1,1,0,0,0,0,2,0,0,0,0,0,3,0,0,0,0,0,1,1,1,0,3,0,0,2,2,2,2,3,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,3,0,0,0,0,0,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,0,0,0,3,0,0,0,1,1,1,1,2,2,2,1,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,3,2,1,1,2,2,2,2,1,2,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,3,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

        //Audio
        Song backgroundMusic;
        //sauce
        //Sound fx
        SoundEffect[] sfx = new SoundEffect[5];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
            new Camera(this, Vector2.Zero, 
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));
            new InputEngine(this);
            
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
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64"));
            
            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            // "free", "pavement", "ground", "blue", "home", "grass" "end"
            TileRefs.Add(new TileRef(4, 2, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(6, 3, 2));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            TileRefs.Add(new TileRef(0, 1, 5));
            TileRefs.Add(new TileRef(0, 4, 6));
            // Names fo the Tiles
            
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);
            List<Tile> found = SimpleTileLayer.getNamedTiles("home");
            
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);
            List<Tile> foundSentry = SimpleTileLayer.getNamedTiles(backTileNames[(int)TileType.GREENBOX]);

            Services.AddService(new TilePlayer(this, new Vector2((int)TileType.HOME), new List<TileRef>()
            {
                new TileRef(15, 2, 0),
                new TileRef(15, 3, 0),
                new TileRef(15, 4, 0),
                new TileRef(15, 5, 0),
                new TileRef(15, 6, 0),
                new TileRef(15, 7, 0),
                new TileRef(15, 8, 0),
            }, 64, 64, 0f));
            SetColliders(TileType.GREENBOX);
            SetColliders(TileType.BLUEBOX);

            TilePlayer tilePlayer = Services.GetService<TilePlayer>();

            tilePlayer.AddHealthBar(new HealthBar(tilePlayer.Game, tilePlayer.PixelPosition));

            player = (TilePlayer)Services.GetService(typeof(TilePlayer));

            Projectile playerProjectile = new Projectile(this, new List<TileRef>()
            {
                new TileRef(8, 0, 0)
            },

            new AnimateSheetSprite(this, player.PixelPosition, new List<TileRef>()
            {
                new TileRef(0, 0, 0),
                new TileRef(1, 0, 1),
                new TileRef(2, 0, 2)
            }, 64, 64, 0), player.PixelPosition, 1);
            player.LoadProjectile(playerProjectile);


            for (int i = 0; i < foundSentry.Count; i++)
            {
                sentryTurret = new Sentry(this, new Vector2(foundSentry[i].X * tileWidth, foundSentry[i].Y * tileHeight), new List<TileRef>()
                {
                    new TileRef(20,2,0),
                    new TileRef(20,3,0),
                    new TileRef(20,4,0),
                    new TileRef(20,5,0),
                    new TileRef(20,6,0),
                    new TileRef(20,6,0),
                    new TileRef(20,8,0),
                }, 64,64, 0);
                sentryList.Add(sentryTurret);
            }

            for (int i = 0; i < sentryList.Count; i++)
            {
                Projectile projectile = new Projectile(this, new List<TileRef>() {
                new TileRef(8, 0, 0)
                },
                new AnimateSheetSprite(this, sentryList[i].PixelPosition, new List<TileRef>() {
                    new TileRef(0, 0, 0),
                    new TileRef(1, 0, 1),
                    new TileRef(2, 0, 2)
                }, 64, 64, 0), sentryList[i].PixelPosition, 1);

                sentryList[i].LoadProjectile(projectile);
                sentryList[i].Health = 20;
            }
            // TODO: use this.Content to load your game content here

            //Background 
            backgroundMusic = Content.Load<Song>(@"Audio/bckGroundMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.IsRepeating = true;

            //Sound FX
            sfx[0] = Content.Load<SoundEffect>(@"Audio/Explosion 2"); //Player Shot
            sfx[1] = Content.Load<SoundEffect>(@"Audio/Beep"); //Sentry Shot
            sfx[2] = Content.Load<SoundEffect>(@"Audio/Explosion"); //Projectile Explosion
            sfx[3] = Content.Load<SoundEffect>(@"Audio/Ching"); //Victory
            sfx[4] = Content.Load<SoundEffect>(@"Audio/Error"); //Fail
            
        }

        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }

                }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

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

            for (int i = 0; i < sentryList.Count; i++)
            {
                sentryList[i].Follow((AnimateSheetSprite)player);

                if (sentryList[i].sentryProjectile.ProjectileState == Projectile.PROJECTILE_STATE.EXPOLODING && sentryList[i].sentryProjectile.collisionDetect(player))
                {
                    if (!sentryList[i].sentryProjectile.hit)
                        player.Health -= 20;
                    sentryList[i].sentryProjectile.hit = true;
                }

                if (player.playerProjectle.ProjectileState == Projectile.PROJECTILE_STATE.EXPOLODING && player.playerProjectle.collisionDetect(sentryList[i]))
                {
                    if (!player.playerProjectle.hit)
                    {
                        sentryList[i].Die();
                        player.playerProjectle.hit = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        
    }
}
