using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace csci485.Character
{

    public class EnemyBehaviour : MonoBehaviour
    {
        private float health = 15;
        private Transform target;
        private float deathDelay = 5;
        private bool hasPlundered;
        private Vector3 v_diff;
        private float atan2;

        [SerializeField] float attackSpeed = 2f;
        [SerializeField] Projectile projectile;
        [SerializeField]private float attackRange;
        [SerializeField] private float speed;
        [SerializeField] private float pursueRange;
        //Vector2 playerDir;
        private float lastAtk = -10;

        public AudioClip enemyLaser;
        public AudioClip enemyHit;
        public AudioClip enemyDeath;

        public AudioSource audioSource;

        private PolygonCollider2D poly;
        private Renderer rend;

        // Start is called before the first frame update
        void Start()
        {
            UnityEngine.Debug.Log("enemy online");
            target = GameObject.Find("Player").GetComponent<Transform>();
            hasPlundered = false;
            audioSource = GetComponent<AudioSource>();
            poly = GetComponent<PolygonCollider2D>();
            rend = GetComponentInChildren<SpriteRenderer>();
        }

        // Follow and look at the player if they are close enough
        //I haven't got shooting done (if the priates shoot to)
        void Update()
        {
            if(Vector2.Distance(transform.position, target.position) <= pursueRange && hasPlundered == false)
            {
                v_diff = (target.position - transform.position);
                atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg + 270);

                if (Vector2.Distance(transform.position, target.position) <= attackRange)
                {
                    if(Time.time > lastAtk)
                    {
                        Instantiate(projectile, transform.position, transform.rotation);
                        lastAtk = attackSpeed + Time.time;
                        audioSource.PlayOneShot(enemyLaser, 0.1f);
                    }    
                    if(Vector2.Distance(transform.position, target.position) > 3f)
                        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
            }
            
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == ("Player"))
            {
                //IDK, play laughing sound fx??
                //Player should teleport back to home planet.
                hasPlundered = true;
                Destroy(gameObject, deathDelay);
                col.gameObject.GetComponent<PlayerController>().Board();
                col.gameObject.GetComponent<PlayerController>().Respawn();
            }
            else
            {
                Projectile p = col.GetComponent<Projectile>();
                if (p != null)
                {
                    if (p.tag == this.tag) return; //your own projectile 
                    TakeDamage(p.damageAmount);
                    audioSource.PlayOneShot(enemyHit, 0.5f);
                    Destroy(p.gameObject);
                }
            }
        }


        //got shot
        private void TakeDamage(float amount)
        {
            health-=amount;
            if(health <= 0)
            {
                //Explosion or something
                rend.enabled = false;
                poly.enabled = false;
                hasPlundered = true; //stop moving and shooting
                audioSource.PlayOneShot(enemyDeath, 0.4f);
                
                Destroy(gameObject, enemyDeath.length);
            }

        }
        
    }
}