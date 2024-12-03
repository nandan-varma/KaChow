
using System;
using System.Collections.Generic;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    public class AiModelTests
    {
        [Test]
        public void AiResponse_GettersAndSetters_Should_Work_Correctly()
        {
            // Arrange
            var messages = new List<ChatMessage>
            {
                new ChatMessage { Role = "user", Content = "Hello", Timestamp = DateTime.UtcNow },
                new ChatMessage { Role = "bot", Content = "Hi there!", Timestamp = DateTime.UtcNow }
            };

            // Act
            var aiResponse = new AiResponse
            {
                Messages = messages
            };

            // Assert
            Assert.That(aiResponse.Messages, Is.EqualTo(messages));
            Assert.That(aiResponse.Messages.Count, Is.EqualTo(2));
            Assert.That(aiResponse.Messages[0].Content, Is.EqualTo("Hello"));
            Assert.That(aiResponse.Messages[1].Content, Is.EqualTo("Hi there!"));
        }
    }
}