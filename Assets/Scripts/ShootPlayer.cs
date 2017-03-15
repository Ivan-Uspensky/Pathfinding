using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GunController))]
public class ShootPlayer : MonoBehaviour {

	GunController gunController;
	public Transform Player;

	void Start () {
		gunController = GetComponent<GunController>();
	}
	
	void Update () {
       	if (Vector2.Distance((Vector2)Player.position, (Vector2)transform.position) < 5f) {
			// RaycastHit hit;
			// if (Physics.Raycast (gunController.weaponHold.position, Vector3.right, out hit)) {
			// 	Debug.DrawLine(gunController.weaponHold.position, hit.point);
			// 	if (hit.collider.gameObject == Player.gameObject) { // can see player
			// 		gunController.Shoot();
			// 	}
			// }
			gunController.Shoot();
		}
	}
}

// if (Physics.Raycast(new Vector3(Player.position.x, Player.position.y - .2f, Player.position.z), Vector3.right, out hit)) {

// if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {