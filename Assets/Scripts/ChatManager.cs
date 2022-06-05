using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    string[] lines;

    public List<Message>[] messageList =
        {new List<Message>(), new List<Message>(), new List<Message>(), new List<Message>()};

    public GameObject textObject;
    public GameObject[] chatPanels = new GameObject[4];

    //0: FIRST team, THIEF
    //1: FIRST team, POLICE
    //2: SECOND team, THIEF
    //3: SECOND team, POLICE

    public void UpdateChat(string team, string type, string agentId, string text)
    {
        print(text);
        
        string line = agentId + ": " + text;
        Message newMessage = new Message();
        newMessage.text = line;
        GameObject newText;
        if (team.Equals("FIRST"))
            if (type.Equals("THIEF")) newText = Instantiate(textObject, (chatPanels[0]).transform);
            else newText = Instantiate(textObject, (chatPanels[1]).transform);
        else if (type.Equals("THIEF")) newText = Instantiate(textObject, (chatPanels[2]).transform);
        else newText = Instantiate(textObject, (chatPanels[3]).transform);

        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        if (type.Equals("FIRST"))
            if (type.Equals("THIEF")) (messageList[0]).Add(newMessage);
            else (messageList[1]).Add(newMessage);
        else if (type.Equals("THIEF")) (messageList[2]).Add(newMessage);
        else (messageList[3]).Add(newMessage);
    }

    [System.Serializable]
    public class Message
    {
        public string text;
        public TMP_Text textObject;
    }
}