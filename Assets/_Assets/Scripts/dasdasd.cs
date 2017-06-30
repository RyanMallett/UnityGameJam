using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dasdasd : MonoBehaviour {

    public CyborgController controller;

    Text text;

    void Start() {
        text = GetComponent<Text>();
    }

    void Update() {
        text.text = controller.totalMoneyEarned.ToString();
    }
}
