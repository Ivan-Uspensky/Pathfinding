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
			gunController.Shoot();
		}
	}
}
