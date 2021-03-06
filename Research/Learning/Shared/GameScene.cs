﻿using System;

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
        SKSpriteNode bg = SKSpriteNode.FromImageNamed("person");

        protected GameScene(IntPtr handle) : base(handle)
        {

            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here
            var myLabel = new SKLabelNode("ArialMT")
            {
                Text = "Can change text and bgcolor",
                FontSize = 50,
                Position = new CGPoint(Frame.Width / 2, Frame.Height / 2)
            };

            //BackgroundColor = NSColor.Black;
            bg.Position = new CGPoint(XScale = Frame.Size.Width / 2, YScale = Frame.Size.Height / 2);

            AddChild(bg);
            AddChild(myLabel);
        }

#if __IOS__
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			// Called when a touch begins
			foreach (var touch in touches) {
				var location = ((UITouch)touch).LocationInNode (this);

				var sprite = new SKSpriteNode ("logo") {
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
        public override void MouseDown(NSEvent theEvent)
        {
            // Called when a mouse click occurs

            var location = theEvent.LocationInNode(this);
            SKSpriteNode sprite = SKSpriteNode.FromImageNamed("person");
            //var sprite = SKSpriteNode.FromImageNamed(NSBundle.MainBundle.PathForResource("Spaceship", "png"));

            sprite.Position = location;
            sprite.SetScale(10);

            var action = SKAction.MoveBy(2, 2, 0.1);

            sprite.RunAction(SKAction.RepeatActionForever(action));
            //sprite.RunAction(SKAction.RepeatActionForever(action));
            AddChild(sprite);

        }
#endif

        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

