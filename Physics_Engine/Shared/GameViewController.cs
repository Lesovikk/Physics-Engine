using System;
using SpriteKit;
using Foundation;
using AppKit;


namespace SpriteKitGame
{
    [Register("GameViewController")]
    public partial class GameViewController : NSViewController
    {
        protected GameViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Setup();
        }

        void Setup()
        {
            // Configure the view.
            GameView.ShowsFPS = true;
            GameView.ShowsNodeCount = true;
            /* Sprite Kit applies additional optimizations to improve rendering performance */
            GameView.IgnoresSiblingOrder = true;

            // Create and configure the scene.
            var scene = SKNode.FromFile<GameScene>("GameScene");
            scene.ScaleMode = SKSceneScaleMode.AspectFill;

            // Present the scene.
            GameView.PresentScene(scene);
        }
    }
}

