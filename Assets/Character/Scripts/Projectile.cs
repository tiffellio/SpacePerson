using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public float projectileSpeed = 10f;
    public float damageAmount = 5f;

    public float lifeTime = 2f;

    private void OnEnable()
    {
        lifeTime += Time.time;
    }

    void Update()
    {
        transform.Translate(Vector3.up *projectileSpeed * Time.deltaTime);
        if(Time.time > lifeTime)
        {
            Destroy(gameObject);
        }
    }

}
