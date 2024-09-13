using System.Numerics;
using Raylib_cs;

namespace REngine{
public class Text : IRenderable
{
    public Rectangle RenderRect { get => new Rectangle(Position,Dimensions); }
    
    public Vector2 Position {get;set;}
    public Vector2 Dimensions {get => Raylib.MeasureTextEx(font,text,Size,1.0f);}
    private String text;
    public int Size;
    public Color Color = Color.White;
    private Font font = Raylib.GetFontDefault();
    public Text(string text,Vector2 position, Color textColor, int textSize=10  ){
        this.text = text;
        Color = textColor;
        Size = textSize;
        Position = position;
        
    }   

    public void Render(RenderTexture2D target)
    {
        Raylib.BeginTextureMode(target);
        Raylib.DrawText(text, (int)Position.X, (int)Position.Y, Size, Color);
        Raylib.EndTextureMode();
    }
}
}

