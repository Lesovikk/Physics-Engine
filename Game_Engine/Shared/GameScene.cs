﻿using System;
using CoreGraphics;
using Foundation;
using SpriteKit;
using Game_Engine.setup;
using Game_Engine.movement;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.Timers;

#if __IOS__
using UIKit;
#else
using AppKit;
#endif



namespace SpriteKitGame
{
    public class setup1 : p1_setup
    { }
    public class GameScene : SKScene
    {
#if __IOS__
#else
        // Initialising instances of classes present in the game.
        setup1 fetch;
        fetch_data data;
        movements move;
        Sprite.Entity player1;
        SKSpriteNode bg;
        Sprite[,,] sprites;
        nfloat Height;
        nfloat Width;
        SKAction action;
        bool Nextmove;

        protected GameScene(IntPtr handle) : base(handle)
        {
            fetch = new setup1();
            data = new fetch_data();
            move = new movements();
            player1 = new Sprite.Entity(fetch.p1());
            sprites = new Sprite[10, 10, 3];
            Nextmove = false;
            //accelx = 0;
            //accely = 0;
            Height = Frame.Size.Height;
            Width = Frame.Size.Width;
            bg = SKSpriteNode.FromImageNamed("background/background");
            player1.spriteNode = SKSpriteNode.FromImageNamed(player1.spritef);
            //p1 = SKSpriteNode.FromImageNamed("sprites/player/p1front");
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here

            // test values for position
            player1.xPos = 0; player1.yPos = 0; player1.zPos = 0;
            fetch.setPos(ref player1, ref sprites, Height, Width);

            sprites = data.fetchMap("map4");
            data.Add_map(sprites);
            // Fetches data from the stored map and implements it into the game
            sprites = data.Getmap(Height, Width, 3);
            //sprites = data.fetchMap(Height, Width, "map1");
            player1.spriteNode.ZPosition = 1;

            BackgroundColor = NSColor.Black;

            bg.Position = new CGPoint(XScale = Frame.Width / 2, YScale = Frame.Height / 2);
            bg.Size = new CGSize(Height, Height);
            Debug.WriteLine("background: ");
            Debug.WriteLine(bg.Size);
            Debug.WriteLine(Frame.Size.Height);
            //player1.spriteNode.Size = new CGSize(Frame.Size.Height / 10, Frame.Size.Height / 10);

            AddChild(bg);
            AddChild(player1.spriteNode);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Debug.WriteLine(sprites[i, j, 0]);
                    if (sprites[i, j, 0] != null)
                    {
                        AddChild(sprites[i, j, 0].spriteNode);
                    }
                }
            }
            /*
            AddChild(block1.spriteNode);
            AddChild(step1.spriteNode);
            AddChild(block2.spriteNode);
            AddChild(step2.spriteNode);
            */
        }
#endif

#if __IOS__
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            // Called when a touch begins
            foreach (var touch in touches)
            {
                var location = ((UITouch)touch).LocationInNode(this);

                var sprite = new SKSpriteNode("Spaceship")
                {
                    Position = location,
                    XScale = 0.5f,
                    YScale = 0.5f
                };

                var action = SKAction.RotateByAngle(NMath.PI, 1.0);

                sprite.RunAction(SKAction.RepeatActionForever(action));

                AddChild(sprite);
            }
        }
#else
        // Move when key is down, and previous movement finished
        public override void KeyDown(NSEvent theEvent)
        {
            // Called when a key is pressed
            base.KeyDown(theEvent);

            if (GetFlag(player1) || GetNear(player1))
            {
                var change = new CGPoint();

                bool collide = move.move(ref player1, ref sprites, theEvent, ref change, Height, Width);
                //fetch.Form1_KeyPress()

                sprites[player1.xPos, player1.yPos, player1.zPos] = player1;
                // Debug
                Debug.WriteLine("Player1 x: {0}, y: {1}", player1.xPos, player1.yPos);
                for (int i = 9; i >= 0; i--)
                {
                    Debug.Write("|");
                    for (int j = 0; j <= 9; j++)
                    {
                        if (sprites[j, i, player1.zPos] == null)
                        { Debug.Write("0|"); }

                        else if (sprites[j, i, player1.zPos] == player1)
                        { Debug.Write("p|"); }

                        else if (sprites[j, i, player1.zPos].Type == "block")
                        { Debug.Write("b|"); }
                    }
                    Debug.WriteLine("");
                }


                if (GetFlag(player1))
                {
                    action = SKAction.MoveTo(new CGPoint(player1.spriteNode.Position.X + change.X, player1.spriteNode.Position.Y + change.Y), 0.5 / (player1.Speed));
                    player1.destination = new CGPoint(player1.spriteNode.Position.X + change.X, player1.spriteNode.Position.Y + change.Y);
                    player1.spriteNode.RunAction(action);
                }
                else
                {
                    Nextmove = true;
                    player1.destination = new CGPoint(player1.destination.X + change.X, player1.destination.Y + change.Y);
                    action = SKAction.MoveTo(player1.destination, 0.5 / (player1.Speed));
                }
            }
            //AddChild(sprite);
        }

        public bool GetFlag(Sprite.Entity entity)
        {
            if(!entity.spriteNode.HasActions)
            {
                return true;
            }
            else { return false; }
        }
        public bool GetNear(Sprite.Entity entity)
        {
            if (Math.Sqrt(Math.Pow(player1.spriteNode.Position.X - player1.destination.X, 2)) <= 2 && (player1.last_direction == "left" || player1.last_direction == "right"))
                { return true; }
            else if (Math.Sqrt(Math.Pow(player1.spriteNode.Position.Y - player1.destination.Y, 2)) <= 2 && (player1.last_direction == "up" || player1.last_direction == "down"))
                { return true; }
            else { return false; }
        }
#endif
        public override void Update(double currentTime)
        {
            // Called before each frame is updated
            if (GetFlag(player1) && Nextmove)
            {
                player1.spriteNode.RunAction(action);
                Nextmove = false;
            }
        }
    }
}

