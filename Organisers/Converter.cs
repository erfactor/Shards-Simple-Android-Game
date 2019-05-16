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
    public enum FlingType { Top, Right, Bot, Left}
    public static class Converter
    {
        public static FlingType Fling(this Vector2 vector)
        {
            float x = vector.X, y = vector.Y;
            if (y > x)
            {
                if (y > -x) return FlingType.Bot;
                else return FlingType.Left;
            }
            else
            {
                if (y > -x) return FlingType.Right;
                else return FlingType.Top;
            }
            
        }
    }
    public interface IDrawable
    {
        void Draw(SpriteBatch sprite);
    }
}