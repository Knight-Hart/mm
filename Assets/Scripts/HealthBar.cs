using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Vector3 offset;

	private GameObject mainCamera;
	private Transform mainCameraTrans;

	void Start () {
		mainCamera = Camera.main.gameObject;
		mainCameraTrans = mainCamera.transform;
		offset = new Vector3 (-4f, 0.8f, 10f);
	}

	void Update () {
		try{
			transform.position = mainCameraTrans.position + offset;
		} catch {
			mainCamera = Camera.main.gameObject;
		}
	}
}
