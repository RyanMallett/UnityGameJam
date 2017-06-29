using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyborgController : MonoBehaviour {
   
    public float moveSpeed = 1.0f;

    public Canvas hud;
    public GameObject notEnoughIngredient;
    GameObject inventoryPanel;
    Text notEnoughIngredientText;

    bool inTrigger = false;
    bool draggingPlayer = false;
    string triggerObjectTag;
    public string currentRoom;
    GameObject currentPerson = null;




    int collectedFingers = 0;
    int collectedToes = 0;
    int collectedEyeballs = 0;
    int collectedEars = 0;
    int collectedHeart = 0;
    int collectedLiver = 0;
    int collectedLard = 0;

    List<int> collectedBodyParts; 
    public List<int> CollectedBodyParts { get { return collectedBodyParts; } }

    // Use this for initialization
    void Start () {
		collectedBodyParts = new List<int> { collectedFingers, collectedToes, collectedEyeballs, collectedEars, collectedHeart, collectedLiver, collectedLard };
        inventoryPanel = hud.gameObject.transform.Find("CookingMenu").gameObject;
        notEnoughIngredientText = notEnoughIngredient.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movePos = new Vector3(horizontal, 0, vertical) + transform.position;

        transform.position = Vector3.Lerp(transform.position, movePos, moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Interact") && inTrigger) {
            switch (triggerObjectTag) {
                case "Person":
                    HandleCharacter();
                    break;
                case "CookingBench":
                    HandleCooking();
                    break;
                default:
                    break;
            }
        }
	}

    void HandleCooking() {        
        inventoryPanel.GetComponent<Animator>().SetFloat("Speed", 1.25f);
        inventoryPanel.GetComponent<Animator>().Play("Move");
    }

    public void MakeFood(int[] cost) {
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

        for (int i = 0; i < collectedBodyParts.Count; i++) {
            collectedBodyParts[i] -= cost[i];
        }
    }

    IEnumerator ActivateMessage() {
        notEnoughIngredient.SetActive(true);
        while (true) {
            yield return new WaitForSeconds(1);
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
            draggingPlayer = true;
        }

    }

    void HarvestParts() {
        for (int i = 0; i < collectedBodyParts.Count; i++) {
            collectedBodyParts[i]++;
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag.IndexOf("Room") != -1) {
            currentRoom = col.gameObject.tag;
        }
        else {
            inTrigger = true;
            triggerObjectTag = col.gameObject.tag;
            if (triggerObjectTag == "Person") {
                currentPerson = col.gameObject;
            }
        }        
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag.IndexOf("Room") != -1) {
        }
        else {
            inTrigger = false;
            if (triggerObjectTag == "Person") {
                currentPerson = null;
            }
            else if (triggerObjectTag == "CookingBench") {
                inventoryPanel.GetComponent<Animator>().SetFloat("Speed", -1);
                inventoryPanel.GetComponent<Animator>().Play("Move");
            }
        }
    }    
}
