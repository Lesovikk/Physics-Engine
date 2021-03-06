﻿using System;

using CoreGraphics;
using Foundation;
using SpriteKit;

#if !__IOS__
using AppKit;
namespace SpriteKitGame
{
    public class GameScene : SKScene
    {
        SKSpriteNode bg = SKSpriteNode.FromImageNamed("background");
        SKSpriteNode p1 = SKSpriteNode.FromImageNamed("p1right");

        protected GameScene(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidMoveToView(SKView view)
        {
            // Setup your scene here
            p1.Position = new CGPoint(Frame.Size.Width / 2, 155*(Frame.Size.Height / 300));
            p1.ZPosition = 1;

            BackgroundColor = NSColor.Black;

            bg.Position = new CGPoint(XScale = Frame.Size.Width / 2, YScale = Frame.Size.Height / 2);
            bg.Size = new CGSize(Frame.Size.Height, Frame.Size.Height);
            p1.Size = new CGSize(Frame.Size.Height / 10, Frame.Size.Height / 10);

            AddChild(bg);
            AddChild(p1);
        }

        public override void KeyDown(NSEvent theEvent)
        {
            base.KeyDown(theEvent);
        }

        public override void KeyUp(NSEvent theEvent)
        {
            // Called when a key is

            var location = theEvent.LocationInNode(this);

            //SKSpriteNode sprite = SKSpriteNode.FromImageNamed("p1front");
            var sprite = SKSpriteNode.FromImageNamed(NSBundle.MainBundle.PathForResource("p1front", "png"));

            sprite.Position = location;
            sprite.SetScale(1f);

            var action = SKAction.MoveBy(2, 2, 0.1);
            //SKAction OutofBounds = SKAction.RemoveFromParent();

            //sprite.
            sprite.RunAction(SKAction.RepeatActionForever(action));

            AddChild(sprite);
        }
        public override void Update(double currentTime)
        {
            // Called before each frame is rendered
        }
    }
}

#endif