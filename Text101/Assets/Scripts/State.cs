using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(10,14)] [SerializeField] private string storyText;

    public string GetStateStory()
    {
        return storyText;
    }

}