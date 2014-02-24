using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tower_Defence
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameStates
        {
            Menu,
            Running,
            End,
        }
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level = new Level();
        //Tower tower;
        Player player;
        //Enemy enemy1;
        //Wave wave;
        WaveManager waveManager;
        Toolbar toolbar;
        Button arrowButton;
        SpriteFont arial;
        SpriteFont lucida;
        Menu menu;
        Input input;
        public static GameStates gameStates;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            menu = new Menu();
            input = new Input();
            graphics.PreferredBackBufferWidth = level.Width * 32;
            graphics.PreferredBackBufferHeight = (level.Height + 1) * 32;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            gameStates=GameStates.Menu;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D grass = Content.Load<Texture2D>("grass");
            Texture2D path = Content.Load<Texture2D>("path");

            level.AddTexture(grass);
            level.AddTexture(path);
            Texture2D enemyTexture = Content.Load<Texture2D>("enemy");

            arial = Content.Load<SpriteFont>("Arial");
            lucida = Content.Load<SpriteFont>("Lucida");
            /*enemy1 = new Enemy(enemyTexture, Vector2.Zero, 100, 10, 0.5f);
            enemy1.SetWaypoints(level.Waypoints);
            wave = new Wave(0, 10, level, enemyTexture);
            wave.Start();
            Texture2D towerTexture = Content.Load<Texture2D>("tower");
            tower = new Tower(towerTexture, Vector2.Zero);
            player = new Player(level, towerTexture);*/

            
            Texture2D towerTexture = Content.Load<Texture2D>("tower");
            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");
            player = new Player(level, towerTexture, bulletTexture);
            waveManager = new WaveManager(player, level, 30, enemyTexture);
            Texture2D topBar = Content.Load<Texture2D>("toolbar");
            SpriteFont font = Content.Load<SpriteFont>("Arial");
            toolbar = new Toolbar(topBar, font, new Vector2(0, level.Height * 32));

            Texture2D arrowNormal = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrow normal");
            Texture2D arrowHover = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrow hover");
            Texture2D arrowPressed = Content.Load<Texture2D>("GUI\\Arrow Tower\\arrow pressed");

            arrowButton = new Button(arrowNormal,arrowHover,arrowPressed,new Vector2(0,level.Height*32));
            arrowButton.Clicked += new EventHandler(arrowButton_Clicked);   
        }

        private void arrowButton_Clicked(object sender, EventArgs e)
        {
            player.NewTowerType = "Arrow Tower";
        }

        protected override void UnloadContent()
        {
        
        }

        protected override void Update(GameTime gameTime)
        {
            //enemy1.Update(gameTime);
            //List<Enemy> enemies = new List<Enemy>();
            //enemies.Add(enemy1);
            //wave.Update(gameTime);
            //player.Update(gameTime, wave.enemies);
            
            if (gameStates == GameStates.Menu)
            {
                /*if (input.Down)
                {
                    menu.Iterator++;
                }

                if (input.Up)
                {
                    menu.Iterator--;
                }

                if (input.MenuSelect)
                {
                    if (menu.Iterator == 0)
                    {
                        gameStates = GameStates.Running;
                    }
                    if (menu.Iterator == 1)
                    {
                        this.Exit();
                    }
                }*/

                if (input.Enter)
                {
                    gameStates = GameStates.Running;
                }
                if (input.Escape)
                {
                    this.Exit();
                }
            }

            else if (gameStates == GameStates.Running)
            {
                GameUpdate(gameTime);
            }

            else if (gameStates == GameStates.End)
            {
                if (input.Enter)
                {
                    this.Exit();
                }
            }

            base.Update(gameTime);
        }

        protected void GameUpdate(GameTime gameTime)
        {
            waveManager.Update(gameTime);
            player.Update(gameTime, waveManager.Enemies);
            arrowButton.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            if (gameStates == GameStates.Menu)
            {
                Texture2D texture = Content.Load<Texture2D>("start_screen");
                spriteBatch.Draw(texture, new Vector2(0f, 0f), Color.White);
            }

            else if (gameStates == GameStates.Running)
            {
                level.Draw(spriteBatch);
                //enemy1.Draw(spriteBatch);
                //wave.Draw(spriteBatch);
                player.Draw(spriteBatch);
                waveManager.Draw(spriteBatch);
                toolbar.Draw(spriteBatch, player);
                //tower.Draw(spriteBatch);
                arrowButton.Draw(spriteBatch);
            }

            else if (gameStates == GameStates.End)
            {
                menu.DrawEndScreen(spriteBatch, level.Width * 32, lucida);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
