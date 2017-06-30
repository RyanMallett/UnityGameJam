using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Spawner : MonoBehaviour {

    public Transform[] toCounter;
    public Transform[] fromCounter;

    public GameObject prefab;

    public GameObject seats;

    //public GameObject anim;

    public float min;
    public float max;

    private float counter;

    // Use this for initialization
    void Start () {
        //SpawnPrefab();
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;

        if (counter <= 0) SpawnPrefab();
	}

    void SpawnPrefab()
    {
        GameObject ai = Instantiate(prefab);

        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(transform.position, out closestHit, 500, 1))
        {
            ai.transform.position = closestHit.position;
            ai.AddComponent<NavMeshAgent>();
        }

        ai.GetComponent<AI_Script>().toCounter = toCounter;
        ai.GetComponent<AI_Script>().fromCounter = fromCounter;

        ai.GetComponent<AI_Script>().seats = seats;

        ai.GetComponentInChildren<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;

        counter = Random.Range(min, max);
    }
}
