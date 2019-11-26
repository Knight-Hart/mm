using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour {


	public float alertDistance = 5f;
	public float health = 5f;
	public float attackMinTime = 1f;
	public float attackMaxTime = 4f;
	public GameObject explosion;
	public GameObject beam;		

	private float attackTime = 0f;	
	private GameObject player;		
	private MegaManController playerCtr;	
	private Animator anim;	
	private bool enemyActive;
	private bool firstDash = false;	
	private float explosionTime = 0f;
	private Animator explosionAnim = null;
	private float yDifference = 0f;

	public float speed = 1.25f;			
	public float vSpeed = 0.5f;	
	public int move = 0;				
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		playerCtr = player.GetComponent<MegaManController>();

		explosionAnim = explosion.GetComponent<Animator>();
		anim = GetComponent<Animator> ();
		attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
	}

	void FixedUpdate(){
		if(enemyActive){
			Attack();
			Movement();
		}
	}
	void Update () {
		try{
			if(!enemyActive){
				if(Vector2.Distance((Vector2)transform.position, player.transform.position) < 5f){
					enemyActive = true;
					speed = 1.25f;
					anim.SetBool("Alert", true);
				} 
			}

			if(enemyActive && !firstDash){
				if(Vector2.Distance((Vector2)transform.position, player.transform.position) > 3f){
					speed = 4f;			
				} else {
					firstDash = true;
				}
			} else {
				speed = 1.25f;
			}

			if(health <= 0){
				anim.SetTrigger("Death");	
				GetComponent<Rigidbody2D>().isKinematic = false;	
				enemyActive = false;			
				explosionAnim.SetTrigger("explode");
				DeathDriver();						
				Destroy(this.gameObject, 0.75f);	
			}
		} catch {
			player = GameObject.FindGameObjectWithTag("Player");
		}	
	}

	void DeathDriver(){
		if(Time.time >= explosionTime){
			float randX = Random.Range(-0.25f, 0.25f);
			float randY = Random.Range(-0.25f, 0.25f);

			explosion.transform.position = 
				new Vector3(transform.position.x + randX, transform.position.y + randY);

			explosionTime = Time.time + 0.9f;
		}
	}


	void Movement(){

		if(transform.position.x - player.transform.position.x > 0)
			move = -1;
		else
			move = 1;

		yDifference = transform.position.y - player.transform.position.y;
		if( yDifference > 1.1f){
				vSpeed = -0.1f;
		} else if (yDifference < 0.5f){
				vSpeed = 0.2f;
		} else {
				vSpeed = 0f;
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, vSpeed);
	}

	void Attack(){
		if(Time.time >= attackTime){
			attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
			anim.SetTrigger("Attack");
			Vector3 beamRotation = new Vector3(transform.rotation.x, transform.rotation.y, 90);
			Instantiate(beam, new Vector3(transform.position.x, transform.position.y-0.04f, 0f), Quaternion.Euler(beamRotation));
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "ChargedBeam")
			health -= 2;
		else if(col.gameObject.layer == 10){
			anim.SetTrigger("Damaged");
			health--;

		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.layer == 10){
			vSpeed = 4f;
			speed = 3f;
		}
	}
}
