using System;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using SpriteKit;

#if __IOS__
using UIKit;
#else
using AppKit;
#endif

namespace SpriteKitGame
{
    public class GameScene : SKScene
    {
        SKSpriteNode bg = SKSpriteNode.FromImageNamed("background");
        SKSpriteNode player1 = SKSpriteNode.FromImageNamed("right_sprite");

        protected GameScene(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here
            var myLabel = new SKLabelNode("ArialMT")
            {
                //Text = "*",
                FontSize = 50,
                Position = new CGPoint(Frame.Width / 2, Frame.Height / 2)
            };

            player1.Position = new CGPoint(Frame.Size.Width / 2, Frame.Size.Height / 2);
            player1.ZPosition = 1;

            BackgroundColor = NSColor.Black;

            bg.Position = new CGPoint(XScale = Frame.Size.Width / 2, YScale = Frame.Size.Height / 2);
            bg.Size = new CGSize(Frame.Size.Height, Frame.Size.Height);
            player1.Size = new CGSize(Frame.Size.Height / 10, Frame.Size.Height / 10);

            AddChild(bg);
            AddChild(player1);
            AddChild(myLabel);
        }

#if __IOS__
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			// Called when a touch begins
			foreach (var touch in touches) {
				var location = ((UITouch)touch).LocationInNode (this);

				var sprite = new SKSpriteNode ("Spaceship") {
					Position = location,
					XScale = 0.5f,
					YScale = 0.5f
				};

				var action = SKAction.RotateByAngle (NMath.PI, 1.0);

				sprite.RunAction (SKAction.RepeatActionForever (action));

				AddChild (sprite);
			}
		}
#else
        public override void KeyDown(NSEvent theEvent)
        {
            // Called when a mouse click occurs

            var location = theEvent.LocationInNode(this);

            SKSpriteNode sprite = SKSpriteNode.FromImageNamed("front_sprite");
            //var sprite = SKSpriteNode.FromImageNamed(NSBundle.MainBundle.PathForResource("Spaceship", "png"));

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

