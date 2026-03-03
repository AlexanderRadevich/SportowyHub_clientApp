using SportowyHub.Models.Api;

namespace SportowyHub.Services.Messaging;

public interface IMessagingService
{
    Task<Conversation> CreateConversationAsync(string listingId, CancellationToken ct = default);
    Task<ConversationsResponse> GetConversationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default);
    Task<Conversation> GetConversationAsync(int conversationId, CancellationToken ct = default);
    Task<MessagesResponse> GetMessagesAsync(int conversationId, int limit = 50, int offset = 0, CancellationToken ct = default);
    Task<Message> SendMessageAsync(int conversationId, string body, CancellationToken ct = default);
}
