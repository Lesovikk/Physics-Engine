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

        // Function that detects the key pressed
        /*
        public void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w' || e.KeyChar == 'W')
            {
                Debug.WriteLine("detected");
                //do something
            }
        }
        */
        // Function that performs the movement
        public void movement(ref Sprite[,,] sprites, ref Sprite.Entity player1, string sprite, ref CGPoint change, nfloat unit, int i, int j, nfloat Height, nfloat Width)
        {
            // Change sprite
            player1.spriteNode.Texture = SKTexture.FromImageNamed(sprite);
            int x = (int)((player1.actualX + i * 4) / 15);
            int y = (int)((player1.actualY + j * 4) / 15);
            Debug.WriteLine("Current pos: {0},{1}", player1.actualX, player1.actualY);
            Debug.WriteLine("Checker pos: {0},{1}", player1.actualX + 4 * i, player1.actualY + 4 * j);
            Debug.WriteLine("Destination: {0},{1}", player1.actualX + i * (3.75), player1.actualY + j * (3.75));
            Debug.WriteLine("Change: {0},{1}", x, y);
            sprites[player1.xPos, player1.yPos, player1.zPos] = null;
            // Checks if move is out of bounds
            if ((0 > player1.actualX + 4 * i | player1.actualX + 4 * i >= 15 * sprites.GetLength(0)) | (0 > player1.actualY + 4 * j | player1.actualY + 4 * j >= 15 * sprites.GetLength(1)))
            {
                Debug.WriteLine("Successfully prevented player from moving out of area");
            }

            // Moves player in the desired direction on the current floor
            else if ((sprites[x, y, player1.zPos] == null || !sprites[x, y, player1.zPos].Solid) & (player1.zPos == 0 || sprites[x, y, player1.zPos - 1] != null))
            {
                Debug.WriteLine("Moved in desired direction");
                player1.actualX += 3.75*i; player1.xPos = (int)(player1.actualX / 15); change.X = i * unit;
                player1.actualY += 3.75*j; player1.yPos = (int)(player1.actualY / 15); change.Y = j * unit;
            }

            // Moves a player in the desired direction and falls
            else if (player1.zPos > 0 && sprites[player1.xPos + i, player1.yPos + j, player1.zPos - 1] == null)
            {
                Debug.WriteLine("Moved in desired direction and fell");
                player1.actualX += 3.75 * i; player1.xPos = (int)(player1.actualX / 15); change.X = i * unit;
                player1.actualY += 3.75 * j; player1.yPos = (int)(player1.actualY / 15); change.Y = j * unit;
                while (player1.zPos > 0 && sprites[player1.xPos, player1.yPos, player1.zPos - 1] == null)
                {
                    player1.zPos--; player1.spriteNode.Position = new CGPoint(player1.spriteNode.Position.X, player1.spriteNode.Position.Y - (4 * Height / 150));
                }
            }

            // Moves player 1 up a floor whilst moving in the desired direction
            else if (sprites[x, y, player1.zPos].GetClimbable() && sprites[x, y, player1.zPos].GetHeight() - player1.zPos == 1)
            {
                Debug.WriteLine("Moved in desired direction and climbed");
                player1.spriteNode.Position = new CGPoint(player1.spriteNode.Position.X, player1.spriteNode.Position.Y + (4 * Height / 150)); ;
                player1.actualX += 3.75 * 2 * i; player1.xPos = (int)(player1.actualX / 15); change.X = 2 * i * unit;
                player1.actualY += 3.75 * 2 * j; player1.yPos = (int)(player1.actualY / 15); change.Y = 2 * j * unit;
                player1.zPos++;
            }
            else { Debug.WriteLine("Error"); }
            player1.spriteNode.ZPosition = player1.defaultZ + 4 * (9 - player1.yPos) + 2 * (player1.zPos);
        }

        // Function to move the player character. 
        public bool move(ref Sprite.Entity player1, ref Sprite[,,] sprites, NSEvent theEvent, ref CGPoint change, nfloat Height, nfloat Width)
        {
            // find direction
            string Direction = direction(theEvent);

            // unit of travel
            nfloat unit = (nfloat)(3.75 * Height / 150);

            // Acceleration
            if(player1.Type=="entity")
            {
                Debug.WriteLine("Prev.Speed: {0}", player1.Speed);
                Debug.WriteLine("Max Speed: {0}", player1.Max_Speed);
                Debug.WriteLine("{0},{1},{2}", player1.last_direction, Direction, player1.last_direction == Direction);
                if (player1.last_direction == Direction && player1.Speed<=player1.Max_Speed)
                {
                    player1.Speed += 0.1 * (double)(player1.Acceleration);
                    player1.last_direction = Direction;
                }
                else if(player1.last_direction!=Direction){ player1.Speed = player1.Base_Speed; player1.last_direction = Direction; }
            }

            Debug.WriteLine("Speed: {0}",player1.Speed);
            // Move around player1 and sets previous position to null
            switch (Direction)
            {
                case "right":
                    movement(ref sprites, ref player1, player1.spriter, ref change, unit, 1, 0, Height, Width);
                    break;

                case "left":
                    movement(ref sprites, ref player1, player1.spritel, ref change, unit, -1, 0, Height, Width);
                    break;

                case "up":
                    movement(ref sprites, ref player1, player1.spriteb, ref change, unit, 0, 1, Height, Width);
                    break;

                case "down":
                    movement(ref sprites, ref player1, player1.spritef, ref change, unit, 0, -1, Height, Width);
                    break;
                default:
                    break;
            }
            if (change.X == 0 & change.Y == 0)
            {
                return true;
            }
            else { return false; }
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