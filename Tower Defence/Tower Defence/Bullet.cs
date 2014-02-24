using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tower_Defence
{
    public class Bullet : Sprite
    {
        private int damage;
        private int age;

        private int speed;

        public int Damage
        {
            get { return damage; }
        }

        public void Kill()
        {
            this.age = 200;
        }

        public bool IsDead()
        {
            return age > 100;
        }

        public override void Update(GameTime gameTime)
        {
            age++;
            position += velocity;

            base.Update(gameTime);
        }

        public Bullet(Texture2D texture, Vector2 position, float rotation,int speed, int damage)
            : base(texture, position)
        {
            this.rotation = rotation;
            this.damage = damage;

            this.speed = speed;
        }

        public void SetRotation(float value)
        {
            rotation = value;

            velocity = Vector2.Transform(new Vector2(0, -speed), Matrix.CreateRotationZ(rotation));
        }
    }
}
