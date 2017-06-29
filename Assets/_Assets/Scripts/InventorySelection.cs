using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySelection : MonoBehaviour {

    public bool Selected = false;

    CookingController cookingController;
    int modifier;

	// Use this for initialization
	void Start () {
        cookingController = GetComponentInParent<CookingController>();
        modifier = GetComponentInChildren<UIInventoryText>().modifier;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPart() {
        //cookingController.add
    }
}
