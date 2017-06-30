using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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

    public GameObject seats;
    public float timeToWaitForFood;
    public float timeToEat;

    private int seatId;

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

        seatId = -1;
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
                // Sit in any available seats, otherwise walk out

                state = STATE.Ordering;
                counter = timeToWaitForFood;
            }

        }

        if (activePath.Count == 0 && agent.destination.x == fromCounter[fromCounter.Length - 1].position.x && agent.destination.z == fromCounter[fromCounter.Length - 1].position.z)
        {
            if (state == STATE.LeavingRestaurant)
            {
                Destroy(this.gameObject);
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
            // Display Happy or Sad
            // Also where diarrhoea check would go

            SeekFromCounter();
            agent.enabled = true;
            state = STATE.LeavingRestaurant;

            seats.GetComponent<Counter_Script>().seats[seatId].occupied = false;
        }
    }

    public void OrderFood()
    {

        for (int i = 0; i < seats.GetComponent<Counter_Script>().seats.Length; i++)
        {
            if (!seats.GetComponent<Counter_Script>().seats[i].occupied && seatId == -1)
            {
                seats.GetComponent<Counter_Script>().seats[i].occupied = true;

                int rand = Random.Range(0, 5);
                order = (MENU)rand;

                seats.GetComponent<Counter_Script>().seats[i].order = order;
                seats.GetComponent<Counter_Script>().seats[i].timeLeft = timeToWaitForFood;

                seatId = i;

                agent.enabled = false;

                transform.position = new Vector3(seats.GetComponent<Counter_Script>().seats[i].obj.transform.position.x, transform.position.y, seats.GetComponent<Counter_Script>().seats[i].obj.transform.position.z);
                transform.LookAt(new Vector3(transform.position.x, transform.position.y, -1));

                return;
            }
        }

        if (seatId == -1 )
        {
            SeekFromCounter();
            agent.enabled = true;
            state = STATE.LeavingRestaurant;
        }

        
    }

    public void Kidnapped()
    {
        agent.enabled = false;
        GetComponentInChildren<Animator>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "AI_Killer" && state == STATE.LeavingRestaurant)
        {
            Destroy(this.gameObject);
        }
    }
}
