using System;
using System.IO;
using SpriteKit;
using System.Diagnostics;
using CoreGraphics;

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
        public double actualX;
        public double actualY;

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
            public double Speed;
            public string last_direction;
            public CGPoint destination;
            //Create on startup
            public int Base_Speed;
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
                this.Base_Speed = entity.Base_Speed; this.Max_Speed = entity.Max_Speed; this.Acceleration = entity.Acceleration; this.Health = entity.Health; this.Strength = entity.Strength; this.spritef = entity.spritef; 
                this.spriteb = entity.spriteb; this.spritel = entity.spritel; this.spriter = entity.spriter;
            }
        }

        // Derived block class for rocks, obstacles and stairs
        public class Block : Sprite
        {
            public bool Climbable;
            public int height;
            public bool Traversable;
            public bool Breakable;
            public bool Moveable;

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

        // Paths to get to the resource tables
        static string location = Directory.GetCurrentDirectory();
        string path_g = Directory.GetParent(location).Parent.Parent.Parent.Parent.Parent + "/Shared/Resources/tables/";

        // Public function in case an outside class needs to access the directory
        public string Get_path_g()
        { return path_g; }

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
            sprites.Close();

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
                        { found = true; fetch1.Max_Speed = int.Parse(data[1]); fetch1.Base_Speed = int.Parse(data[2]); fetch1.Acceleration = int.Parse(data[3]); fetch1.Health = int.Parse(data[4]); fetch1.Strength = int.Parse(data[5]); }
                    }
                    fetch2 = new Sprite.Block();
                    entities.Close();
                    blocks.Close();
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
                    entities.Close();
                    blocks.Close();
                    return temp;

                default:
                    fetch1 = new Sprite.Entity();
                    fetch1.ID = temp.ID; fetch1.Name = temp.Name; fetch1.Solid = temp.Solid; fetch1.Type = temp.Type; fetch1.defaultZ = temp.defaultZ;
                    fetch2 = new Sprite.Block();
                    fetch2.ID = temp.ID; fetch2.Name = temp.Name; fetch2.Solid = temp.Solid; fetch2.Type = temp.Type; fetch2.defaultZ = temp.defaultZ;
                    entities.Close();
                    blocks.Close();
                    return temp;
            }
        }

        // Path to maps directory
        string path_map = Directory.GetParent(location).Parent.Parent.Parent.Parent.Parent + "/Shared/Resources/maps/";

        // Public function so that the maps directory can be used to access the maps directory without re-using code
        public string Get_path_map() { return path_map; }

        // Fetches a map file and imports it into a 3 dimensional array while instantiating the values
        public Sprite[,,] fetchMap(nfloat Height, nfloat Width, string mapID)
        {
            block_setup setup = new block_setup();
            Sprite[,,] sprites = new Sprite[10,10,3];
            StreamReader map1 = null;

            try
            {
                map1 = new StreamReader(path_map + mapID + ".csv");
                Debug.WriteLine("No error for map path");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error whilst trying to access map: {0}", ex);
            }

            string[] row;

            for (int y = 9; y >= 0; y--)
            {
                row = map1.ReadLine().Split(',');
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x]!="0")
                    {
                        Sprite.Block block = new Sprite.Block(setup.b(row[x]));
                        block.xPos = x; block.yPos = y; block.zPos = 0;
                        block.spriteNode = SKSpriteNode.FromImageNamed(block.path);
                        setup.setPos(ref block, ref sprites, Height, Width);
                    }
                }
            }
            map1.Close();
            return sprites;
        }

        // Fetches a map from a map file and imports it into a 3 dimensional array
        public Sprite[,,] fetchMap(string mapID)
        {
            block_setup setup = new block_setup();
            Sprite[,,] sprites = new Sprite[10, 10, 3];
            StreamReader map1 = null;

            try
            {
                map1 = new StreamReader(path_map + mapID + ".csv");
                Debug.WriteLine("No error for map path");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error whilst trying to access map: {0}", ex);
            }

            string[] row;

            for (int y = 9; y >= 0; y--)
            {
                row = map1.ReadLine().Split(',');
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] != "0")
                    {
                        Sprite.Block block = new Sprite.Block(setup.b(row[x]));
                        block.xPos = x; block.yPos = y; block.zPos = 0;
                        sprites[x, y, 0] = block;
                    }
                }
            }
            map1.Close();
            return sprites;
        }

        // Adds the map to the map database
        public void Add_map(Sprite[,,] sprites)
        {
            string[] mapsreader = null;
            StreamWriter mapswriter = null;

            try
            {
                mapsreader = File.ReadAllLines(path_map + "maps.csv");
                mapswriter = File.AppendText(path_map + "maps.csv");
                Debug.WriteLine("No error accessing maps.csv for writing");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to access maps.csv directory: {0}", ex);
            }

            try
            {
                int map_ID = int.Parse(mapsreader[0].Split(',')[0]) + 1;

                for (int y = 0; y < sprites.GetLength(0); y++)
                {
                    for (int x = 0; x < sprites.GetLength(1); x++)
                    {
                        if (sprites[x, y, 0] != null)
                        { mapswriter.WriteLine("{0},{1},{2},{3}", map_ID, sprites[x, y, 0].xPos, sprites[x, y, 0].yPos, sprites[x, y, 0].ID); }
                    }
                }
                Debug.WriteLine("Added map to the table without issue");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to add map to table: {0}", ex);
            }
            mapswriter.Close();
        }

        // Gets the map from the table and returns it in a Sprite array
        public Sprite[,,] Getmap(nfloat Height, nfloat Width, int mapnum)
        {
            block_setup setup = new block_setup();
            Sprite[,,] sprites = new Sprite[10, 10, 3];
            StreamReader maps = null;

            try
            {
                maps = new StreamReader(path_map + "maps.csv");
                Debug.WriteLine("No error for map path");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to access maps directory: {0}",ex);
            }

            string[] row;
            maps.ReadLine();
            try
            {
                do
                {
                    row = maps.ReadLine().Split(',');
                    if (int.TryParse(row[0],out int z) && int.Parse(row[0]) == mapnum)
                    {
                        Sprite.Block block = new Sprite.Block(setup.b(row[3]));
                        block.xPos = int.Parse(row[1]); block.yPos = int.Parse(row[2]); block.zPos = 0;
                        block.spriteNode = SKSpriteNode.FromImageNamed(block.path);
                        setup.setPos(ref block, ref sprites, Height, Width);
                    }
                }
                while (int.Parse(row[0]) <= mapnum);
                Debug.WriteLine("Fetched map from maps table without problem");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to fetch from maps table: {0}",ex);
            }
            maps.Close();
            return sprites;
        }
    }
}
