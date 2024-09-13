using Raylib_cs;
using System.Numerics;
using System.Diagnostics;
using REngine;
namespace SpriteViewer;



class SpriteViewer
{

    public static void Main()
    {  
        
        Vector2 offset = new(20,5);

        
        System.Drawing.Point WinDim = new(
            800,
            600);  
        Renderer.Init((int)WinDim.X, (int)WinDim.Y);

        Sprite sprite = new(DataModel.LoadSpriteDataFromJSON("TestSprite/SpriteData.json"));
        sprite.Position = new Vector2(0,150) + offset;
        Text textline1 = new("Texture Test", offset, Color.Orange, 21);
        Text textline2 = new("Animations:",  (offset + new Vector2(0,400)), Color.Orange, 21);
        Text textline3 = new(string.Join(", ", sprite.AnimationNames) ,  (offset + new Vector2(0,450)), Color.Orange, 21);

        Renderer.RenderQueue.Enqueue(sprite);
        Renderer.RenderQueue.Enqueue(textline1);
        Renderer.RenderQueue.Enqueue(textline2);
        Renderer.RenderQueue.Enqueue(textline3);

        while (!Raylib.WindowShouldClose())
        {
            sprite.Update();
            // Draw full spritesheet with Rect of current frame of animation
            Renderer.Draw();
            //Renderer.DrawToBuffer(sprite.SpriteSheet, offset + new Vector2(0, 50));
            //Debug.WriteLine(sprite.Frame);    
        }

        Raylib.CloseWindow();
    }

  
}
