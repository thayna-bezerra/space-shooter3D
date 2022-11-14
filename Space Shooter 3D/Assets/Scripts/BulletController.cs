using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed = 400f;
    private Rigidbody rb;

    public GameObject explosion;
   // public int Score;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.z > AsteroidManager.Instance.asteroideSpawnDistance) Destroy(gameObject);

        rb.velocity = new Vector3(-90f, 0f, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Asteroid"))
        {

            GameController.gameController.Adicionar();

            SoundController.sounds.explosion.Play();

            collider.gameObject.GetComponent<AsteroidController>().DestroyAsteroid(); //chamando o método de outro script
            Destroy(gameObject);

            Instantiate(explosion, transform.position, Quaternion.identity);
            //instanciar particula de explosion
        }
    }
    
}
