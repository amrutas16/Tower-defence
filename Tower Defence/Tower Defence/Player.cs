using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defence
{
    class Player
    {
        private int money = 50;
        private int lives = 10;

        private Texture2D towerTexture;

        private List<Tower> towers = new List<Tower>();

        private MouseState mouseState; 
        private MouseState oldState; 

        private int cellX;
        private int cellY;
        private string newTowerType;
        private int tileX;
        private int tileY;
        private Texture2D bulletTexture;
        private Level level;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public int Lives
        {
            get 
            {
                return lives; 
            }
            set 
            { 
                lives = value; 
            }
        }

        public Player(Level level, Texture2D towerTexture, Texture2D bulletTexture)
        {
            this.level = level;
            this.towerTexture = towerTexture;
            this.bulletTexture = bulletTexture;
        }

        public string NewTowerType
        {
            set { newTowerType = value; }
        }

        public void Update(GameTime gameTime, List<Enemy> enemies)
        {
            mouseState = Mouse.GetState();

            cellX = (int)(mouseState.X / 32); 
            cellY = (int)(mouseState.Y / 32); 

            tileX = cellX * 32;
            tileY = cellY * 32;

            if (mouseState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                if (string.IsNullOrEmpty(newTowerType) == false)
                {
                    AddTower();
                }
            }
            
            foreach (Tower tower in towers)
            {
                if (tower.Target == null)
                {
                    tower.GetClosestEnemy(enemies);
                }

                tower.Update(gameTime);
            }

            oldState = mouseState;
        }

        private bool IsCellClear()
        {
            bool inBounds = cellX >= 0 && cellY >= 0 && 
                cellX < level.Width && cellY < level.Height;

            bool spaceClear = true;

            foreach (Tower tower in towers) 
            {
                spaceClear = (tower.Position != new Vector2(tileX, tileY));

                if (!spaceClear)
                    break;
            }

            bool onPath = (level.GetIndex(cellX, cellY) != 1);

            return inBounds && spaceClear && onPath; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower tower in towers)
            {
                tower.Draw(spriteBatch);
            }
        }

        public void AddTower()
        {
            Tower towerToAdd = null;

            switch (newTowerType)
            {
                case "Arrow Tower":
                    {
                        towerToAdd = new ArrowTower(towerTexture,
                            bulletTexture, new Vector2(tileX, tileY));
                        break;
                    }
            }
            if (IsCellClear() == true && towerToAdd.Cost <= money)
            {
                towers.Add(towerToAdd);
                money -= towerToAdd.Cost;
                newTowerType = string.Empty;
            }
        }
    }
}
