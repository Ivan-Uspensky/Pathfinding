using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

	public Transform UpperDoor;
	public Transform LowerDoor;
	public static float yUpperPosition;
	public static float xUpperPosition;
	public static float yLowerPosition;
	public static float xLowerPosition;

	void Awake () {
		if (UpperDoor) {
			yUpperPosition = UpperDoor.position.y;
			xUpperPosition = UpperDoor.position.x;	
		}
		if (LowerDoor) {
			yLowerPosition = LowerDoor.position.y;
			xLowerPosition = LowerDoor.position.x;	
		}
	}
}
