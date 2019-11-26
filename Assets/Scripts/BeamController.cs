using UnityEngine;
using System.Collections;

public class BeamController : MonoBehaviour {

	private int Enemy_Layer = 12;
	private Animator anim;
	private bool impact = false;
	private int direction = 0;		
	public int speed = 5;			
	void Start () {
		anim = GetComponent<Animator> ();		
		bool facingRight = GameObject.FindGameObjectWithTag("Player").GetComponent<MegaManController>().facingRight;
		direction = facingRight ? 1 : -1;		
		Vector3 scale = transform.localScale; 	
		scale.x *= direction;					
		transform.localScale = scale;		
		Destroy (this.gameObject, 1.5f);	
	}
	
	void Update () {
		if(impact){								
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
			Destroy(this.gameObject, 0.20f);
		} else {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == Enemy_Layer){
			anim.SetTrigger("impact"); 			
			impact = true;						
		}
	}

	void OnCollisionEnter2D(Collision2D col){	
		if(col.gameObject.layer == Enemy_Layer){
			anim.SetTrigger("impact"); 
			impact = true;
		}
	}

}
