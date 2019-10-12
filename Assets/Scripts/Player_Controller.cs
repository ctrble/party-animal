using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {
  public int moveSpeed;
  public int lookSpeed;
  public int jumpSpeed;
  public Rigidbody entityRigidbody;
  private Vector3 moveInput;
  private Vector3 rotateInput;
  private bool jumpInput;
  public bool canJump;
  private bool doJump;
  public float groundedTolerance;
  private float yaw;
  private float pitch;

  void OnEnable() {
    if (entityRigidbody == null) {
      entityRigidbody = GetComponent<Rigidbody>();
    }

    canJump = true;
    doJump = false;
  }

  void Update() {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = Input.GetAxis("Mouse Y");
    moveInput = new Vector3(horizontal, 0, vertical);
    rotateInput = new Vector3(mouseX, mouseY, 0);

    jumpInput = Input.GetButtonDown("Jump");
    if (transform.position.y <= groundedTolerance) {
      canJump = true;
    }
    else {
      canJump = false;
    }
    if (canJump && jumpInput) {
      doJump = true;
      canJump = false;
    }
  }

  void FixedUpdate() {
    Move(moveInput);
    Rotate(rotateInput);
  }

  void Move(Vector3 direction) {
    Vector3 forward = transform.forward * direction.z * moveSpeed * Time.fixedDeltaTime;
    Vector3 strafe = transform.right * direction.x * (moveSpeed * 0.5f) * Time.fixedDeltaTime;
    Vector3 newPosition = transform.position + forward + strafe;
    entityRigidbody.MovePosition(newPosition);

    if (doJump) {
      entityRigidbody.AddForce(Vector3.up * jumpSpeed);
      doJump = false;
    }
  }

  void Rotate(Vector3 direction) {
    yaw += direction.x * lookSpeed * Time.fixedDeltaTime;
    pitch += direction.y * (lookSpeed * 0.5f) * Time.fixedDeltaTime;
    Quaternion deltaRotation = Quaternion.Euler(pitch, yaw, 0.0f);
    entityRigidbody.MoveRotation(deltaRotation);
  }
}
