using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour {


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
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		playerCtr = player.GetComponent<MegaManController>();

		explosionAnim = explosion.GetComponent<Animator>();
		anim = GetComponent<Animator> ();
		attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
	}

	void FixedUpdate(){
			Attack();
	}
	void Update () {
		try
        { 
			if(health <= 0){
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

	void Attack(){
		if(Time.time >= attackTime){
			attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
			Instantiate(beam, new Vector3(90, transform.position.y, 0f), Quaternion.identity);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "ChargedBeam")
			health -= 2;
		else if(col.gameObject.layer == 10){
			health--;

		}
	}
}
