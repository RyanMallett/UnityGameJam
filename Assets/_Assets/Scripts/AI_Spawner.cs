using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Spawner : MonoBehaviour {

    public Transform[] toCounter;
    public Transform[] fromCounter;

    public GameObject prefab;

    public GameObject anim;

    public float min;
    public float max;

    private float counter;

    // Use this for initialization
    void Start () {
        SpawnPrefab();
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;

        //if (counter <= 0) SpawnPrefab();
	}

    void SpawnPrefab()
    {
        GameObject ai = Instantiate(prefab);

        ai.GetComponent<AI_Script>().toCounter = toCounter;
        ai.GetComponent<AI_Script>().fromCounter = fromCounter;

        ai.GetComponentInChildren<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;

        ai.transform.position = this.transform.position;

        counter = Random.Range(min, max);
    }
}
