namespace REngine
{
    public class DataModel
    {
        
        // data class dor deserialization from JSON
        public class SpriteEntity
        {
            public class Dimensions{
                public int w { get; set; }
                public int h { get; set; }
            }
            public class AnimationEntity{
                public int[] frameIndices { get; set;}
                public string name { get; set; }
            }
        
            public Dimensions dimensions { get; set; }
            public int frameRow { get; set; }
            
            public List<AnimationEntity> animations { get; set; }
            
            public string textureFilename { get; set; }
        }
        
        public static SpriteEntity LoadSpriteDataFromJSON(String JSONFile){
        try
        {
            var json = File.ReadAllText(JSONFile);
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<SpriteEntity>(json);
            return values;
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        
    }
}

}