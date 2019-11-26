using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private Transform playerLoc;
	private float waitTime = 1.5f;
	private Transform camPosition;

	void Start () {
		playerLoc = GameObject.Find("MegaMan").GetComponent<Transform>();
		waitTime += Time.time;
		camPosition = Camera.main.GetComponent<Transform>();
	}
	
	void Update () {

		if(waitTime > Time.time)
			return;
		try{
		if(playerLoc.transform.position.x >= 5.5f){
			camPosition.position = new Vector3 (playerLoc.position.x, 
		                                              camPosition.position.y, 
		                                              camPosition.position.z);
			}
		} catch {
			playerLoc = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		}
	}
}
