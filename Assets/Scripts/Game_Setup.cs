using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Setup : MonoBehaviour {

  public GameObject partiers;
  public GameObject partyPerson;
  public int partyPeople;


  void OnEnable() {
    Cursor.visible = false;

    for (int i = 0; i < partyPeople; i++) {
      GameObject partier = Instantiate(partyPerson, Vector3.zero, Quaternion.identity);
      partier.transform.parent = partiers.transform;
    }
  }
}
