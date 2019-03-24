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

        // Sets the postition and height of an entity
        public void setPos(ref Sprite.Entity sprite, ref Sprite[,,] sprites, nfloat Height, nfloat Width)
        {
            // XY positions have a value between 1 and 10
            sprite.spriteNode.Position = new CoreGraphics.CGPoint(((sprite.xPos + 0.5) * (Height / 10)) + (Width - Height) / 2, (sprite.yPos + sprite.yShift + 0.5) * (Height / 10));
            // Adds their postition to the sprite array
            sprite.actualX = sprite.xPos * 15 + 7.5;
            sprite.actualY = sprite.yPos * 15 + 7.5;
            sprites[sprite.xPos, sprite.yPos, 0] = sprite;
            sprite.spriteNode.ZPosition = sprite.defaultZ + (9-sprite.yPos);
            sprite.spriteNode.Size = new CGSize(Height / 10, (Height * sprite.spriteh) / 150);

        }
    }
}
#endif