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
    public class FadingNumber : FadingTexture
    {
        int number;
        public FadingNumber(int number , Rectangle rectangle)
        {
            this.number = number;
            this.rectangle = rectangle;
        }


        [Obsolete]
        public override void Draw(SpriteBatch sprite)
        {
            sprite.DrawString(Game1.font, number.ToString(), position: rectangle.TopLeftCorner, color: Color.FromNonPremultiplied(0, 0, 0, visibility),0,new Vector2(),scale:2,SpriteEffects.None,1);
        }
    }
}