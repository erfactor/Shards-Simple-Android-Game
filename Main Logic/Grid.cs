using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static Shards.ContentManager;
#pragma warning disable 612, 618
namespace Shards
{
    public class Grid : IDrawable
    {
        const int numOfJewels = 5;
        int width;
        int height;
        int[,] tab;
        Jewel[,] Jewels;
        List<FadingTexture> fadingTextures = new List<FadingTexture>();
        Random rnd = new Random();
        Combo combo;
        public static int combolevel = 0;
        public static bool playerTouch = false;
        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            tab = new int[width, height];
            Jewels = new Jewel[width, height];
            PopulateGrid();
            float halfwidth = Game1.Bounds.width / 2;
            float halfheight = Game1.Bounds.height / 2;
            combo = new Combo(Textures["combo"], new Rectangle(halfwidth/3 , halfheight * 1.3f, halfwidth * 4 / 3, halfheight * 0.6f));
        }

        public void PopulateGrid()
        {
            playerTouch = true;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    int random = rnd.Next(numOfJewels);
                    tab[i, j] = random;
                    Jewel jewel = new Jewel(new Rectangle(i * side, j * side, side, side), Game1.ShardTextures[random], random);
                    Jewels[i, j] = jewel;
                }
        }

        public float side { get { return Game1.Bounds.width / width; } }

        public void SwapJewels(Vector2 Touch,FlingType flingType)
        {
            if (GridMoving) return;
            playerTouch = true;
            int X = TouchX(Touch), Y = TouchY(Touch);
            try
            {
                switch (flingType)
                {
                    case FlingType.Top:
                        if (Y <= 0) break;
                        SwapTwoJewels(X, Y, X , Y - 1);
                        break;
                    case FlingType.Right:
                        if (X >= width - 1) break;
                        SwapTwoJewels(X, Y,X+1,Y);
                        break;
                    case FlingType.Bot:
                        if (Y >= height-1) break;
                        SwapTwoJewels(X, Y, X , Y+1);
                        break;
                    case FlingType.Left:
                        if (X <= 0) break;
                        SwapTwoJewels(X, Y, X - 1, Y );
                        break;
                }
            }
            catch { }
        }

        public int TouchX(Vector2 touch) { return (int)(touch.X / side); }
        public int TouchY(Vector2 touch) { return (int)(touch.Y / side); }

        private void SwapTwoJewels(int X, int Y,int X2, int Y2)
        {
            Jewel jewel1 = Jewels[X, Y];
            Jewel jewel2 = Jewels[X2, Y2];
            jewel1.DestPosition = jewel2.Position;
            jewel2.DestPosition = jewel1.Position;
            Jewels[X, Y] = jewel2;
            Jewels[X2, Y2] = jewel1;
            tab[X, Y] = jewel2.Number;
            tab[X2, Y2] = jewel1.Number;
            JewelsToMove.Add(jewel1);
            JewelsToMove.Add(jewel2);
        }
        private bool GridMoving
        {
            get
            {
                return Jewels.Any((Jewel jewel) => { return jewel is null ? false : jewel.DestPosition != jewel.Position; });
            }
        }

        public void Update()
        {
            FadingUpdate();
            FillGrid();
            DeleteLines();
            CheckForFreeSpace();
            MoveJewelsAction();
            ComboLogic();
        }

        private void ComboLogic()
        {
            combo.Update(combolevel);
        }

        private void FadingUpdate()
        {
            foreach(var fade in fadingTextures)
            {
                fade.Update();
            }
            fadingTextures = fadingTextures.Where(f => f.alive = true).ToList();
        }

        private void DeleteLines()
        {
            if (GridMoving) return;
            var ToDelete = CheckVerticalLine(0);
            for (int i = 1;i<width;i++)
                ToDelete.AddRange(CheckVerticalLine(i));
            for (int i = 0; i < height; i++)
                ToDelete.AddRange(CheckHorizontalLine(i));
            if (ToDelete.Count != 0)
            {
                foreach (var tuple in ToDelete)
                {
                    int x = tuple.x, y = tuple.y;
                    if (Jewels[x, y] == null) continue;
                    fadingTextures.Add(new FadingTexture(Jewels[x, y].Texture, Jewels[x, y].rectangle));
                    tab[x,y] = -1;
                    Jewels[x,y] = null;
                }
                if (playerTouch)
                {
                    playerTouch = false;
                    combolevel = 1;
                }
                else
                    combolevel++;
            }
        }

        private void FillGrid()
        {
            if (GridMoving) return;
            for (int i = 0; i < width; i++)
            {
                if (Jewels[i,0] == null)
                {
                    int random = rnd.Next(numOfJewels);
                    tab[i, 0] = random;
                    Jewels[i,0] = new Jewel(new Rectangle(i * side, 0, side, side), Game1.ShardTextures[random], random);
                }
            }
        }

        List<Jewel> JewelsToMove = new List<Jewel>();
        private void CheckForFreeSpace()
        {
            int x1, x2, x3, y1, y2, y3;
            var seq = (from x in Enumerable.Range(0, width)
                       join y in Enumerable.Range(0, height - 1)
                       on 1 equals 1
                       select (x, y)).ToArray().Shuffle();
                      ;
            for (int a = 0; a < seq.Length; a++)
                {
                int i = seq[a].x;
                int j = seq[a].y;
                    if (Jewels[i, j] == null) continue;
                    x1 = i - 1; x2 = i; x3 = i + 1;
                    y1 = j + 1; y2 = j + 1; y3 = j + 1;
                    if(tab[x2,y2] == -1)
                    {
                        CommonLogic(x2, y2, i, j);
                    }
                    else if (x1 >= 0 && tab[x1,y1] == -1)
                    {
                        CommonLogic(x1, y1, i, j);
                    }
                    else if (x3 <width && tab[x3, y3] == -1)
                    {
                        CommonLogic(x3, y3, i, j);
                    }
                }
        }
        private void CommonLogic(int x, int y,int i , int j)
        {
            Jewel jewel = Jewels[i, j];
            jewel.DestPosition = new Vector2(x * side, y * side);
            tab[x, y] = jewel.Number;
            tab[i, j] = -1;
            JewelsToMove.Add(jewel);
            Jewels[x, y] = jewel;
            Jewels[i, j] = null;
        }
        List<Jewel> Bin = new List<Jewel>();

        int TimeToMove = 38;
        private void MoveJewelsAction()
        {
            foreach(Jewel jewel in JewelsToMove)
            {
                Vector2 remaining = (jewel.DestPosition - jewel.Position);
                if (remaining.Length() > ((jewel.DestPosition - jewel.StartPosition)*2 / TimeToMove).Length())
                {
                    Vector2 vector = (jewel.DestPosition - jewel.StartPosition) / TimeToMove;
                    jewel.Move(vector);
                }
                else
                {
                    jewel.Position = jewel.DestPosition;
                    jewel.StartPosition = jewel.DestPosition;
                    Bin.Add(jewel);
                }
            }
            JewelsToMove = JewelsToMove.Except(Bin).ToList();
            Bin.Clear();
        }

        public void Draw(SpriteBatch sprite)
        {
            if (Jewels == null) return;

            foreach (var fade in fadingTextures)
            {
                fade.Draw(sprite);
            }

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (Jewels[i,j] == null || Jewels[i,j].Number == -1) continue;
                    Jewels[i,j].Draw(sprite);
                }
            combo.Draw(sprite);

        }

        public List<(int x,int y)> CheckVerticalLine(int line)
        {
            List<(int,int)> result = new List<(int x,int y)>();
            if (line >= width) return result;
            int comboLength = 1;
            for(int i = 1; i < height; i++)
            {
                int last = tab[line, i];
                int curr = tab[line, i - 1];
                if(curr == -1)
                {
                    comboLength = 0;continue;
                }
                if(last == curr)
                {
                    comboLength++;
                }
                else
                {
                    if (comboLength > 2)
                    {
                        result.AddRange(Enumerable.Range(1,comboLength).Select(x => (line,i-x)));
                    }
                    comboLength = 1;
                }
            }
            if (comboLength > 2)
            {
                result.AddRange(Enumerable.Range(1, comboLength).Select(x => (line, height - x)));
            }
            return result;
        }

        public List<(int x, int y)> CheckHorizontalLine(int line)
        {
            List<(int, int)> result = new List<(int x, int y)>();
            if (line >= height) return result;
            int comboLength = 1;
            for (int i = 1; i < width; i++)
            {
                int last = tab[i,line];
                int curr = tab[i-1,line];
                if (curr == -1)
                {
                    comboLength = 0; continue;
                }
                if (last == curr)
                {
                    comboLength++;
                }
                else
                {
                    if (comboLength > 2)
                    {
                        result.AddRange(Enumerable.Range(1, comboLength).Select(x => (i-x,line)));
                    }
                    comboLength = 1;
                }
            }
            if (comboLength > 2)
            {
                result.AddRange(Enumerable.Range(1, comboLength).Select(x => (width-x,line)));
            }
            return result;
        }
        public void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }



    public static class Extender
    {
        public static Rectangle Move(this Rectangle rect, Vector2 vec)
        {
            return new Rectangle((int)(rect.X + vec.X), (int)(rect.Y + vec.Y), rect.width, rect.height);
        }
        public static bool Any<T>(this T[,] array,Predicate<T> pred)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (pred(array[i, j])) return true;
                }
            return false;
        }
        public static T[] Shuffle<T>(this T[] array)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int n = array.Length; n > 1;)
            {
                int k = rnd.Next(n);
                --n;
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            return array;
        }
    }
}