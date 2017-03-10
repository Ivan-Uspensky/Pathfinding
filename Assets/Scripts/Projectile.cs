using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	public float SpreadRange;
	float speed = 10;
	float damage = 1;

	float lifetime = 3;
	float skinWidth = .1f;
	
	Vector3 spreadVector;
	
	void Start() {
		Destroy (gameObject, lifetime);

		float spread = Random.Range (-SpreadRange, SpreadRange);
		spreadVector = new Vector3(spread, spread, 1);

		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f, collisionMask);
		if (initialCollisions.Length > 0) {
			OnHitObject(initialCollisions[0], transform.position);
		}
	}
	
	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	void Update () {
		float moveDistance = Time.deltaTime * speed;
		CheckCollisions (moveDistance);
		// transform.Translate (spreadVector * moveDistance);
		transform.Translate (Vector3.right * moveDistance);
		Debug.Log(transform.position.x);
		Debug.Log(transform.position.y);
		Debug.Log(transform.position.z);
	}

	void CheckCollisions(float moveDistance) {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint) {
		GameObject.Destroy (gameObject);
	}
}
