namespace SportowyHub.Models.Api;

public record Conversation(
    int Id,
    string ListingId,
    string? ListingTitle,
    int BuyerId,
    int SellerId,
    string CreatedAt,
    string UpdatedAt);

public record ConversationsResponse(List<Conversation> Items, int Limit, int Offset);

public record Message(int Id, int? ConversationId, int SenderId, string Body, string CreatedAt);

public record MessagesResponse(List<Message> Items, int Limit, int Offset);

public record CreateConversationRequest(string ListingId);

public record SendMessageRequest(string Body);
