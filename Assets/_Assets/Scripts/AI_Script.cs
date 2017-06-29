using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MENU
{
    Chips,
    HotDog,
    Taco,
    Burger,
    Pizza,
}

public enum STATE
{
    GoingToRestaurant,
    Ordering,
    Eating,
    LeavingRestaurant,
    Kidnapped,
    Dead,
}

public class AI_Script : MonoBehaviour {

    public float timeToWaitForFood;
    public float timeToEat;

    public float pointBuffer;
    public Transform[] toCounter;
    public Transform[] fromCounter;
    private Queue<Vector2> activePath;
    private NavMeshAgent agent;

    [HideInInspector]
    public STATE state;
    [HideInInspector]
    public bool bFat;

    [HideInInspector]
    public MENU order;

    private float counter;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        activePath = new Queue<Vector2>();
        state = STATE.GoingToRestaurant;
        SeekToCounter();
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;

        print(state);

        StateMachine();
        GetComponentInChildren<Animator>().SetInteger("State", (int)state);
	}

    public void AddWaypoint(Vector2 waypoint)
    {
        activePath.Enqueue(waypoint);
    }

    public void SeekToCounter()
    {
        for (int i = 0; i < toCounter.Length; i++)
        {
            Vector2 v = new Vector2(toCounter[i].position.x, toCounter[i].position.z);
            activePath.Enqueue(v);
        }
    }

    public void SeekFromCounter()
    {
        for (int i = 0; i < fromCounter.Length; i++)
        {
            Vector2 v = new Vector2(fromCounter[i].position.x, fromCounter[i].position.z);
            activePath.Enqueue(v);
        }
    }

    public void ClearActivePath()
    {
        while (activePath.Count > 0)
        {
            activePath.Dequeue();
        }
    }

    public void TraversePath()
    {
        if (activePath.Count > 0)
        {
            Vector2 point = activePath.Peek();
            Vector3 target = new Vector3(point.x, this.transform.position.y, point.y);
            agent.SetDestination(target);

            if (Vector3.Distance(transform.position, target) < pointBuffer)
            {
                activePath.Dequeue();
            }
        }

        if (activePath.Count == 0 && agent.destination.x == toCounter[toCounter.Length - 1].position.x && agent.destination.z == toCounter[toCounter.Length - 1].position.z)
        {
            if (state == STATE.GoingToRestaurant)
            {
                state = STATE.Ordering;
                counter = timeToWaitForFood;
            } 
            
        }                     
    }

    void StateMachine()
    {
        switch (state)
        {
            case STATE.GoingToRestaurant:
                TraversePath();
                break;
            case STATE.Ordering:
                OrderFood();
                WaitForFood();
                break;
            case STATE.Eating:
                WaitToEat();
                break;
            case STATE.LeavingRestaurant:
                TraversePath();
                break;
            case STATE.Kidnapped:
                break;
            case STATE.Dead:
                break;
            default:
                break;
        }
    }

    void WaitForFood()
    {
        if (counter <= 0)
        {
            state = STATE.Eating;
            counter = timeToEat;
        }
    }

    void WaitToEat()
    {
        if (counter <= 0)
        {
            SeekFromCounter();
            state = STATE.LeavingRestaurant;
        }
    }

    public void OrderFood()
    {
        int rand = Random.Range(0, 5);

        order = (MENU)rand;
    }

    public void GetKidnapped()
    {

    }

}
