using System;
using System.Diagnostics;
using SpriteKit;
using System.Windows.Input;
using CoreGraphics;
using System.Windows.Forms;
#if !__IOS__
using AppKit;

namespace Game_Engine.setup
{
    public class block_setup
    {
        public block_setup()
        {
        }

        // Generic block fetching program
        public Sprite.Block b(string ID)
        {
            fetch_data fetch_Data = new fetch_data();
            Sprite.Block block = new Sprite.Block();
            try
            {
                fetch_Data.Fetch(ID, out Sprite.Entity e, out block);
                Debug.WriteLine("No problems fetching: {0}", ID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("p1_setup block setup");
                Debug.WriteLine(ex + "\n");
            }
            return block;
        }

        // Sets the position and height of a block
        public void setPos(ref Sprite.Block sprite, ref Sprite[,,] sprites, nfloat Height, nfloat Width)
        {
            try
            {
                // XY positions have a value between 1 and 10
                sprite.spriteNode.Position = new CoreGraphics.CGPoint(((sprite.xPos + 0.5) * (Height / 10)) + (Width - Height) / 2, (sprite.yPos + sprite.yShift + 0.5 + (sprite.spriteh - 15) / 30) * (Height / 10));
                // Adds their position to the sprite array
                for (int i = 0; i < sprite.height; i++)
                {
                    sprites[sprite.xPos, sprite.yPos, i] = sprite;
                }
                sprite.spriteNode.ZPosition = sprite.defaultZ + (9 - sprite.yPos);
                sprite.spriteNode.Size = new CGSize(Height / 10, (Height * sprite.spriteh) / 150);
                Debug.WriteLine("no error setting position");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            Debug.WriteLine("Name: {0}, Climbable: {1}", sprite.Name, sprite.Climbable);
            Debug.WriteLine(sprite.spriteNode.Position);
            Debug.WriteLine((sprite.yPos + sprite.yShift) * (Height / 10));
            Debug.WriteLine(sprite.spriteNode.Size);
        }
    }
}
#endif