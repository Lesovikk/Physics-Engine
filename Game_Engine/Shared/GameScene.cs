﻿using System;
using CoreGraphics;
using Foundation;
using SpriteKit;
using Game_Engine.setup;
using Game_Engine.movement;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;

#if __IOS__
using UIKit;
#else
using AppKit;
#endif



namespace SpriteKitGame
{
    public class GameScene : SKScene
    {
#if __IOS__
#else
        // Initialising instances of classes present in the game.
        p1_setup fetchp1;
        movements move;
        fetch_data data;
        Sprite.Entity player1;
        SKSpriteNode bg;
        Sprite[,,] sprites;
        nfloat Height;
        nfloat Width;
        //int accelx;
        //int accely;

        protected GameScene(IntPtr handle) : base(handle)
        {
            fetchp1 = new p1_setup();
            move = new movements();
            data = new fetch_data();
            player1 = new Sprite.Entity(fetchp1.p1());
            sprites = new Sprite[10, 10, 3];
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

            fetchp1.setPos(ref player1, ref sprites, Height, Width);

            // Fetches data from the stored map and implements it into the game
            sprites = data.fetchMap(Height, Width);
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
        public override void KeyDown(NSEvent theEvent)
        {
            // Called when a key is pressed
            base.KeyDown(theEvent);
            var change = new CGPoint();

            sprites[player1.xPos, player1.yPos, player1.zPos] = null;
            player1.xPos = Convert.ToInt32((player1.spriteNode.Position.X - (Width - Height) / 2) / (Height / 10));
            player1.yPos = Convert.ToInt32(player1.spriteNode.Position.Y / (Height / 10));
            sprites[player1.xPos, player1.yPos, player1.zPos] = player1;

            move.move(ref player1, ref sprites, theEvent, ref change, Height, Width);
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

            var action = SKAction.MoveTo(new CGPoint(player1.spriteNode.Position.X + change.X, player1.spriteNode.Position.Y + change.Y), 0.5);

            //var action = SKAction.MoveBy(change.X, change.Y, 0.5);
            //SKAction OutofBounds = SKAction.RemoveFromParent();
            player1.spriteNode.RunAction(action);
        }
#endif
        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}
