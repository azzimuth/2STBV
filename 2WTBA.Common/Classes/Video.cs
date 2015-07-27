namespace _2WTBA.Common.Classes
{
    public class Video
    {
        public string file_id { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        public int duration { get; set; }

        public PhotoSize thumb { get; set; }

        public string mime_type { get; set; }

        public int? file_size { get; set; }
    }
}
