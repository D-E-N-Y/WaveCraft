using System;

public class MessageSystem : GameSystem
{
    public static MessageSystem current;
    public Action<string> addMessage;

    public override void Initialize()
    {
        current = this;
    }

    public void AddMessage(string message)
    {
        addMessage?.Invoke(message);
    }
}