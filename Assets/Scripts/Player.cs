using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	public float moveSpeed = 5;

	PlayerController controller;
	
	bool nextToDoor = false;
	Vector3 offsetPosition;
	float upperDoor;
	float lowerDoor;

	void Start () {
		controller = GetComponent<PlayerController> ();
	}
	
	void Update () {
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, 0);
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		if (Input.GetKey (KeyCode.UpArrow) && nextToDoor) {
			moveSpeed = 0;
			transform.position = new Vector3(transform.position.x, transform.position.y, -0.16f);
			StartCoroutine(StairsMove(upperDoor));
		}
		if (Input.GetKey (KeyCode.DownArrow) && nextToDoor) {
			moveSpeed = 0;
			transform.position = new Vector3(transform.position.x, transform.position.y, -0.16f);
			StartCoroutine(StairsMove(lowerDoor));
		}
	}

	IEnumerator StairsMove(float door) {
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(transform.position.x, door - 0.35f, -0.5f);
        moveSpeed = 5;
    }


	void OnTriggerEnter(Collider other) {
        offsetPosition = other.gameObject.transform.position;
		if (other.gameObject.layer == 12) {
        	nextToDoor = true;	
        	if (other.GetComponent<Stairs>().UpperDoor != null) {
        		upperDoor = other.GetComponent<Stairs>().UpperDoor.position.y;
        	}
        	if (other.GetComponent<Stairs>().LowerDoor != null) {
        		lowerDoor = other.GetComponent<Stairs>().LowerDoor.position.y;
        	}
        }
    }

    void OnTriggerExit(Collider other) {
        nextToDoor = false;
	}
}
