using System.IO;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    private StreamReader reader;

    void Start()
    {
        reader = new StreamReader("Assets/log.txt");
    }

    public string GetNextLine()
    {
       return reader.ReadLine();
    }
}

