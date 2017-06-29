using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryText : MonoBehaviour {
    public int modifier;

    int value;
    Text text;
    CyborgController cyController;

    Button a;
    
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        cyController = GameObject.FindGameObjectWithTag("Player").GetComponent<CyborgController>();
       // a.
	}
	
	// Update is called once per frame
	void Update () {
        value = cyController.CollectedBodyParts[modifier];
        text.text = value.ToString();
	}
}
