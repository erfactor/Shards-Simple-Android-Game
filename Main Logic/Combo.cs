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
    public class Combo : IDrawable
    {
        Texture2D Texture;
        Rectangle rectangle;
        public bool alive = true;
        int combo = 0;

        public Combo(Texture2D texture, Rectangle rectangle)
        {
            this.Texture = texture;
            this.rectangle = rectangle;
        }
        int[] combosList = new int[70];
        int index = 0;
        public void Update(int combolevel)
        {
            this.combo = combolevel;
            combosList[index++ % 70] = combolevel;
            if (combosList.All(c => c == combolevel))
            {
                if (visibility > 0)
                {
                    visibility -= 5;
                    if(visibility<=0) Grid.playerTouch = true;
                }
            }
            else
            {
                
                if (visibility < 200)  visibility += 10;  
            }
        }


        int visibility = 0;
        [Obsolete]
        public void Draw(SpriteBatch sprite)
        {
            if (combo > 9 || combo<2) return;
            sprite.Draw(Texture, position: rectangle.TopLeftCorner + new Vector2(0, 50), scale: new Vector2(rectangle.width / Texture.Width, rectangle.height / Texture.Height)/2, color: Color.FromNonPremultiplied(255, 255, 255, visibility));
            numberTexture = ContentManager.Textures[$"number{combo}"];
            sprite.Draw(numberTexture, position: rectangle.Move(new Vector2(rectangle.width / 4 - 10, rectangle.height / 4 - 18)).TopLeftCorner + new Vector2(0,50), scale: new Vector2(rectangle.width / numberTexture.Width / 4, rectangle.height / numberTexture.Height / 1.65f)/2, color: Color.FromNonPremultiplied(255, 255, 255,Math.Max(0, visibility)));
            
        }
        Texture2D numberTexture;
    }
}