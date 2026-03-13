using CommunityToolkit.Mvvm.Messaging.Messages;
using SportowyHub.Models.Api;

namespace SportowyHub.Messages;

public sealed class NavigateToSearchMessage(Section section) : ValueChangedMessage<Section>(section);
