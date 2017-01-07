using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {
	
	Grid grid;
	GameObject go;

	public Transform Player;
	public float speed = 20;
	public LayerMask playerLayer;
	public Transform covers;
	public Transform CoverZero;

	// public Transform target;

	Vector2[] path;
	int targetIndex;

	void Awake() {
		// grid = GetComponent<Grid>();
		// go = GameObject.Find("Pathfinding").GetComponent(Grid);

		go = GameObject.Find ("Pathfinding");
		grid = go.GetComponent <Grid> ();
		Debug.Log(grid);
	}

	void Start() {
		StartCoroutine (RefreshPath ());
	}

	Vector3 BehaveModel() {

		float currentDistance;
		float bestDistance = 999f;
		Vector3 closestCover = CoverZero.position;
		foreach (Transform child in covers) {
			currentDistance = Vector3.Distance((Vector2)Player.position, child.position);
			
			if (currentDistance < bestDistance && (Mathf.Abs(Mathf.Abs(Player.position.y) - Mathf.Abs(child.position.y)) < 0.2f)) {
			
				Vector3 coverSide = Player.InverseTransformPoint(child.position);
				if (coverSide.x > 0) {
					//player is on left side, get right cover
					closestCover = child.position + child.right;
				} else {
					//player is on right side, get left cover
					closestCover = child.position - child.right;
				}
			
				bestDistance = currentDistance;
			}
		}

		return closestCover;
	}

	IEnumerator RefreshPath() {
		
		Vector3 target = CoverZero.position;
		Vector2 targetPositionOld = (Vector2)CoverZero.position + Vector2.up; // ensure != to target.position initially

		Node currentPlayerPosition;
		Node previousPlayerPosition = grid.NodeFromWorldPoint((Vector2)Player.position);

		while (true) {
			target = BehaveModel();
			
			currentPlayerPosition = grid.NodeFromWorldPoint((Vector2)Player.position);	
			if (currentPlayerPosition.gridX != previousPlayerPosition.gridX) {
				previousPlayerPosition.walkable = true;
				previousPlayerPosition = currentPlayerPosition;
				currentPlayerPosition.walkable = false;
				
			}

			// if (target != null) {
				if (targetPositionOld != (Vector2)target) {
					targetPositionOld = (Vector2)target;
					path = Pathfinding.RequestPath (transform.position, target);
					StopCoroutine ("FollowPath");
					StartCoroutine ("FollowPath");
				}	
			// }
			
			yield return new WaitForSeconds (.25f);
		}
	}
		
	IEnumerator FollowPath() {
		if (path.Length > 0) {
			targetIndex = 0;
			Vector2 currentWaypoint = path [0];

			while (true) {
				if ((Vector2)transform.position == currentWaypoint) {
					targetIndex++;
					if (targetIndex >= path.Length) {
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}

				transform.position = Vector2.MoveTowards (transform.position, currentWaypoint, speed * Time.deltaTime);
				yield return null;

			}
		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube((Vector3)path[i], Vector3.one *.5f);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
