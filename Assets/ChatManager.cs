using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ChatManager : MonoBehaviour
{
    string[] lines;
    public List<Message> messageList = new List<Message>();
    public GameObject chatPanel, textObject;
    
    void Update()
    {
    if (new FileInfo( "texts.txt" ).Length != 0)
    {
        lines = System.IO.File.ReadAllLines(@"texts.txt");
        foreach (string line in lines)
        {
            Message newMessage = new Message();
            newMessage.text = line;
            GameObject newText = Instantiate(textObject, chatPanel.transform);
            newMessage.textObject = newText.GetComponent<Text>();
            newMessage.textObject.text = newMessage.text;
            messageList.Add(newMessage);
        }
        
        System.IO.File.WriteAllText(@"texts.txt", string.Empty);
    }
    }

    [System.Serializable]
    public class Message {
        public string text;
        public Text textObject;
    }

    
}
