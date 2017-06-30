using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeButton : MonoBehaviour {
    public int recipe;
    public int fingersCost;
    public int toesCost;
    public int eyeballsCost;
    public int earsCost;
    public int heartCost;
    public int liverCost;
    public int lardCost;
    public GameObject dude;

    int[] recipeCost;

    void Start() {
        recipeCost = new int[] { fingersCost, toesCost, eyeballsCost, earsCost, heartCost, liverCost, lardCost };
    }

    public void onClick() {
        dude.GetComponent<CyborgController>().MakeFood(recipeCost, recipe);
    }
}
