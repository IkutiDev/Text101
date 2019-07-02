using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(10,14)] [SerializeField] private string storyText;
    [SerializeField] private State[] nextStates;
    [SerializeField] private StateType stateType;
    public string GetStateStory()
    {
        return storyText;
    }

    public State[] GetNextStates()
    {
        return nextStates;
    }

    public StateType GetStateType()
    {
        return stateType;
    }
}