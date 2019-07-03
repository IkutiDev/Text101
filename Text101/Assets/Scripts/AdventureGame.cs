using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] private Text textComponent;

    [SerializeField] private Text healthTextComponent;

    [SerializeField] private State startingState;

    [SerializeField] private State gameOverState;

    [SerializeField] private bool hasGun;

    [SerializeField] private bool hasLoot;

    [SerializeField] private int health=3;
    private int guardHealth=2;
    private int leaderHealth = 5;
    private int leaderCounter = 1;

    private State currentState;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        currentState = startingState;
        textComponent.text = currentState.GetStateStory();
        healthTextComponent.text = health.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        var nextStates = currentState.GetNextStates();
        if (health==0)
        {
            currentState = gameOverState;
            textComponent.text = currentState.GetStateStory();
            health = -1;
        }
        else if (currentState.GetStateType()==StateType.FightGuard || currentState.GetStateType() ==StateType.FightLeader)
        {
            textComponent.text = currentState.GetStateStory();
            if (currentState.GetStateType() == StateType.FightGuard)
            {
                if (guardHealth <= 0)
                {
                    currentState = nextStates[3];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    currentState = nextStates[0];
                    guardHealth--;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    currentState = nextStates[1];
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    currentState = nextStates[2];
                    health--;
                    healthTextComponent.text = health.ToString();
                }
            }
            else if(currentState.GetStateType() == StateType.FightLeader)
            {
                BossFight();
            }
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

    private void BossFight()
    {
        var nextStates = currentState.GetNextStates();
        if (leaderHealth <= 0)
        {
            currentState = nextStates[9];
        }
        else if (leaderCounter==1 || leaderCounter == 3)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentState = nextStates[0];
                leaderCounter++;

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentState = nextStates[5];
                health--;
                healthTextComponent.text = health.ToString();
                leaderCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentState = nextStates[7];
                leaderHealth--;
                leaderCounter++;
            }

            
        }
        else if (leaderCounter == 2 || leaderCounter == 4)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentState = nextStates[2];
                leaderHealth--;
                leaderCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentState = nextStates[4];
                leaderCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentState = nextStates[6];
                health--;
                healthTextComponent.text = health.ToString();
                leaderCounter++;
            }
        }
        else if (leaderCounter == 5)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentState = nextStates[1];
                health--;
                healthTextComponent.text = health.ToString();
                leaderCounter = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentState = nextStates[3];
                leaderHealth--;
                leaderCounter = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentState = nextStates[8];
                leaderCounter = 1;
            }

            
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
                healthTextComponent.text = health.ToString();
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
                healthTextComponent.text = health.ToString();
                guardHealth = 2;
                leaderHealth = 5;
                leaderCounter = 1;
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
