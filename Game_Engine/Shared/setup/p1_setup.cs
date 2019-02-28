using System;
using System.Diagnostics;
using SpriteKit;
using System.Windows.Input;
using CoreGraphics;

namespace Game_Engine.setup
{
    public class p1_setup
    {
        public p1_setup()
        {

        }

        /// <summary>
        /// Fetches the values for the entity p1.
        /// </summary>
        /// <returns>the sprite.entity p1.</returns>
        public Sprite.Entity p1()
        {
            fetch_data fetch_Data = new fetch_data();
            Sprite.Entity player = new Sprite.Entity();
            try
            {
                fetch_Data.Fetch("p1", out player, out Sprite.Block b);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("p1_setup p1()");
                Debug.WriteLine(ex);
            }
            player.spritef = player.path + "front.png"; player.spriteb = player.path + "back"; player.spritel = player.path + "left"; player.spriter = player.path + "right";
            return player;
        }

        // Generic block fetching program
        public Sprite.Block b(string ID)
        {
            fetch_data fetch_Data = new fetch_data();
            Sprite.Block block = new Sprite.Block();
            try
            {
                fetch_Data.Fetch(ID, out Sprite.Entity e, out block);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("p1_setup block setup");
                Debug.WriteLine(ex);
            }
            return block;
        }

        // Sets the postition and height of an entity
        public void setPos(ref Sprite.Entity sprite, ref Sprite[,] sprites, nfloat Height, nfloat Width)
        {
            // XY positions have a value between 1 and 10
            sprite.spriteNode.Position = new CoreGraphics.CGPoint(((sprite.xPos + 0.5) * (Height / 10)) + (Width - Height) / 2, (sprite.yPos + sprite.yShift + 0.5) * (Height / 10));
            // Adds their postition to the sprite array
            sprites[sprite.xPos, sprite.yPos] = sprite;
            sprite.spriteNode.ZPosition = sprite.defaultZ;
            sprite.spriteNode.Size = new CGSize(Height / 10, (Height*sprite.spriteh) / 150);
            Debug.WriteLine("Check height");
            Debug.WriteLine(sprite.spriteh);

        }

        // Sets the position and height of a block
        public void setPos(ref Sprite.Block sprite, ref Sprite[,] sprites, nfloat Height, nfloat Width)
        {
            // XY positions have a value between 1 and 10
            sprite.spriteNode.Position = new CoreGraphics.CGPoint(((sprite.xPos + 0.5) * (Height / 10)) + (Width - Height) / 2, (sprite.yPos + sprite.yShift + 0.5 + (sprite.spriteh - 15) / 30) * (Height / 10));
            // Adds their position to the sprite array
            sprites[sprite.xPos, sprite.yPos] = sprite;
            sprite.spriteNode.ZPosition = sprite.defaultZ;
            sprite.spriteNode.Size = new CGSize(Height / 10, (Height*sprite.spriteh) / 150);
            Debug.WriteLine(sprite.spriteNode.Position);
            Debug.WriteLine((sprite.yPos + sprite.yShift) * (Height / 10));
            Debug.WriteLine(sprite.spriteNode.Size);
        }

        public void move(ref Sprite.Entity entity, ref Sprite.Entity[,] entities)
        {
            string direction = "f";
            switch (direction)
            {
                case "f":
                    entity.yPos--;
                    break;
                case "b":
                    entity.yPos++;
                    break;
                case "l":
                    entity.xPos--;
                    break;
                case "r":
                    entity.xPos++;
                    break;
                default:
                    break;
            }
        }

        public void movef(ref Sprite.Entity entity, ref Sprite.Entity[,] entities)
        {
            if (entity.yPos == 10) { Debug.WriteLine("Can't move forward, max height reached"); }
            else if (entities[entity.xPos,entity.yPos+1]==null)
            {

            }
            
        }
    }
}
