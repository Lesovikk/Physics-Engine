using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using Game_Engine.setup;
using System.Windows.Input;
using System.Diagnostics;


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
        setup1 fetch;
        Sprite.Entity player1;
        Sprite.Block block1;
        Sprite.Block step1;
        Sprite.Block block2;
        Sprite.Block step2;
        SKSpriteNode bg;
        Sprite[,] sprites;
        nfloat Height;
        nfloat Width;
        int accelx;
        int accely;

        protected GameScene(IntPtr handle) : base(handle)
        {
            fetch = new setup1();
            player1 = new Sprite.Entity(fetch.p1());
            block1 = new Sprite.Block(fetch.b("b1"));
            step1 = new Sprite.Block(fetch.b("s1"));
            block2 = new Sprite.Block(fetch.b("b2"));
            step2 = new Sprite.Block(fetch.b("s2"));
            sprites = new Sprite[15, 15];
            accelx = 0;
            accely = 0;
            Height = Frame.Size.Height;
            Width = Frame.Size.Width;
            bg = SKSpriteNode.FromImageNamed("background/background");
            player1.spriteNode = SKSpriteNode.FromImageNamed(player1.spritef);
            block1.spriteNode = SKSpriteNode.FromImageNamed(block1.path);
            step1.spriteNode = SKSpriteNode.FromImageNamed(step1.path);
            block2.spriteNode = SKSpriteNode.FromImageNamed(block2.path);
            step2.spriteNode = SKSpriteNode.FromImageNamed(step2.path);
            //p1 = SKSpriteNode.FromImageNamed("sprites/player/p1front");
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here

            // test values for position
            player1.xPos = 0; player1.yPos = 0;
            block1.xPos = 8; block1.yPos = 9;
            step1.xPos = 5; step1.yPos = 5;
            block2.xPos = 9; block2.yPos = 8;
            step2.xPos = 8; step2.yPos = 8;

            fetch.setPos(ref player1, ref sprites, Height, Width);
            fetch.setPos(ref block1, ref sprites, Height, Width);
            fetch.setPos(ref step1, ref sprites, Height, Width);
            fetch.setPos(ref block2, ref sprites, Height, Width);
            fetch.setPos(ref step2, ref sprites, Height, Width);


            player1.spriteNode.ZPosition = 1;

            BackgroundColor = NSColor.Black;

            //bg.Position = new CGPoint(Height / 2, Width / 2);
            bg.Position = new CGPoint(XScale = Frame.Width / 2, YScale = Frame.Height / 2);
            bg.Size = new CGSize(Height, Height);
            Debug.WriteLine("background: ");
            Debug.WriteLine(bg.Size);
            Debug.WriteLine(Frame.Size.Height);
            //player1.spriteNode.Size = new CGSize(Frame.Size.Height / 10, Frame.Size.Height / 10);

            AddChild(bg);
            AddChild(player1.spriteNode);
            AddChild(block1.spriteNode);
            AddChild(step1.spriteNode);
            AddChild(block2.spriteNode);
            AddChild(step2.spriteNode);
        }

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

            var location = theEvent.LocationInNode(this);
            var difference = new CGPoint(location.X-player1.spriteNode.Position.X, location.Y - player1.spriteNode.Position.Y);
            var change = new CGPoint();

            // Move around player1 and sets previous position to null
            switch (Math.Abs(difference.X)>Math.Abs(difference.Y))
            {
                case true:
                    if(difference.X>=0 && sprites[player1.xPos+1, player1.yPos] == null)
                    { sprites[player1.xPos,player1.yPos] = null; player1.xPos++; change.X = Height / 10; }
                    //else if(difference.X>=0 && sprites[player1.xPos, player1.x])
                    else if(difference.X<0 && sprites[player1.xPos-1, player1.yPos]== null)
                    { sprites[player1.xPos,player1.yPos] = null; player1.xPos--; change.X = -Height / 10; }
                    break;
                case false:
                    if(difference.Y>=0 && sprites[player1.xPos, player1.yPos+1] == null)
                    { sprites[player1.xPos,player1.yPos] = null; player1.yPos++; change.Y = Height / 10; }
                    else if(difference.Y<0 && sprites[player1.xPos, player1.yPos-1]== null)
                    { sprites[player1.xPos,player1.yPos] = null; player1.yPos--; change.Y = -Height / 10; }
                    break;
                default:
                    break;
            }
            sprites[player1.xPos, player1.yPos] = player1;
            // Debug
            Debug.WriteLine("Player1 x: {0}, y: {1}", player1.xPos, player1.yPos);
            for (int i = 9; i >= 0; i--)
            {
                Debug.Write("|");
                for (int j = 0; j <= 9; j++)
                {
                    if (sprites[j, i] == player1)
                    { Debug.Write("p|"); }

                    else if (sprites[j, i] == block1 || sprites[j,i] == block2 || sprites [j,i] == step1 || sprites[j,i] == step2)
                    { Debug.Write("b|"); }

                    else
                    { Debug.Write("0|"); }
                }
                Debug.WriteLine("");
            }
            var action = SKAction.MoveTo(new CGPoint(player1.spriteNode.Position.X + change.X, player1.spriteNode.Position.Y + change.Y), 0.5);
            //var action = SKAction.MoveBy(change.X, change.Y, 0.5);
            //SKAction OutofBounds = SKAction.RemoveFromParent();

            player1.spriteNode.RunAction(SKAction.RepeatActionForever(action));

            //AddChild(sprite);

        }
        /*
        public void key_Press(object sender, KeyPressEventArgs e)
        {

        }
        */       
#endif
        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

