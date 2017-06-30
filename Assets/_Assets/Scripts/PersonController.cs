using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour {

    public Sprite normalSprite;
    public Sprite draggingSprite;
    public Sprite deadSprite;

    bool isDead;
    public bool IsDead {
        get { return isDead; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSprite(string spriteName) {
        Sprite newSprite = null;
        switch (spriteName) {
            case "Drag":
                newSprite = draggingSprite;
                break;
            case "Dead":
                newSprite = deadSprite;
                isDead = true;
                GetComponent<BoxCollider>().center = new Vector3(0, -10, 0);
                break;
            case "Normal":
                newSprite = normalSprite;
                break;
            default:
                Debug.LogWarning("No Current Sprite with that name exists");
                break;
        }
        GetComponentInChildren<SpriteRenderer>().sprite = newSprite;
    }
}
