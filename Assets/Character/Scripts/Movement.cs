using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System;
using UnityEngine;

namespace csci485.Character
{
    // Controls Character movement behavior
    public class Movement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 2f;
        
        Rigidbody2D rigidBody;
        [SerializeField]GameObject player_sprite;
        
        public GameObject flame_sprite;
        
        public Animator flame_animator;
        public Animator player_animator;
        
        
        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            flame_sprite.SetActive(false);
        }

        
        // move the rigidbody
        // last edit - Adam - 2020/10/27
        public void Move(Vector2 direction)
        {
            rigidBody.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
        }

        
        //rotation
        //last edit - Anson - 2020/12
        public void Rotate(Vector2 direction) 
        {
            if (!player_sprite) return; //if player sprite is not assigned then ignore this function
        
            flame_animator.SetBool("Speed", false);
            player_animator.SetFloat("x", direction.x);
            player_animator.SetFloat("y", direction.y);





            if (direction.x==0&&direction.y==1)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 0);
                
                flame_animator.SetBool("Speed", true);
            }
            else if (direction.x==0&&direction.y==1)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 0);
                
                flame_animator.SetBool("Speed", true);
            }
            else if (direction.x == -1 && direction.y == 0)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 90);
               
                flame_animator.SetBool("Speed", true);
            }
            else if (direction.x == 0 && direction.y == -1)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 180);
                
                flame_animator.SetBool("Speed", true);
            }
            else if (direction.x == 1 && direction.y == 0)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 270);

                flame_animator.SetBool("Speed", true);
            }




            //??? doesnt work
            else if (direction.x <= -0.7 && direction.y >= 0.7)
            
            {
                //topleft
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 45);
                
                flame_animator.SetBool("Speed", true);
                
            }
            else if (direction.x >= 0.7 && direction.y >= 0.7)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 315);

                flame_animator.SetBool("Speed", true);
                
            }
            else if (direction.x <= -0.7 && direction.y <= -0.7)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 135);

                flame_animator.SetBool("Speed", true);
                
            }

            else if (direction.x >= 0.7 && direction.y <= -0.7)
            {
                player_sprite.transform.eulerAngles = new Vector3(0, 0, 225);

                flame_animator.SetBool("Speed", true);
                
            }

            if (flame_sprite) flame_sprite.SetActive(true);
        }
        
    }
}


