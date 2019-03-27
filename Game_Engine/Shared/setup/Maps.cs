using System;
using System.Diagnostics;
using System.IO;
using SpriteKit;

namespace Game_Engine.setup
{
    public class Maps
    {
        public Maps()
        {
        }

        public Sprite[,,] convert_map(string mapID)
        {
            block_setup setup = new block_setup();
            fetch_data fetch = new fetch_data();
            Sprite[,,] sprites = new Sprite[10, 10, 3];
            StreamReader map1 = null;

            try
            {
                map1 = new StreamReader(fetch.Get_path_map() + mapID + ".csv");
                Debug.WriteLine("No error for map path");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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

        public void Add_map(Sprite[,,] sprites)
        {
            fetch_data fetch = new fetch_data();
            string path_maps = fetch.Get_path_map();

            string[] mapsreader = null;
            StreamWriter mapswriter = null;

            try
            {
                mapsreader = File.ReadAllLines(path_maps + "maps.csv");
                mapswriter = File.AppendText(path_maps + "maps.csv");
                Debug.WriteLine("No error accessing maps.csv for writing");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            int map_ID = int.Parse(mapsreader[0].Split(',')[0]) + 1;

            for (int i = 0; i < sprites.GetLength(0); i++)
            {
                for (int j = 0; j < sprites.GetLength(1); j++)
                {
                    if(sprites[i,j,0]!=null)
                    { mapswriter.WriteLine("{0},{1},{2},{3}", map_ID, sprites[i, j, 0].xPos, sprites[i, j, 0].yPos, sprites[i, j, 0].ID); }
                }
            }
            mapswriter.Close();
        }

        public Sprite[,,] Getmap(nfloat Height, nfloat Width, int mapnum)
        {
            block_setup setup = new block_setup();
            fetch_data fetch = new fetch_data();
            Sprite[,,] sprites = new Sprite[10, 10, 3];
            StreamReader maps = null;

            try
            {
                maps = new StreamReader(fetch.Get_path_map() + "maps.csv");
                Debug.WriteLine("No error for map path");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            string[] row;

            do
            {
                row = maps.ReadLine().Split(',');
                if (int.Parse(row[0]) == mapnum)
                {
                    Sprite.Block block = new Sprite.Block(setup.b(row[3]));
                    block.xPos = int.Parse(row[1]); block.yPos = int.Parse(row[2]); block.zPos = 0;
                    block.spriteNode = SKSpriteNode.FromImageNamed(block.path);
                    setup.setPos(ref block, ref sprites, Height, Width);
                }
            }
            while (int.Parse(row[0]) <= mapnum);
            maps.Close();
            return sprites;
        }
    }
}
