using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace csci485.Character
{
    // This class is the brain that manages the player state and functionality
    public class PlayerController : MonoBehaviour
    {
        private Movement movement;
        private Rigidbody2D rigidbody;
        
        [SerializeField] private float playerHealth = 100f;
        public float currentHealth;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private GameObject sprite = null;


        public Animator player_animator;

        private PlayerSoundHandler sh;
        private ResourceManagement resourceManagement;

        public delegate void OnCurrentHealthChange(float hp);
        public OnCurrentHealthChange onCurrentHealthChange;

        public delegate void OnMaxHealthChange(float hp);
        public OnMaxHealthChange onMaxHealthChange;

        public delegate void OnMinHealthChange(float hp);
        public OnMinHealthChange onMinHealthChange;

        Vector3 startPosition;

        public AudioClip laserShot;
        public AudioClip shieldHit;
        public AudioClip deathExplosion;
        public AudioClip pickUp;
        public AudioClip boarded;


        AudioSource audioSource;

        float nextfuelUse = 1f;

        private void Awake()
        {
            movement = GetComponent<Movement>();
            sh = GetComponent<PlayerSoundHandler>();
            resourceManagement = GetComponent<ResourceManagement>();
            rigidbody = GetComponent<Rigidbody2D>();
            currentHealth = playerHealth;
        }

        private void Start()
        {
            onMaxHealthChange?.Invoke(playerHealth);
            onCurrentHealthChange?.Invoke(currentHealth);
            startPosition = transform.position;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if(rigidbody.velocity.magnitude > 0) //moving
            {
                if(Time.time > nextfuelUse)
                {
                    if (resourceManagement.useFuel(1))
                    {
                        nextfuelUse = Time.time + 3f;
                        //print("fuel");
                    }
                    else
                    {
                        //no fuel left
                    }
                    
                }
            }
        }

        // called from message on PlayerInput Component
        // Calls Move function to perform movement with direction parameter
        // last edit - Adam / 2020-10-27
        private void OnMove(InputValue value)
        {
            Vector2 dir = value.Get<Vector2>();
            if (movement.flame_sprite) //movement.flame_sprite.SetActive(false); 
            movement.Move(dir);

            movement.Rotate(dir);

        }

        public void OnFire(InputValue value)
        {
            Instantiate(projectile, transform.position, sprite.transform.rotation);
            audioSource.PlayOneShot(laserShot, 0.3f);
            player_animator.SetTrigger("attack");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Projectile p = col.GetComponent<Projectile>();
            if (p != null)
            {
                if (p.tag == this.tag) return; //your own projectile 
                TakeDamage(p.damageAmount);
                Destroy(p.gameObject);
            }
            else if (col.tag == "pickUp")
            {
                audioSource.PlayOneShot(pickUp, 0.8f);
            }
        }

        private void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                audioSource.PlayOneShot(deathExplosion , 0.5f);
                Respawn();
            }
            onCurrentHealthChange?.Invoke(currentHealth);
            audioSource.PlayOneShot(shieldHit, 0.3f);
            player_animator.SetTrigger("damage");
        }

        public void Heal()
        {
            currentHealth = playerHealth;
            onCurrentHealthChange?.Invoke(currentHealth);
        }

        public void Respawn()
        {
            currentHealth = playerHealth;
            GetComponent<ResourceManagement>().loseHalf();
            //transform.position = startPosition;
            World.MapBehavior map = FindObjectOfType<World.MapBehavior>();
            if (map) map.ResetCoordinates();

            onCurrentHealthChange?.Invoke(currentHealth);
        }

        public void Board()
        {
            audioSource.PlayOneShot(boarded, 1.0f);
        }
    }
}


