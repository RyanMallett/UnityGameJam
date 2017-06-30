using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MENU
{
    None,
    Chips,
    HotDog,
    Taco,
    Burger,
    Pizza,
}

public struct Seat
{
    public Seat( int i, Sprite s)
    {
        id = i;
        order = MENU.None;
        timeLeft = 0;
        obj = new GameObject();
        obj.AddComponent<SpriteRenderer>();
        sprite = s;
        obj.GetComponent<SpriteRenderer>().sprite = sprite;
        obj.name = "Stool" + id.ToString();
        occupied = false;

        customer = null;
    }

    public int id;
    public bool occupied;
    public MENU order;
    public float timeLeft;
    public GameObject obj;
    public Sprite sprite;

    public AI_Script customer;
}

public class Counter_Script : MonoBehaviour {

    public Sprite stoolSprite;
    public Seat[] seats;

	// Use this for initialization
	void Start () {

        seats = new Seat[4] { new Seat(1, stoolSprite), new Seat(2, stoolSprite), new Seat(3, stoolSprite), new Seat(4, stoolSprite) };

        float counterWidth = GetComponent<BoxCollider>().bounds.extents.x;

        for (int i = 0; i < seats.Length; i++)
        {
            float xPos = transform.position.x - counterWidth + ((counterWidth * 2) / 5) * i + ((counterWidth * 2) / 5);

            seats[i].obj.transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

            seats[i].obj.transform.Rotate(Vector3.right, 90);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GotFood(int seatNum)
    {
        if (seats[seatNum].occupied)
        {
            seats[seatNum].customer.GotFood();
        }       
    }
}
