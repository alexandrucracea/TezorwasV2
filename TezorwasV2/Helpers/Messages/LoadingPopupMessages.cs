using CommunityToolkit.Mvvm.Messaging.Messages;


namespace TezorwasV2.Helpers.Messages
{
    public class ShowLoadingMessage : ValueChangedMessage<bool>
    {
        public ShowLoadingMessage(bool value) : base(value) { }
    }

    public class HideLoadingMessage : ValueChangedMessage<bool>
    {
        public HideLoadingMessage(bool value) : base(value) { }
    }
}
