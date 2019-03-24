using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Forms;
using System.Timers;

using SpriteKit;
using CoreGraphics;
using Game_Engine.setup;

#if !__IOS__
using AppKit;

namespace Game_Engine.movement
{
    public class movements
    {
        public movements()
        {
        }

        // Function that detects the key pressed

        public void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w' || e.KeyChar == 'W')
            {
                Debug.WriteLine("detected");
                //do something
            }
        }

        // Function that performs the movement
        public void movement(ref Sprite[,,] sprites, ref Sprite.Entity player1, string sprite, nfloat unit, int i, int j, nfloat Height, nfloat Width)
        {
            // Change sprite
            player1.spriteNode.Texture = SKTexture.FromImageNamed(sprite);
            player1.Speed = 1;
            //Checks if the current move has been completed
            if (!flag) { }

            // Checks if move is out of bounds
            else if ((0 > player1.xPos + i | player1.xPos + i >= sprites.GetLength(0)) | (0 > player1.yPos + j | player1.yPos + j >= sprites.GetLength(1)))
            {
                Debug.WriteLine("Successfully prevented player from moving out of area");
            }

            // Moves player in the desired direction on the current floor
            else if ((sprites[player1.xPos + i, player1.yPos + j, player1.zPos] == null || !sprites[player1.xPos + i, player1.yPos + j, player1.zPos].Solid) & (player1.zPos == 0 || sprites[player1.xPos + i, player1.yPos + j, player1.zPos - 1] != null))
            {
                sprites[player1.xPos, player1.yPos, player1.zPos] = null;
                displace(player1, unit, i, j);
                if (player1.zPos > 0) { player1.spriteNode.ZPosition = sprites[player1.xPos, player1.yPos, player1.zPos - 1].spriteNode.ZPosition + 2; }
            }

            // Moves a player in the desired direction and falls
            else if (player1.zPos > 0 && sprites[player1.xPos + i, player1.yPos + j, player1.zPos - 1] == null)
            {
                sprites[player1.xPos, player1.yPos, player1.zPos] = null;
                displace(player1, unit, i, j);
                while (player1.zPos > 0 && sprites[player1.xPos, player1.yPos, player1.zPos - 1] == null)
                {
                    player1.zPos--; player1.spriteNode.Position = new CGPoint(player1.spriteNode.Position.X, player1.spriteNode.Position.Y - (4 * Height / 150));
                }
                if (player1.zPos > 0) { player1.spriteNode.ZPosition = sprites[player1.xPos, player1.yPos, player1.zPos - 1].spriteNode.ZPosition + 2; }
                else { player1.spriteNode.ZPosition = player1.defaultZ + (9 - player1.yPos); }
            }

            // Moves player 1 up a floor whilst moving in the desired direction
            else if (sprites[player1.xPos + i, player1.yPos + j, player1.zPos].GetClimbable() && sprites[player1.xPos + i, player1.yPos + j, player1.zPos].GetHeight() - player1.zPos == 1)
            {
                sprites[player1.xPos, player1.yPos, player1.zPos] = null;
                player1.spriteNode.ZPosition = sprites[player1.xPos + i, player1.yPos + j, player1.zPos].spriteNode.ZPosition + 2;
                player1.spriteNode.Position = new CGPoint(player1.spriteNode.Position.X, player1.spriteNode.Position.Y + (4 * Height / 150));
                displace(player1, unit, i, j);
                player1.zPos++;
            }
        }

        // Function to move the player character. 
        public void move(ref Sprite.Entity player1, ref Sprite[,,] sprites, NSEvent theEvent, nfloat Height, nfloat Width, out int i, out int j)
        {
            // find direction
            string Direction = direction(theEvent);

            // unit of travel
            nfloat unit = Height / 10;

            // Move around player1 and sets previous position to null
            switch (Direction)
            {
                case "right":
                    movement(ref sprites, ref player1, player1.spriter, unit, 1, 0, Height, Width);
                    i = 1; j = 0;
                    break;

                case "left":
                    movement(ref sprites, ref player1, player1.spritel, unit, -1, 0, Height, Width);
                    i = -1;j = 0;
                    break;

                case "up":
                    movement(ref sprites, ref player1, player1.spriteb, unit, 0, 1, Height, Width);
                    i = 0;j = 1;
                    break;

                case "down":
                    movement(ref sprites, ref player1, player1.spritef, unit, 0, -1, Height, Width);
                    i = 0;j = -1;
                    break;
                default:
                    i = 0; j = 0;
                    break;
            }
        }

        public string direction(NSEvent theEvent)
        {
            string Direction = "";
            switch (theEvent.KeyCode)
            {
                case 13:
                    Direction = "up";
                    break;
                case 0:
                    Direction = "left";
                    break;
                case 1:
                    Direction = "down";
                    break;
                case 2:
                    Direction = "right";
                    break;
                default:
                    break;
            }
            return Direction;
        }

        private static System.Timers.Timer time;
        private static System.Timers.Timer time2;
        private static bool flag = true;
        private static bool addmove = false;

        public static void displace(Sprite.Entity entity, nfloat unit, int i, int j)
        {
            flag = false;
            addmove = false;
            time = new System.Timers.Timer();
            time2 = new System.Timers.Timer();
            time.Interval = 500 / (entity.Speed);
            time2.Interval = 500 / (entity.Speed * 150);
            time.Elapsed += make_true;
            time2.Elapsed += movetime;
            time.Enabled = true;
            time2.Enabled = true;
            while(!flag)
            {
                if(addmove)
                {
                    Debug.WriteLine("{0} : {1}", entity.spriteNode.Position, new CGPoint(entity.spriteNode.Position.X + i * (unit / 10), entity.spriteNode.Position.Y + j * (unit / 10)));
                    entity.spriteNode.Position = new CGPoint(entity.spriteNode.Position.X + i * (unit / 150), entity.spriteNode.Position.Y + j * (unit / 150));
                    addmove = false;
                    time.AutoReset = true;
                }
            }
            flag = true;
            addmove = false;
            time.Enabled = false;
            time.AutoReset = true;
            time2.Enabled = false;
        }

        public bool GetFlag()
        { return flag; }

        private static void make_true(object sender, ElapsedEventArgs e)
        { flag = true; }

        private static void movetime(object sender, ElapsedEventArgs e)
        { addmove = true; }
    }
}
#endif