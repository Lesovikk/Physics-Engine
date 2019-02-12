using System;
using System.Diagnostics;
using SpriteKit;

namespace Game_Engine.setup
{
    public class p1_setup
    {
        public p1_setup()
        {

        }

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
                Debug.WriteLine(ex);
            }
            player.spritef = player.path + "front.png"; player.spriteb = player.path + "back"; player.spritel = player.path + "left"; player.spriter = player.path + "right";
            return player;
        }

        public void move(Sprite.Entity entity)
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

    }
}
