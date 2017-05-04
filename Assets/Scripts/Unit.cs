using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(GunController))]
public class Unit : MonoBehaviour {
	
	public Transform Player;
	public float speed = 20;
	public LayerMask playerLayer;
	public Transform covers;
	public Transform CoverZero;
	public int IdCounter;
	
	Grid grid;
	GameObject go;
	GunController gunController;

	Node childNodeL;
	Node childNodeR;
	Node childNode;
	Node previousChildNode;

	Vector2[] path;
	int targetIndex;

	string state = "";

	void Awake() {
		go = GameObject.Find ("Pathfinding");
		grid = go.GetComponent<Grid>();
		gunController = GetComponent<GunController>();
	}

	void Start() {
		StartCoroutine (RefreshPath ());
	}

	Vector3 BehaveModel() {
		float currentDistance;
		float bestDistance = 999f;
		Vector3 closestCover = CoverZero.position;
		
		if (previousChildNode == null){
			previousChildNode = grid.NodeFromWorldPoint((Vector2)closestCover);
		}  

		if (state == "Finished" && (Vector2.Distance((Vector2)Player.position, (Vector2)transform.position) > 4.7f)) {
			Vector3 playerSide = Player.InverseTransformPoint(transform.position);
			RaycastHit hit;
			
			if (playerSide.x > 0) {
				closestCover = (Vector2)Player.position + (Vector2.right * 4.7f);
			} else {
				closestCover = (Vector2)Player.position - (Vector2.right * 4.7f);
			}	
		} else {
			foreach (Transform child in covers) {
				currentDistance = Vector2.Distance((Vector2)Player.position, child.position);
				childNode = grid.NodeFromWorldPoint((Vector2)child.position);
				if (currentDistance < bestDistance && (Mathf.Abs(Mathf.Abs(Player.position.y) - Mathf.Abs(child.position.y)) < 0.4f)) {
					if (childNode.busy == 0 || childNode.busy == IdCounter) {
						Vector3 coverSide = Player.InverseTransformPoint(child.position);
						if (coverSide.x > 0) {
							//player is on left side, get right cover
							closestCover = child.position + child.right;
						} else {
							//player is on right side, get left cover
							closestCover = child.position - child.right;
						}
						bestDistance = currentDistance;
						previousChildNode.busy = 0;
						previousChildNode = childNode;
						childNode.busy = IdCounter;
					}
				}
			}
		}
		
		return closestCover;
	}

	IEnumerator RefreshPath() {
		Vector3 target = CoverZero.position;
		Vector2 targetPositionOld = (Vector2)CoverZero.position + Vector2.up; // ensure != to target.position initially

		Node currentPlayerPosition;
		Node previousPlayerPosition = grid.NodeFromWorldPoint((Vector2)CoverZero.position + Vector2.up);

		Node rightPos = null;
		Node leftPos = null;
		Node oldRightPos = null;
		Node oldLeftPos = null;

		while (true) {
			target = BehaveModel();
			currentPlayerPosition = grid.NodeFromWorldPoint((Vector2)Player.position);
			if (currentPlayerPosition.gridX != previousPlayerPosition.gridX) {
				previousPlayerPosition.walkable = true;
				previousPlayerPosition = currentPlayerPosition;
				currentPlayerPosition.walkable = false;

        		RaycastHit hit;
        		// if (Physics.Raycast(Player.position, Vector3.right, out hit)) {
				if (Physics.Raycast(new Vector3(Player.position.x, Player.position.y - .2f, Player.position.z), Vector3.right, out hit)) {
            		rightPos = grid.NodeFromWorldPoint((Vector2)hit.point);
            		Debug.DrawLine(Player.position, hit.point);
        		}
            	// if (Physics.Raycast(Player.position, -Vector3.right, out hit)) {
				if (Physics.Raycast(new Vector3(Player.position.x, Player.position.y - .2f, Player.position.z), -Vector3.right, out hit)) {
            		leftPos = grid.NodeFromWorldPoint((Vector2)hit.point);
            		Debug.DrawLine(Player.position, hit.point);
            	}
            	if (oldLeftPos != leftPos || oldRightPos != rightPos) {
            		if (oldLeftPos != null && oldRightPos != null) { 
	            		for (int i = oldLeftPos.gridX; i< oldRightPos.gridX; i++ ) {
			            	Node temp = grid.GetNode(i, oldLeftPos.gridY);
			            	temp.walkable = true;
			            }
			        }

			        oldLeftPos = leftPos;
	            	oldRightPos = rightPos;

	            	// if (leftPos != null && rightPos != null) { 
		            // 	for (int i = leftPos.gridX; i< rightPos.gridX; i++ ) {
		            // 		Node temp = grid.GetNode(i, leftPos.gridY);
		            // 		temp.walkable = false;
		            // 	}	
	            	// }
            	}

				if (targetPositionOld != (Vector2)target) {
					targetPositionOld = (Vector2)target;
					path = Pathfinding.RequestPath (transform.position, target, IdCounter);
					StopCoroutine ("FollowPath");
					StartCoroutine ("FollowPath");
				}
			}
			yield return new WaitForSeconds (.2f);
		}
	}
		
	// IEnumerator RefreshPath() {
	// 	Vector2 targetPositionOld = (Vector2)Player.position + Vector2.up; // ensure != to target.position initially
	// 	while (true) {
	// 		if (targetPositionOld != (Vector2)Player.position) {
	// 			targetPositionOld = (Vector2)Player.position;
	// 			path = Pathfinding.RequestPath (transform.position, Player.position);
	// 			StopCoroutine ("FollowPath");
	// 			StartCoroutine ("FollowPath");
	// 		}
	// 		yield return new WaitForSeconds (.25f);
	// 	}
	// }

	IEnumerator FollowPath() {
		if (path.Length > 0) {
			targetIndex = 0;
			Vector2 currentWaypoint = path [0];

			while (true) {
				if ((Vector2)transform.position == currentWaypoint) {
					state = "Moving";
					targetIndex++;
					if (targetIndex >= path.Length) {
						state = "Finished";
						yield break;
					}
					currentWaypoint = path [targetIndex];
				}

				// float moveDirX = Mathf.Sign(currentWaypoint.x - transform.position.x);
				Vector2 direction = (Vector2)transform.position + currentWaypoint;
				// if at waypoint, maintain prior rotation
				// if (moveDirX != 0) { 
				// transform.eulerAngles = Vector3.up * ((moveDirX == 1) ? 1 : 180);
				transform.eulerAngles = direction;
				// }
				transform.position = Vector3.MoveTowards (transform.position, new Vector3(currentWaypoint.x, currentWaypoint.y, -0.5f), speed * Time.deltaTime);
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
