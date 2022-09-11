using DG.Tweening;
using GraphCreator;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Thief : Agent
{
    private bool _isCaught;
    private bool _isVisible;

    [SerializeField] private SpriteRenderer visualSR;
    
    public bool IsCaught
    {
        get => _isCaught;
        set
        {
            _isCaught = value;
            UpdateViewState();
        }
    }
    
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            visualSR.DOFade(value ? 1 : 0.6f, 0.1f);
            _isVisible = value;
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
    
    public override void UpdateViewState()
    {
        if(team == Team.FIRST)
        {
            visualGO.SetActive(Config.firstTeamThiefView && !IsCaught);
        }
        else
        {
            visualGO.SetActive(Config.secondTeamThiefView && !IsCaught);
        }
    }
}