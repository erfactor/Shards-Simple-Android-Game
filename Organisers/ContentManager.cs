using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Shards
{
    public static class ContentManager
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SoundEffect> SoundEffects;
        public static Dictionary<string, Song> Songs;
        public static Dictionary<string, Effect> Shaders;
        public static Game game;

        //public int MyProperty { get; private set; }

        public static void Initialize(Game game1)
        {
            Textures = new Dictionary<string, Texture2D>();
            SoundEffects = new Dictionary<string, SoundEffect>();
            Shaders = new Dictionary<string, Effect>();
            game = game1;
        }

        public static void LoadTextures(params (string nazwa, string file)[] TextureNames)
        {
            foreach (var pair in TextureNames)
            {
                Textures.Add(pair.nazwa, game.Content.Load<Texture2D>(pair.file));
            }
        }

        public static void LoadSoundEffects(params (string nazwa, string file)[] SoundEffectNames)
        {
            foreach (var pair in SoundEffectNames)
            {
                SoundEffects.Add(pair.nazwa, game.Content.Load<SoundEffect>(pair.file));
            }
        }

        public static void LoadShaders(params (string nazwa, string file)[] ShaderNames)
        {
            foreach (var pair in ShaderNames)
            {
                game.Content.Load<Effect>(pair.file);
                Shaders.Add(pair.nazwa, game.Content.Load<Effect>(pair.file));
            }
        }

        private static Random rnd = new Random((int)System.DateTime.UtcNow.TimeOfDay.TotalSeconds);


       
    }
}

