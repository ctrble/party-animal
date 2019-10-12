using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Person_Controller : MonoBehaviour, IInteractable {

  public SphereCollider interactionCollider;
  public NavMeshAgent navAgent;
  private float range = 50f;
  private float tolerance = 0.5f;
  public float hangTimer;
  public float maxHangTimer = 120f;
  public int friendlyFeelings;



  void OnEnable() {
    if (navAgent == null) {
      navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    Vector3 randomPosition;
    if (RandomPoint(transform.position, range, out randomPosition)) {
      Debug.DrawRay(randomPosition, Vector3.up, Color.red, 1f);
      navAgent.Warp(randomPosition);
    }

    hangTimer = Random.Range(0.5f, 5f);

    friendlyFeelings = 0;
  }

  void Update() {

    if (CanInteract()) {
      Debug.DrawRay(transform.position, Vector3.up, Color.green, 2.5f);

      if (Input.GetButtonDown("Interact")) {
        Interact();
      }
    }

    Wander();
  }

  void Wander() {
    hangTimer -= Time.deltaTime;
    if (hangTimer <= 0f) {
      if (navAgent.remainingDistance <= navAgent.stoppingDistance + tolerance) {
        Vector3 randomPosition;
        if (RandomPoint(transform.position, range, out randomPosition)) {
          Debug.DrawRay(randomPosition, Vector3.up, Color.blue, 1f);
          navAgent.SetDestination(randomPosition);
        }
      }
      hangTimer = Random.Range(0.5f, maxHangTimer);
    }
  }

  bool RandomPoint(Vector3 center, float range, out Vector3 result) {
    for (int i = 0; i < 30; i++) {
      int filterMask = 1 << NavMesh.GetAreaFromName("Walkable");
      Vector3 randomPoint = center + Random.insideUnitSphere * range;
      randomPoint.y = 0;
      NavMeshHit hit;
      if (NavMesh.SamplePosition(randomPoint, out hit, 4f, filterMask)) {
        result = hit.position;
        return true;
      }
    }
    result = Vector3.zero; // set to the entryway
    return false;
  }

  public void Interact() {
    Debug.Log("PET PET PET PET");
    friendlyFeelings++;

    if (friendlyFeelings >= 10) {
      Debug.Log("THIS IS THE BEST DOGGO");
    }
  }

  bool CanInteract() {
    bool canInteract = false;
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionCollider.radius);
    for (int i = 0; i < hitColliders.Length; i++) {
      if (hitColliders[i].transform.root.CompareTag("Player")) {
        canInteract = true;
      }
    }
    return canInteract;
  }
}
