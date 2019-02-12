using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using Game_Engine.setup;
using AppKit;

#if __IOS__
using UIKit;
#else
using AppKit;
#endif

namespace SpriteKitGame
{
    public class Entity : Sprite.Entity
    {
        public Entity(Sprite.Entity se) : base(se)
        { }
    }
    public class setup1 : p1_setup
    { }
    public class GameScene : SKScene
    {
        setup1 fetch;
        Entity player1;
        SKSpriteNode bg;
        SKSpriteNode p1;

        protected GameScene(IntPtr handle) : base(handle)
        {
            fetch = new setup1();
            player1 = new Entity(fetch.p1());
            //Sprite.Entity spe = fetch.p1();
            //player1 = new Entity(spe);

            bg = SKSpriteNode.FromImageNamed("background/background");
            p1 = SKSpriteNode.FromImageNamed(player1.spritef);
            //p1 = SKSpriteNode.FromImageNamed("sprites/player/p1front");
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here
            p1.Position = new CGPoint(Frame.Size.Width / 2, 155 * (Frame.Size.Height / 300));
            p1.ZPosition = 1;

            BackgroundColor = NSColor.Black;

            bg.Position = new CGPoint(XScale = Frame.Size.Width / 2, YScale = Frame.Size.Height / 2);
            bg.Size = new CGSize(Frame.Size.Height, Frame.Size.Height);
            p1.Size = new CGSize(Frame.Size.Height / 10, Frame.Size.Height / 10);

            AddChild(bg);
            AddChild(p1);
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
        public override void MouseDown(NSEvent theEvent)
        {
            // Called when a mouse click occurs
            var location = theEvent.LocationInNode(this);

            //SKSpriteNode sprite = SKSpriteNode.FromImageNamed("p1front");
            var sprite = SKSpriteNode.FromImageNamed(NSBundle.MainBundle.PathForResource("sprites/player/p1front", "png"));

            sprite.Position = location;
            sprite.SetScale(1f);

            var action = SKAction.MoveBy(2, 2, 0.1);
            //SKAction OutofBounds = SKAction.RemoveFromParent();

            //sprite.
            sprite.RunAction(SKAction.RepeatActionForever(action));

            AddChild(sprite);
        }
#endif
        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

