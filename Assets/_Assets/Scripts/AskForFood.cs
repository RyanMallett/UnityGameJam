using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskForFood : MonoBehaviour {

    public Sprite chipSprite;
    public Sprite hotDogSprite;
    public Sprite tacoSprite;
    public Sprite burgerSprite;
    public Sprite pizzaSprite;

    public GameObject child;

	// Use this for initialization
	void Start () {

        child = new GameObject();

        child.transform.parent = GetComponentInChildren<SpriteRenderer>().transform;

        child.AddComponent<SpriteRenderer>();

        child.GetComponent<SpriteRenderer>().transform.position = new Vector3(GetComponent<AI_Script>().transform.position.x, GetComponent<AI_Script>().transform.position.y + 1f, GetComponent<AI_Script>().transform.position.z);
        child.GetComponent<SpriteRenderer>().transform.Rotate(new Vector3(0, 1, 0), -90);
	}
	
	// Update is called once per frame
	void Update () {
        switch (GetComponent<AI_Script>().order)
        {
            case MENU.None:
                child.GetComponent<SpriteRenderer>().sprite = null;
                break;
            case MENU.Chips:
                child.GetComponent<SpriteRenderer>().sprite = chipSprite;
                break;
            case MENU.HotDog:
                child.GetComponent<SpriteRenderer>().sprite = hotDogSprite;
                break;
            case MENU.Taco:
                child.GetComponent<SpriteRenderer>().sprite = tacoSprite;
                break;
            case MENU.Burger:
                child.GetComponent<SpriteRenderer>().sprite = burgerSprite;
                break;
            case MENU.Pizza:
                child.GetComponent<SpriteRenderer>().sprite = pizzaSprite;
                break;
            default:
                break;
        }
    }
}
