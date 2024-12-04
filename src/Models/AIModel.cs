using System.Collections.Generic;
using System;

namespace ContosoCrafts.WebSite.Models
{
    // Represents a response from an AI-powered chat system.
    public class AiResponse
    {
        // A list of chat messages included in the AI response.
        public List<ChatMessage> Messages { get; set; }
    }

    // Represents a single chat message in the conversation.
    public class ChatMessage
    {
        // The role of the entity in the conversation (e.g., "user", "assistant").
        public string Role { get; set; }

        // The actual content of the message.
        public string Content { get; set; }

        // The timestamp indicating when the message was sent or received.
        public DateTime Timestamp { get; set; }
    }
}