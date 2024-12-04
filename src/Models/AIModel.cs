using System;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Models
{
    public class AiResponse
    {
        public List<ChatMessage> Messages { get; set; }
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
