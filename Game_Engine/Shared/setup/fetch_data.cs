using System;
using System.Windows.Input;
using System.IO;

namespace Game_Engine.setup
{
    public class sprite
    {
        public String ID;
        public String Name;
        public bool Solid;
        public String Type;
        public int defaultZ;
        public class entity : sprite
        {
            public int Speed;
            public int Health;
            public int Strength;
        }

        public class block : sprite
        {
            public bool Climbable;
            public int height;
            public bool Traversable;
        }
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
        public sprite fetch(String ID)
        {
            StreamReader sprites = new StreamReader(path_g + "sprites.csv");
            StreamReader entities = new StreamReader(path_g + "entities.csv");
            StreamReader blocks = new StreamReader(path_g + "entities.csv");
            sprite temp = new sprite();

            bool found = false;

            string[] data;
            while (!found)
            {
                data = sprites.ReadLine().Split(',');
                if (data[0] == ID)
                { found = true; temp.ID = data[0]; temp.Name = data[1]; temp.Solid = bool.Parse(data[2]); temp.Type = data[3]; temp.defaultZ = int.Parse(data[4]); }
            }
            found = false;

            switch (temp.Type)
            {
                case "entity":
                    sprite.entity fetch1 = (sprite.entity)temp;
                    while (!found)
                    {
                        data = entities.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch1.Speed = int.Parse(data[1]); fetch1.Health = int.Parse(data[2]); fetch1.Strength = int.Parse(data[3]); }
                    }
                    return fetch1;
                    break;

                case "block":
                    sprite.block fetch2 = (sprite.block)temp;
                    while (!found)
                    {
                        data = blocks.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch2.Climbable = bool.Parse(data[1]); fetch2.height = int.Parse(data[2]); fetch2.Traversable = bool.Parse(data[3]); }
                    }
                    return fetch2;
                    break;

                default:
                    return temp;
                    break;
            }
        }
    }
}
