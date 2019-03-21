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

            // Checks if move is out of bounds
            if ((0 > player1.xPos + i | player1.xPos + i >= sprites.GetLength(0)) | (0 > player1.yPos + j | player1.yPos + j >= sprites.GetLength(1)))
            {
                Debug.WriteLine("Successfully prevented player from moving out of area");
            }

            // Moves player in the desired direction on the current floor
            else if (sprites[player1.xPos + i, player1.yPos + j, player1.zPos] == null & (player1.zPos == 0 || sprites[player1.xPos + i, player1.yPos + j, player1.zPos - 1] != null))
            {
                sprites[player1.xPos, player1.yPos, player1.zPos] = null;
                player1.xPos += i; change.X = i * unit;
                player1.yPos += j; change.Y = j * unit;
                if(player1.zPos>0) { player1.spriteNode.ZPosition = sprites[player1.xPos, player1.yPos, player1.zPos - 1].spriteNode.ZPosition + 2; }
            }

            // Moves a player in the desired direction and falls
            else if (player1.zPos > 0 && sprites[player1.xPos + i, player1.yPos + j, player1.zPos - 1] == null)
            {
                sprites[player1.xPos, player1.yPos, player1.zPos] = null;
                player1.xPos += i; player1.yPos += j;
                change.X = i * unit; change.Y = j * unit;
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
                player1.spriteNode.Position = new CGPoint(player1.spriteNode.Position.X, player1.spriteNode.Position.Y + (4 * Height / 150)); ;
                player1.xPos += i; change.X = i * unit;
                player1.yPos += j; change.Y = j * unit;
                player1.zPos++;
            }
        }

        // Function to move the player character. 
        public void move(ref Sprite.Entity player1, ref Sprite[,,] sprites, NSEvent theEvent, ref CGPoint change, nfloat Height, nfloat Width)
        {
            // find direction
            string Direction = direction(theEvent);

            // unit of travel
            nfloat unit = Height / 10;

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
        }
    }
}
#endif