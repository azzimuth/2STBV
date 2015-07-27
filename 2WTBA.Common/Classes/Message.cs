using Newtonsoft.Json;

namespace _2WTBA.Common.Classes
{
    public class Message
    {
        public int message_id { get; set; }

        public User from { get; set; }

        public int date { get; set; }

        [JsonProperty("chat")]
        public User chat { get; set; }

        [JsonProperty("chat")]
        public GroupChat group_chat { get; set; }

        public User forward_from { get; set; }

        public int forward_date { get; set; }

        public Message reply_to_message { get; set; }

        public string text { get; set; }

        public Audio audio { get; set; }

        public Document document { get; set; }

        public PhotoSize[] photo { get; set; }

        public Sticker sticker { get; set; }

        public Video video { get; set; }

        public Contact contact { get; set; }

        public Location location { get; set; }

        public User new_chat_participant { get; set; }

        public User left_chat_participant { get; set; }

        public string new_chat_title { get; set; }

        public PhotoSize[] new_chat_photo { get; set; }

        public bool? delete_chat_photo { get; set; }

        public bool? group_chat_created { get; set; }

        public string caption { get; set; }
    }
}
