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
		if (Mathf.Abs(transform.position.y - Player.position.y) <= 0.1f) {
			if (Vector2.Distance((Vector2)Player.position, (Vector2)transform.position) < 5f) {
				// RaycastHit hit;
				// if (Physics.Raycast (gunController.weaponHold.position, Vector3.right, out hit)) {
				// 	Debug.DrawLine(gunController.weaponHold.position, hit.point);
				// 	// if (hit.collider.gameObject == Player.gameObject) { // can see player
				// 	// 	gunController.Shoot();
				// 	// }
				// }
				gunController.Shoot();
			}
		}
	}
}
