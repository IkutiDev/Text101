using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] private Text textComponent;

    [SerializeField] private State startingState;

    [SerializeField] private bool hasGun;

    [SerializeField] private bool hasLoot;

    [SerializeField] private int health=3;
    public int enemyHealth;

    private State currentState;

    //private readonly string[] daysOfWeek = { "Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
    // Start is called before the first frame update
    void Start()
    {
        currentState = startingState;
        textComponent.text = currentState.GetStateStory();
    }
    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        var nextStates = currentState.GetNextStates();
        if (currentState.GetStateType()==StateType.FightGuard || currentState.GetStateType() ==StateType.FightLeader)
        {
            // Fight System
        }
        else if (currentState.GetStateType() == StateType.Hit)
        {
            
        }
        else if (currentState.GetStateType() == StateType.Miss)
        {

        }
        else if (currentState.GetStateType()==StateType.TimeLimit)
        {
            //coroutine and inputs
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentState = nextStates[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentState = nextStates[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentState = nextStates[2];
            }

            textComponent.text = currentState.GetStateStory();
            ManageStateType();
        }
    }

    private void ManageStateType()
    {
        switch (currentState.GetStateType())
        {
            case StateType.GainGun:
            {
                hasGun = true;
                break;
            }

            case StateType.GainHealth:
            {
                health = 5;
                break;
            }

            case StateType.GainLoot:
            {
                hasLoot = true;
                break;
            }
            case StateType.Normal:
            {
                break;
            }
            default:
            {
                Debug.LogError("Unknown State Type!");
                break;
            }
        }
    }
}
