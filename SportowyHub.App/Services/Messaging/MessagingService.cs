using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Messaging;

internal class MessagingService(IRequestProvider requestProvider, IAuthService authService) : IMessagingService
{
    public async Task<Conversation> CreateConversationAsync(string listingId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<CreateConversationRequest, Conversation>(
            "/api/private/conversations/",
            new CreateConversationRequest(listingId), token, ct: ct);
    }

    public async Task<ConversationsResponse> GetConversationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.GetAsync<ConversationsResponse>(
            $"/api/private/conversations?limit={limit}&offset={offset}", token, ct);
    }

    public async Task<Conversation> GetConversationAsync(int conversationId, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.GetAsync<Conversation>(
            $"/api/private/conversations/{conversationId}", token, ct);
    }

    public async Task<MessagesResponse> GetMessagesAsync(int conversationId, int limit = 50, int offset = 0, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.GetAsync<MessagesResponse>(
            $"/api/private/conversations/{conversationId}/messages?limit={limit}&offset={offset}", token, ct);
    }

    public async Task<Message> SendMessageAsync(int conversationId, string body, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<SendMessageRequest, Message>(
            $"/api/private/conversations/{conversationId}/messages",
            new SendMessageRequest(body), token, ct: ct);
    }
}
