using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_MessageContainer : UIPanel
{
    [SerializeField] private UI_Message ui_messagePrefab;
    [SerializeField] private RectTransform messageContainer;
    private List<UI_Message> ui_messages;

    private MessageSystem messageSystem;

    public void Initialize()
    {
        messageSystem = MessageSystem.current;
        messageSystem.addMessage += AddMessage;

        ui_messages = messageContainer.GetComponentsInChildren<UI_Message>(true).ToList();
        foreach (UI_Message ui_message in ui_messages)
        {
            ui_message.Initialize();
            ui_message.Hide();
        }
    }

    private void AddMessage(string message)
    {
        UI_Message ui_AvaliableMessage = ui_messages.Where(x => x.isAvaliable).ToList().FirstOrDefault();

        if (ui_AvaliableMessage == null)
        {
            ui_AvaliableMessage = Instantiate(ui_messagePrefab, messageContainer);
        }

        ui_AvaliableMessage.InitializeMessage(message);
        messageContainer.SetSiblingIndex(messageContainer.childCount - 1);
    }
}