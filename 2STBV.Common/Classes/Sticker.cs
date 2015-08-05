namespace _2STBV.Common.Classes
{
    public class Sticker
    {
        public string file_id { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public PhotoSize thumb { get; set; }       

        public int? file_size { get; set; }
    }
}
