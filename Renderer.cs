using Raylib_cs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace REngine
{
    
    public interface IRenderable {
        Rectangle RenderRect{get; }
        public Vector2 Position {get;set;}
        public Vector2 Dimensions {get;}
        void Render(RenderTexture2D target);
    }
    
    public class Renderer
    {
        static private RenderTexture2D target;
        static public Queue<IRenderable> RenderQueue = new();

        static void Enqueue(IRenderable renderable) => RenderQueue.Enqueue(renderable);
        static public Color DefaultBackground = Color.Black;
        public static void Init(int screenWidth, int screenHeight)
        {
            Raylib.InitWindow(500,500, "-");   
            Raylib.SetTargetFPS(60);
            target = Raylib.LoadRenderTexture(screenWidth, screenHeight);

        }

        public static void DrawToBuffer(string text, Vector2 pos, Color textColor, int textSize)        
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawText(text, (int)pos.X, (int)pos.Y, textSize, textColor);
            Raylib.EndTextureMode();
        }

        public static void DrawToBuffer(Texture2D texture, Vector2 pos)
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawTextureV(texture, pos,Color.White);
            Raylib.EndTextureMode();
        }
        public static void DrawToBuffer(Texture2D texture, Rectangle frame, Vector2 pos)
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawTextureRec(texture, frame, pos,Color.White);
            Raylib.EndTextureMode();
        }


        public static void DrawToBuffer(Rectangle rect,Color color,float thickness=2f)
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawRectangleLinesEx( rect, thickness, color);
            Raylib.EndTextureMode();
        }
            

        public static void DrawToBuffer(Image image, Vector2 pos)
        {
            throw new NotImplementedException();
        }

        public static void Draw()
        {
            // Reset buffer
            Raylib.BeginTextureMode(target);
            Raylib.ClearBackground(DefaultBackground);
            Raylib.EndTextureMode();
            // Render objects to buffer
            foreach (var renderable in RenderQueue)
            {
                renderable.Render(target);
            }
            // Render buffer to screen
            Raylib.BeginDrawing();
            Raylib.ClearBackground(DefaultBackground);
            // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
            Raylib.DrawTextureRec(target.Texture, new Rectangle( 0, 0, target.Texture.Width, -target.Texture.Height), (new Vector2(0,0)), Color.White);
            Raylib.EndDrawing();
            
        }

    }
}
