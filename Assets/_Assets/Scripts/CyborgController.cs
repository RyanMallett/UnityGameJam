using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyborgController : MonoBehaviour {
   
    public float moveSpeed = 2.0f;
    public float rotateSpeed = 0.25f;

    public Canvas hud;
    public GameObject notEnoughIngredient;

    public Sprite[] movementSprites;
    public Sprite[] foodSprites;

    public Counter_Script counterScript;

    SpriteRenderer foodRenderer;
    SpriteRenderer spriteRenderer;
    int foodBeingHeld = 0;

    GameObject inventoryPanel;
    Text notEnoughIngredientText;

    bool inTrigger = false;
    bool draggingPlayer = false;
    string triggerObjectTag;
    public string currentRoom;
    GameObject currentPerson = null;

    bool itemMade = false;
    bool inMenu = false;

    public int collectedFingers = 0;
    public int collectedToes = 0;
    public int collectedEyeballs = 0;
    public int collectedEars = 0;
    public int collectedHeart = 0;
    public int collectedLiver = 0;
    public int collectedLard = 0;

    List<int> collectedBodyParts; 
    public List<int> CollectedBodyParts { get { return collectedBodyParts; } }

    float nextSprite = 1;

    public int totalMoneyEarned = 0;

    // Use this for initialization
    void Start () {
		collectedBodyParts = new List<int> { collectedFingers, collectedToes, collectedEyeballs, collectedEars, collectedHeart, collectedLiver, collectedLard };
        inventoryPanel = hud.gameObject.transform.Find("CookingMenu").gameObject;
        notEnoughIngredientText = notEnoughIngredient.GetComponentInChildren<Text>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        foodRenderer = transform.FindChild("Food").gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movePos = new Vector3(horizontal, 0, vertical) + transform.position;
        //(movePos.x)

        transform.position = Vector3.Lerp(transform.position, movePos, moveSpeed * Time.deltaTime);
        Quaternion oldRot = transform.rotation;
        transform.LookAt(movePos);
        Quaternion newRot = transform.rotation;
        transform.rotation = Quaternion.Lerp(oldRot, newRot, rotateSpeed);

        nextSprite += Time.deltaTime * moveSpeed * 1.75f;

        if (movePos == transform.position)
            nextSprite = 0;
        if (nextSprite >= 3)
            nextSprite = 1;

        spriteRenderer.sprite = movementSprites[(int)nextSprite];

        if (Input.GetButtonDown("Interact") && inTrigger) {
            switch (triggerObjectTag) {
                case "Person":
                    HandleCharacter();
                    break;
                case "CookingBench":
                    HandleCooking();
                    break;
                case "Bin":
                    ThrowOutFood();
                    break;
                case "Counter":
                    GiveFood();
                    break;
                default:
                    break;
            }
        }

        if (draggingPlayer)
            currentPerson.transform.position = transform.position + new Vector3(0, 0, 0.5f);
	}

    void GiveFood() {
        for (int i = 0; i < counterScript.seats.Length; i++) {
            if ((int)counterScript.seats[i].order == foodBeingHeld) {
                foodRenderer.sprite = new Sprite();
                counterScript.GotFood(i);
                switch (foodBeingHeld) {
                    case 1:
                        totalMoneyEarned += 5;
                        break;
                    case 2:
                        totalMoneyEarned += 8;
                        break;
                    case 3:
                        totalMoneyEarned += 15;
                        break;
                    case 4:
                        totalMoneyEarned += 20;
                        break;
                    case 5:
                        totalMoneyEarned += 25;
                        break;
                    default:
                        break;
                }
                foodBeingHeld = 0;
                return;
            }
        }
    }

    void ThrowOutFood() {
        foodRenderer.sprite = new Sprite();
    }

    void HandleCooking() {
        inMenu = true;  
        inventoryPanel.GetComponent<Animator>().Play("Move");
    }

    public void MakeFood(int[] cost, int recipeName) {
        //bool ableToMake = true;
        string[] lowIngredient = { "Fingers", "Toes", "Eyeballs", "Ears", "Heart", "Liver", "Lard"};
        for (int i = 0; i < collectedBodyParts.Count; i++) {
            if (collectedBodyParts[i] >= cost[i]) {
                //ableToMake = true;
            }
            else {
                // ableToMake = false;
                StartCoroutine(ActivateMessage());
                notEnoughIngredientText.text = "Not Enough " + lowIngredient[i] + ": " + collectedBodyParts[i].ToString() + " of " + cost[i].ToString() + " collected";
                return;
            }            
        }
            itemMade = true;
            inMenu = false;
            inventoryPanel.GetComponent<Animator>().Play("MoveDown");
        for (int i = 0; i < collectedBodyParts.Count; i++) {
            collectedBodyParts[i] -= cost[i];
        }

        foodRenderer.sprite = foodSprites[recipeName];
        foodBeingHeld = recipeName + 1;
    }

    IEnumerator ActivateMessage() {
        notEnoughIngredient.SetActive(true);
        while (true) {
            yield return new WaitForSeconds(2);
            notEnoughIngredient.SetActive(false);
        }
    }

    void HandleCharacter() {        
        if (draggingPlayer) {
            draggingPlayer = false;
            currentPerson.transform.parent = null;
            if (currentRoom == "KillRoom") {
                currentPerson.GetComponent<PersonController>().ChangeSprite("Dead");
                HarvestParts();
                return;
            }
        currentPerson.GetComponent<PersonController>().ChangeSprite("Normal");
        }
        else {
            currentPerson.transform.parent = transform;
            currentPerson.GetComponent<PersonController>().ChangeSprite("Drag");
            currentPerson.GetComponent<AI_Script>().Kidnapped();
            draggingPlayer = true;
        }

    }

    void HarvestParts() {
        collectedBodyParts[0] += 10;
        collectedBodyParts[1] += 10;
        collectedBodyParts[2] += 2;
        collectedBodyParts[3] += 2;
        collectedBodyParts[4] += 1;
        collectedBodyParts[5] += 1;
        collectedBodyParts[6] += 3;

        collectedFingers = collectedBodyParts[0];
        collectedToes = collectedBodyParts[1];
        collectedEyeballs = collectedBodyParts[2];
        collectedEars = collectedBodyParts[3];
        collectedHeart = collectedBodyParts[4];
        collectedLiver = collectedBodyParts[5];
        collectedLard = collectedBodyParts[6];
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag.IndexOf("Room") != -1) {
            currentRoom = col.gameObject.tag;
        }
        else {
            inTrigger = true;
            triggerObjectTag = col.gameObject.tag;
            if (triggerObjectTag == "Counter") {
                triggerObjectTag = "Counter";
                return;
            }
            if (triggerObjectTag == "Person" && !draggingPlayer) {
                currentPerson = col.gameObject;
            }
        }        
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.IndexOf("Room") != -1) {
        }
        else {
            inTrigger = false;
            if (draggingPlayer)
                inTrigger = true;
            if (triggerObjectTag == "Person" && !draggingPlayer) {
                currentPerson = null;
            }
            else if (triggerObjectTag == "CookingBench") {
                if (!itemMade && inMenu)
                    inventoryPanel.GetComponent<Animator>().Play("MoveDown");
                itemMade = false;
                inMenu = false;
            }
            triggerObjectTag = "None";
        }
    }    
}
