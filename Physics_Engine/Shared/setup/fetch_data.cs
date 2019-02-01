using System;
using System.Windows.Input;
using System.IO;

namespace fetch_data
{

    public abstract class sprite
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

    public class temp
    {
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
        string location = Directory.GetCurrentDirectory();
        string path_g = Directory.GetParent(location)+"/Resources/tables/";
        public sprite tempData(String ID, out sprite fetch)
        {
            StreamReader sprites = new StreamReader(path_g + "sprites.csv");
            StreamReader entities = new StreamReader(path_g + "entities.csv");
            StreamReader blocks = new StreamReader(path_g + "entities.csv");
            sprite temp;

            bool found = false;

            string[] data;
            while (!found)
            {
                data = sprites.ReadLine().Split(',');
                if(data[0]==ID)
                { found = true; temp.ID = data[0]; temp.Name = data[1]; temp.Solid = bool.Parse(data[2]);  temp.Type = data[3]; temp.defaultZ = int.Parse(data[4]);}
            }
            found = false;

            switch (temp.Type)
            {
                case "entity":
                    sprite.entity fetch = (sprite.entity)temp;
                    while(!found)
                    {
                        data = entities.ReadLine().Split(',');
                        if(data[0]==ID)
                        { found = true; fetch.Speed = int.Parse(data[1]); fetch.Health = int.Parse(data[2]); fetch.Strength = int.Parse(data[3]); }
                    }
                    break;

                case "block":
                    sprite.block fetch = (sprite.block)temp;
                    while (!found)
                    {
                        data = blocks.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch.Climbable = bool.Parse(data[1]); fetch.height = int.Parse(data[2]); fetch.Traversable = int.Parse(data[3]); }
                    }
                    break;

                default:
                    break;
            }
            return fetch;
        }
    }
}
