using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GunController))]
public class ShootPlayer : MonoBehaviour {

	GunController gunController;
	public Transform Player;

	// Use this for initialization
	void Start () {
		gunController = GetComponent<GunController>();
	}
	
	// Update is called once per frame
	void Update () {
       	// if (Vector2.Distance((Vector2)Player.position, (Vector2)transform.position) < 5f) {
			// gunController.Shoot();
		// }
		if (Input.GetMouseButton(0)) {
			gunController.Shoot();
		}
	}
}
