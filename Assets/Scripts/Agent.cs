using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    public AgentType type;
    public int money;

    private int _currentNode;

    public void Move(int from, int to)
    {
        
    }
}

public enum AgentType
{
    COP,
    THIEF
}
