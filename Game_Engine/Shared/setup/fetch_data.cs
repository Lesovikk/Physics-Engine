using System;
using System.IO;
using SpriteKit;
using System.Diagnostics;

namespace Game_Engine.setup
{
    // Class for all sprites in-game
    public abstract class Sprite
    {
        //data from table
        public String ID;
        public String Name;
        public bool Solid;
        public String Type;
        public int defaultZ;
        public string path;

        //game data
        public SKSpriteNode spriteNode;
        public int xPos;
        public int yPos;
        public int zPos;
        public double yShift;
        public double spriteh;
        public virtual bool GetClimbable()
        { return false; }
        public virtual int GetHeight()
        { return 0; }

        // Derived Entity class for monsters and player
        public class Entity : Sprite
        {
            //Variable
            public int Speed;
            //Create on startup
            public int Max_Speed;
            public int Acceleration;
            public int Health;
            public int Strength;
            public string spritef;
            public string spriteb;
            public string spritel;
            public string spriter;

            // Functions to set up a new instance of the Entity derived class
            public Entity() { }

            public Entity(Entity entity)
            {
                this.ID = entity.ID; this.Name = entity.Name; this.Solid = entity.Solid; this.Type = entity.Type; this.defaultZ = entity.defaultZ; this.path = entity.path; 
                this.yShift = entity.yShift; this.spriteh = entity.spriteh;
                this.Max_Speed = entity.Max_Speed; this.Acceleration = entity.Acceleration; this.Health = entity.Health; this.Strength = entity.Strength; this.spritef = entity.spritef; 
                this.spriteb = entity.spriteb; this.spritel = entity.spritel; this.spriter = entity.spriter;
            }
        }

        // Derived block class for rocks, obstacles and stairs
        public class Block : Sprite
        {
            public bool Climbable;
            public int height;
            public bool Traversable;

            // Functions to set up a new instance of the Entity derived class
            public Block() { }

            public Block(Block block)
            {
                this.ID = block.ID; this.Name = block.Name; this.Solid = block.Solid; this.Type = block.Type; this.defaultZ = block.defaultZ; this.path = block.path;
                this.yShift = block.yShift; this.spriteh = block.spriteh;
                this.Climbable = block.Climbable; this.height = block.height; this.Traversable = block.Traversable;
            }

            public override bool GetClimbable()
            { return Climbable; }
            public override int GetHeight()
            { return height; }
        }

        // Basic class to help set up instances of the other derived classes
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
            int Max_Speed;
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
        // Paths to get to the resource tables
        static string location = Directory.GetCurrentDirectory();
        string path_g = Directory.GetParent(location).Parent.Parent.Parent.Parent.Parent + "/Shared/Resources/tables/";

        // Function to fetch values from the tables
        public Sprite Fetch(string ID, out Sprite.Entity fetch1, out Sprite.Block fetch2)
        {
            // Tries to set up streamreader from the tables. 
            Sprite.Basic temp=null; 
            StreamReader sprites=null, entities=null, blocks=null;
            try
            {
                sprites = new StreamReader(path_g + "sprites.csv");
                entities = new StreamReader(path_g + "entities.csv");
                blocks = new StreamReader(path_g + "blocks.csv");
                temp = new Sprite.Basic();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }

            bool found = false;
            string[] data;

            // Fetches values from sprites table and stores them in a temporary sprite.Basic
            while (!found)
            {
                data = sprites.ReadLine().Split(',');
                if (data[0] == ID)
                { found = true; temp.ID = data[0]; temp.Name = data[1]; temp.Solid = bool.Parse(data[2]); temp.Type = data[3]; temp.defaultZ = int.Parse(data[4]); temp.path = data[5]; temp.yShift = Convert.ToDouble(data[6]); temp.spriteh = int.Parse(data[7]); }
            }
            found = false;

            // Checks the type of the sprite whose value was fetched and sets up a corresponding sprite.entity or sprite.block
            switch (temp.Type)
            {
                case "entity":
                    fetch1 = new Sprite.Entity();
                    fetch1.ID = temp.ID; fetch1.Name = temp.Name; fetch1.Solid = temp.Solid; fetch1.Type = temp.Type; fetch1.defaultZ = temp.defaultZ; fetch1.path = temp.path; fetch1.yShift = temp.yShift; fetch1.spriteh = temp.spriteh;
                    while (!found)
                    {
                        data = entities.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch1.Max_Speed = int.Parse(data[1]); fetch1.Acceleration=int.Parse(data[3]); fetch1.Health = int.Parse(data[2]); fetch1.Strength = int.Parse(data[3]); }
                    }
                    fetch2 = new Sprite.Block();
                    return temp;

                case "block":
                    fetch2 = new Sprite.Block();
                    fetch2.ID = temp.ID; fetch2.Name = temp.Name; fetch2.Solid = temp.Solid; fetch2.Type = temp.Type; fetch2.defaultZ = temp.defaultZ; fetch2.path = temp.path; fetch2.yShift = temp.yShift; fetch2.spriteh = temp.spriteh;
                    while (!found)
                    {
                        data = blocks.ReadLine().Split(',');
                        if (data[0] == ID)
                        { found = true; fetch2.Climbable = Convert.ToBoolean(int.Parse(data[1])); fetch2.height = int.Parse(data[2]); fetch2.Traversable = Convert.ToBoolean(int.Parse(data[3])); }
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
