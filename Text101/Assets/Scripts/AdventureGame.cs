using System.Collections;
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
    private IEnumerator coroutine;
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
        else if (currentState.GetStateType() == StateType.Branching)
        {
            if (currentState.GetNextStates().Length == 2)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentState = hasGun ? nextStates[1] : nextStates[0];
                }
            }
            else if (currentState.GetNextStates().Length == 3)
            {
                if (!Input.GetKeyDown(KeyCode.Alpha1)) return;
                if (hasGun)
                {
                    currentState = nextStates[1];
                }
                else if (hasLoot)
                {
                    currentState = nextStates[2];
                }
                else
                {
                    currentState = nextStates[0];
                }
            }
        }
        else if (currentState.GetStateType()==StateType.TimeLimit)
        {
            currentState.stateType = StateType.Normal;
            print("Starting " + Time.time + " seconds");
            coroutine = WaitAndSelect(10.0f,currentState);
            StartCoroutine(coroutine);
            print("Coroutine started");
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

            case StateType.Start:
            {
                hasGun = false;
                hasLoot = false;
                health = 3;
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
    private IEnumerator WaitAndSelect(float waitTime, State currState)
    {
        yield return new WaitForSeconds(waitTime);
        if (currentState == currState)
        {
            currentState = currentState.GetNextStates()[0];
        }

        currState.stateType = StateType.TimeLimit;
        print("Coroutine ended: " + Time.time + " seconds");
    }
}
