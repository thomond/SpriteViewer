using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;

[assembly: InternalsVisibleTo("SpriteViewer.Tests")]
namespace REngine
{



    public class Animation {
        
        public List<Vector2> Frames;
        public int FrameIndex;
        public int FrameCounter;
        public int FrameSpeed;
        Vector2 FrameDim;
        public string Name;

        public Animation(Vector2 _FrameDim, List<Vector2> _Frames) : this(_FrameDim, _Frames, _Frames.Count){}
        

        public Animation(Vector2 _FrameDim, List<Vector2> _Frames, int _FrameSpeed){
            Frames = _Frames;
            FrameIndex = 0;
            FrameDim = _FrameDim;
            // Frames per secs 
            FrameSpeed = _FrameSpeed;
            FrameCounter = 0;
        }   

        public Rectangle FrameRect  { 
            get => new Rectangle(Frames[FrameIndex] * FrameDim,FrameDim);  
            
        }

        public void Update(){
            FrameCounter++;
            if (FrameCounter >= (60/FrameSpeed)){
                FrameCounter = 0;
                FrameIndex++;
                if(FrameIndex >= Frames.Count) FrameIndex = 0;// else FrameIndex = 1; 
            }
        }
    }

    public class Animations{
        private Dictionary<string, Animation> animations = new();

        public void Add(string animationName, Animation animation){
            animation.Name = animationName;
            animations[animationName] = animation;
        
        }

        public string[] Names { get => animations.Keys.ToArray(); }

        public Animation this[string name]
        {
            get
            {
                return this.animations[name];
            }
    }

    }

    public class Sprite :Texture,IRenderable
    {

        //public Texture2D SpriteSheet {get; set;}

        private Animations animations = new();

        private String AnimationName {get;set;}
        
        public String[] AnimationNames {get => animations.Names;}

        public void SetAnimation(string name){
            AnimationName = name;
        }
        
        public Animation Animation { get { return animations[AnimationName]; } }
           
        //Gets the current frame of animation as an Image
        public Rectangle Frame 
        {
            get => Animation.FrameRect; 
        }

        //public Rectangle RenderRect {get => new Rectangle(Position,Dimensions); set { RenderRect = value; Position = value.Position;}}

        //public Vector2 Position { get ; set ; }

        new public Vector2  Dimensions {get => Frame.Size;} // Override Dimensions to account for spritemap

        public Sprite(string fn) {
            
            if(!Raylib.IsWindowReady()) throw new Exception("Raylib has not been Initialised");
            BackingTexture = Raylib.LoadTexture(fn);
            animations.Add( "walk_down",
                new Animation(new Vector2(32, 48), 
                new List<Vector2>{
                    new Vector2(0, 0),
                    new Vector2(1, 0), 
                    new Vector2(2, 0)
                }
                ));

            AnimationName = "walk_down";
            
        }

        public Sprite(){}

        // Load from sprite metadata object
        public Sprite(DataModel.SpriteEntity spriteMeta)
        {
            try
            {
                if(!Raylib.IsWindowReady()) throw new Exception("Raylib has not been Initialised");
                BackingTexture = Raylib.LoadTexture(spriteMeta.textureFilename);
            
                var spriteDim = new Vector2(spriteMeta.dimensions.w, spriteMeta.dimensions.h);

                foreach (var animationMeta in spriteMeta.animations)
                {
                    var frameIndices = new List<Vector2>();
                    foreach (var FrameIndex in animationMeta.frameIndices)
                    {
                        frameIndices.Add(new Vector2(FrameIndex,spriteMeta.frameRow));
                    }
                    
                    animations.Add(animationMeta.name, 
                    new Animation(spriteDim, frameIndices));
                } 

                AnimationName = spriteMeta.animations[0].name; // Set first ani in list to default
            }
            catch (System.Exception)
            {
                throw;
            }
            
            
        }

        public void Update()
        {
            animations[AnimationName].Update();
        }

        new public void Render(RenderTexture2D target)// Override to account for frame
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawTextureRec(BackingTexture, Frame, Position,Color.White);
            Raylib.EndTextureMode();
        }
    }

/// <summary>
/// Basic Texture renderable 
/// </summary>
    public class Texture : IRenderable{
        
        public Rectangle RenderRect =>  new Rectangle(Position,Dimensions);

        public Vector2 Position { get ; set ; }

        public Vector2 Dimensions {get => new Vector2(BackingTexture.Width,BackingTexture.Height)  ; }

        protected Texture2D BackingTexture{ get; set; }

        
        
        public void Render(RenderTexture2D target)
        {
            Raylib.BeginTextureMode(target);
            Raylib.DrawTextureV(BackingTexture, Position,Color.White);
            Raylib.EndTextureMode();
        }
    }
}
