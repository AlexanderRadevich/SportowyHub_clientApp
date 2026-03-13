using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.Messaging;

internal class MessagingService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<MessagingService> logger) : IMessagingService
{
    public async Task<Result<Conversation>> CreateConversationAsync(string listingId, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<CreateConversationRequest, Conversation>(
                "/api/private/conversations",
                new CreateConversationRequest(listingId), token, ct: ct);
            return Result<Conversation>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to create conversation for listing {ListingId}", listingId);
            return Result<Conversation>.Failure(ex.Message);
        }
    }

    public async Task<Result<ConversationsResponse>> GetConversationsAsync(int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.GetAsync<ConversationsResponse>(
                $"/api/private/conversations?limit={limit}&offset={offset}", token, ct);
            return Result<ConversationsResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get conversations");
            return Result<ConversationsResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<Conversation>> GetConversationAsync(int conversationId, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.GetAsync<Conversation>(
                $"/api/private/conversations/{conversationId}", token, ct);
            return Result<Conversation>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get conversation {ConversationId}", conversationId);
            return Result<Conversation>.Failure(ex.Message);
        }
    }

    public async Task<Result<MessagesResponse>> GetMessagesAsync(int conversationId, int limit = 50, int offset = 0, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.GetAsync<MessagesResponse>(
                $"/api/private/conversations/{conversationId}/messages?limit={limit}&offset={offset}", token, ct);
            return Result<MessagesResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get messages for conversation {ConversationId}", conversationId);
            return Result<MessagesResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<Message>> SendMessageAsync(int conversationId, string body, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(body);
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<SendMessageRequest, Message>(
                $"/api/private/conversations/{conversationId}/messages",
                new SendMessageRequest(body), token, ct: ct);
            return Result<Message>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to send message to conversation {ConversationId}", conversationId);
            return Result<Message>.Failure(ex.Message);
        }
    }
}
