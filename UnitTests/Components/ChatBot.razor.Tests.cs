using System.Threading.Tasks;
using Bunit;
using ContosoCrafts.WebSite.Components;
using Microsoft.AspNetCore.Components.Web;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using Moq;
using Moq.Protected;

namespace UnitTests.Components
{
    public class ChatBotTests : Bunit.TestContext
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        }

        [Test]
        public void ChatBot_ToggleVisibility_Should_Show_And_Hide_ChatBot()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();

            // Act
            var button = cut.Find("#chatbot-button");
            button.Click();

            // Assert
            Assert.That(cut.Instance._isChatBotVisible, Is.True);

            // Act
            var closeButton = cut.Find(".close-button");
            closeButton.Click();

            // Assert
            Assert.That(cut.Instance._isChatBotVisible, Is.False);
        }

        [Test]
        public async Task ChatBot_HandleKeyPress_Should_Send_Message_On_Enter()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            // Act
            input.Change("test");
            await cut.InvokeAsync(() => input.KeyDown(new KeyboardEventArgs { Key = "Enter" }));

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(1));
            Assert.That(cut.Instance._messages[0].Role, Is.EqualTo("user"));
            Assert.That(cut.Instance._messages[0].Content, Is.EqualTo("test"));
        }

        [Test]
        public async Task ChatBot_HandleKeyPress_Should_Send_Message_On_Enter_With_Non_Empty_Input()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            // Act
            input.Change("test");
            await cut.InvokeAsync(() => input.KeyDown(new KeyboardEventArgs { Key = "Enter" }));

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(1));
            Assert.That(cut.Instance._messages[0].Role, Is.EqualTo("user"));
            Assert.That(cut.Instance._messages[0].Content, Is.EqualTo("test"));
        }

        [Test]
        public async Task ChatBot_HandleKeyPress_Should_Not_Send_Message_On_Empty_Input()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            // Act
            input.Change("");
            await cut.InvokeAsync(() => input.KeyDown(new KeyboardEventArgs { Key = "Enter" }));

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatBot_HandleKeyPress_Should_Not_Send_Message_On_Null_Input()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            // Act
            cut.Instance._userInput = null;
            await cut.InvokeAsync(() => input.KeyDown(new KeyboardEventArgs { Key = "Enter" }));

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatBot_HandleKeyPress_Should_Not_Send_Message_On_Whitespace_Input()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            // Act
            input.Change("   ");
            await cut.InvokeAsync(() => input.KeyDown(new KeyboardEventArgs { Key = "Enter" }));

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatBot_SendMessage_Should_Not_Send_Empty_Message()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var sendButton = cut.Find("button");

            // Act
            await cut.InvokeAsync(() => sendButton.Click());

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatBot_SendMessage_Should_Not_Send_When_Loading()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            cut.Instance._isLoading = true;
            var input = cut.Find("input");

            // Act
            input.Change("test");
            await cut.InvokeAsync(() => cut.Instance.SendMessage());

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatBot_SendMessage_Should_Handle_Successful_Response()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"messages\":[{\"role\":\"bot\",\"content\":\"response\"}]}")
                });
            cut.Instance._httpClient = _httpClient;

            // Act
            input.Change("test");
            await cut.Instance.SendMessage();

            // Assert
            Assert.That(cut.Instance._messages.Count, Is.EqualTo(1));
            Assert.That(cut.Instance._messages[0].Role, Is.EqualTo("bot"));
            Assert.That(cut.Instance._messages[0].Content, Is.EqualTo("response"));
        }

        [Test]
        public async Task ChatBot_SendMessage_Should_Handle_Failed_Response()
        {
            // Arrange
            var cut = RenderComponent<ChatBot>();
            var button = cut.Find("#chatbot-button");
            button.Click();
            var input = cut.Find("input");

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request failed"));
            cut.Instance._httpClient = _httpClient;

            // Act
            input.Change("test");
            await cut.InvokeAsync(() => cut.Instance.SendMessage());

            // Assert
            Assert.That(cut.Instance._error, Is.EqualTo("Failed to get response: Request failed"));
        }
    }
}
