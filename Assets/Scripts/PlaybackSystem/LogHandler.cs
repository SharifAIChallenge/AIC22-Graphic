using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    private StreamReader reader;
    private List<string> logLines = new();
    
    private int _currentLine;

    public void Setup()
    {
        //reader = new StreamReader(Config.GamePath + "/log.txt");
        reader = new StreamReader(Config.LogFilePath);

        if (Config.Cached)
        {
            while (reader.ReadLine() is { } line)
            {
                logLines.Add(line);
            }
            reader.Close();
            _currentLine = 0;
        }
    }

    public string GetNextLine()
    {
        if (Config.Cached)
        {
            if (_currentLine >= logLines.Count)
            {
                return null;
            }
            
            return logLines[_currentLine++];
        }
        else
        {
            if (reader.EndOfStream)
            {
                return null;
            }
            return reader.ReadLine();
        }
    }
    
    public int GetCurrentLine()
    {
        return _currentLine;
    }
    
    public void SetCurrentLine(int line)
    {
        _currentLine = line;
    }
}

