using UnityEngine;
using System.Collections;

public class EnemyBeamController : MonoBehaviour {

	private int Player_Layer = 10;
	private Animator anim;
	private bool impact = false;
	public int speed = 5;

	void Start () {
		Destroy (this.gameObject, 1f);	
		anim = GetComponent<Animator> ();
	}
	
	void Update () {

		if(!impact)
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, -speed);
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);

		if(impact){
			Destroy(this.gameObject, 0.25f);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == Player_Layer){
	  		  anim.SetTrigger("impact"); 
			impact = true;
		}
	}


}
