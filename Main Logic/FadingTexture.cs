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

namespace Shards
{
    public class FadingTexture : IDrawable
    {
        Texture2D Texture;
        Rectangle rectangle;
        public bool alive = true;
        private int visibility = 255;

        public FadingTexture(Texture2D texture, Rectangle rectangle)
        {
            this.Texture = texture;
            this.rectangle = rectangle;
        }

        public void Update()
        {
            visibility -= 6;
            if(visibility <= 0)
            {
                alive = false;
            }
        }

        [Obsolete]
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(Texture, position: rectangle.TopLeftCorner, scale: new Vector2(rectangle.width / Texture.Width, rectangle.height / Texture.Height),color:Color.FromNonPremultiplied(255,255,255,visibility));
        }
    }
}