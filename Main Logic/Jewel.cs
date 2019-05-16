using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Shards
{
    public class Jewel : IDrawable
    {
        public Rectangle rectangle;
        public Vector2 StartPosition;
        public Vector2 DestPosition;
        public Texture2D Texture;
        int number = -1;

        public Jewel(Rectangle rectangle, Texture2D texture,int number)
        {
            this.number = number;
            this.rectangle = rectangle;
            StartPosition = rectangle.TopLeftCorner;
            DestPosition = rectangle.TopLeftCorner;
            Texture = texture;
        }

        public void Move(Vector2 vector)
        {
            rectangle.X += vector.X;
            rectangle.Y += vector.Y;
        }

        public Vector2 Position { get => new Vector2(rectangle.X, rectangle.Y); set { rectangle.X = value.X;rectangle.Y = value.Y; } }
        public int Number { get => number; set => number = Number; }

        [Obsolete]
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture, position: rectangle.TopLeftCorner, scale: new Vector2(rectangle.width / Texture.Width, rectangle.height / Texture.Height));
        }
    }
}