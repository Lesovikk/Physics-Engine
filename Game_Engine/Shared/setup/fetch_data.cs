using System;
using System.Windows.Input;
using System.IO;
using SpriteKit;

namespace Game_Engine.setup
{
    public abstract class Sprite
    {
        public String ID;
        public String Name;
        public bool Solid;
        public String Type;
        public int defaultZ;
        public string path;
        SKSpriteNode spriteNode;

        public class Entity : Sprite
        {
            public int Speed;
            public int Health;
            public int Strength;
            public string spritef;
            public string spriteb;
            public string spritel;
            public string spriter;
        }

        public class Block : Sprite
        {
            public bool Climbable;
            public int height;
            public bool Traversable;
        }

        public class Basic : Sprite
        { }
    }
    public class fetch_data
    {
        public fetch_data()
        {
        }
        /*
        public struct g_values
        {
            String ID;
            String Name;
            bool Solid;
            String Type;
            int defaultZ;
        }

        public struct e_values
        {
            String ID;
            int Speed;
            int Health;
            int Strength;
        }

        public struct b_values
        {
            String ID;
            bool Climbable;
            int height;
            bool Traversable;
        }
        */
        static string location = Directory.GetCurrentDirectory();
        string path_g = Directory.GetParent(location) + "/Resources/tables/";

        public Sprite Fetch(string ID, out Sprite.Entity fetch1, out Sprite.Block fetch2)
        {
            StreamReader sprites = new StreamReader(path_g + "sprites.csv");
            StreamReader entities = new StreamReader(path_g + "entities.csv");
            StreamReader blocks = new StreamReader(path_g + "entities.csv");
            Sprite.Basic temp = new Sprite.Basic();

            bool found = false;

            string[] data;
            while (!found)
            {
                data = sprites.ReadLine().Split(',');
                if (data[0] == ID)
                { found = true; temp.ID = data[0]; temp.Name = data[1]; temp.Solid = bool.Parse(data[2]); temp.Type = data[3]; temp.defaultZ = int.Parse(data[4]); temp.path = data[5]; }
            }
            found = false;

            switch (temp.Type)
            {
                case "entity":
                    fetch1 = new Sprite.Entity();
                    fetch1.ID = temp.ID; fetch1.Name = temp.Name; fetch1.Solid = temp.Solid; fetch1.Type = temp.Type; fetch1.defaultZ = temp.defaultZ;
                    while (!found)
                    {
                        data = entities.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch1.Speed = int.Parse(data[1]); fetch1.Health = int.Parse(data[2]); fetch1.Strength = int.Parse(data[3]); }
                    }
                    fetch2 = new Sprite.Block();
                    return temp;

                case "block":
                    fetch2 = new Sprite.Block();
                    fetch2.ID = temp.ID; fetch2.Name = temp.Name; fetch2.Solid = temp.Solid; fetch2.Type = temp.Type; fetch2.defaultZ = temp.defaultZ;
                    while (!found)
                    {
                        data = blocks.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch2.Climbable = bool.Parse(data[1]); fetch2.height = int.Parse(data[2]); fetch2.Traversable = bool.Parse(data[3]); }
                    }
                    fetch1 = new Sprite.Entity();
                    return temp;

                default:
                    fetch1 = new Sprite.Entity();
                    fetch1.ID = temp.ID; fetch1.Name = temp.Name; fetch1.Solid = temp.Solid; fetch1.Type = temp.Type; fetch1.defaultZ = temp.defaultZ;
                    fetch2 = new Sprite.Block();
                    fetch2.ID = temp.ID; fetch2.Name = temp.Name; fetch2.Solid = temp.Solid; fetch2.Type = temp.Type; fetch2.defaultZ = temp.defaultZ;
                    return temp;
            }
        }
    }
}
