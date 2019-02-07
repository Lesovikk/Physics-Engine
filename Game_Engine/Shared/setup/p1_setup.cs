using System;
namespace Game_Engine.setup
{
    public class p1_setup
    {

        public Sprite.Entity p1()
        {
            fetch_data fetch_Data = new fetch_data();
            Sprite.Entity player = new Sprite.Entity();
            fetch_Data.Fetch("p1", out player, out Sprite.Block b);
            player.spritef = player.path + "front"; player.spriteb = player.path + "back"; player.spritel = player.path + "left"; player.spriter = player.path + "right";
            return player;
        }
    }
}
