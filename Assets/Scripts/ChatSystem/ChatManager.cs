using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class ChatManager : Cacheable
{
    string[] lines;

    public List<Message>[] messageList =
        { new List<Message>(), new List<Message>(), new List<Message>(), new List<Message>() };

    public GameObject textObject;
    public GameObject[] chatPanels = new GameObject[4];

    //0: FIRST team, THIEF
    //1: FIRST team, POLICE
    //2: SECOND team, THIEF
    //3: SECOND team, POLICE

    public void UpdateChat(string team, string type, string agentId, string text)
    {
        string line = agentId + ": " + text;
        Message newMessage = new Message();
        newMessage.text = line;
        GameObject newText;
        if (team.Equals("FIRST"))
        {
            newText = Instantiate(textObject,
                type.Equals("THIEF") ? (chatPanels[0]).transform : (chatPanels[1]).transform);
        }
        else
        {
            newText = Instantiate(textObject,
                type.Equals("THIEF") ? (chatPanels[2]).transform : (chatPanels[3]).transform);
        }

        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        if (team.Equals("FIRST"))
        {
            if (type.Equals("THIEF")) (messageList[0]).Add(newMessage);
            else (messageList[1]).Add(newMessage);
        }
        else
        {
            if (type.Equals("THIEF")) (messageList[2]).Add(newMessage);
            else (messageList[3]).Add(newMessage);
        }
    }


    [Serializable]
    public class Message
    {
        public string text;
        [JsonIgnore] public TMP_Text textObject;
    }

    public override void SaveState()
    {
        var json = JsonConvert.SerializeObject(messageList, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        history.Add(json);
    }

    public override void LoadState(int index)
    {
        var json = history[index];
        var loadedMessageList = JsonConvert.DeserializeObject<List<Message>[]>(json);

        for (int i = 0; i < 4; i++)
        {
            var current = messageList[i];
            var loaded = loadedMessageList[i];

            var min = Mathf.Min(current.Count, loaded.Count);

            if (loaded.Count > current.Count)
            {
                for (int j = min; j < loaded.Count; j++)
                {
                    Debug.Log(i + ", " + j + ", " + loaded[j].text);
                    var newText = Instantiate(textObject, (chatPanels[i]).transform);
                    var newMessage = new Message
                    {
                        text = loaded[j].text,
                        textObject = newText.GetComponent<TMP_Text>()
                    };
                    newMessage.textObject.text = newMessage.text;
                    current.Add(newMessage);
                }
            }
            else if (loaded.Count < current.Count)
            {
                for (int j = min; j < current.Count; j++)
                {
                    Destroy(current[j].textObject.gameObject);
                }

                current.RemoveRange(min, current.Count - loaded.Count);
            }

            for (int j = 0; j < min; j++)
            {
                current[j].text = loaded[j].text;
                current[j].textObject.text = loaded[j].text;
            }
        }
    }
}