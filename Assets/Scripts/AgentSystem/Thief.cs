using GraphCreator;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Thief : Agent
{
    private bool _isCaught;

    public bool IsCaught
    {
        get => _isCaught;
        set
        {
            visualGO.SetActive(!value);
            _isCaught = value;
        }
    }

    private void Awake()
    {
        type = AgentType.THIEF;
        IsCaught = false;
    }

    public void Caught()
    {
        IsCaught = true;
    }
    
    public override JObject GetJsonObject()
    {
        var o = base.GetJsonObject();
        o.Add("caught", IsCaught);

        return o;
    }
    
    public override void LoadFromJson(JToken j)
    {
        base.LoadFromJson(j);
        var o = JObject.Parse(j.ToString());
        IsCaught = (bool) o?["caught"];
    }
}