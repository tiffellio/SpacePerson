using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csci485.World
{
    public class Area : MonoBehaviour
    {
        MapBehavior mapBehavior;

        private void Awake()
        {
            mapBehavior = FindObjectOfType<MapBehavior>();
        }

        // Player Enters this area - update player coordinates and loaded map
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag != "Player") return;

            //print(name);
            mapBehavior.UpdatePlayerCoordinates(GetCoordinates());
        }

        // Get Current Coordinates
        public Vector2Int GetCoordinates()
        {
            return new Vector2Int((int)transform.position.x / mapBehavior.AreaSize, (int)transform.position.y / mapBehavior.AreaSize);
        }
    }


}
