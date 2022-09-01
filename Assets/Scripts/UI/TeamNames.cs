using System;
using TMPro;
using UnityEngine;

public class TeamNames : MonoBehaviour
{
    [SerializeField] private TMP_Text firstTeamNameText;
    [SerializeField] private TMP_Text secondTeamNameText;
    
    public void SetTeamNames(string json)
    {
        json = json.Substring(1, json.Length - 2);
        try
        {
            var parsed = JsonUtility.FromJson<TeamNamesJson>(json);
            firstTeamNameText.text = parsed.first;
            secondTeamNameText.text = parsed.second;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            firstTeamNameText.text = "FIRST";
            secondTeamNameText.text = "SECOND";
        }
    }
    
    [Serializable]
    private class TeamNamesJson
    {
        public string first;
        public string second;
    }
}