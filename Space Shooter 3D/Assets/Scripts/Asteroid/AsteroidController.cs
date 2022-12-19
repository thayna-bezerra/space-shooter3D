using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float moveSpeed = 50f;
    private Rigidbody rb;

    private Vector3 randomRotation;
    private float removeAsteroidZ;

    public Material targetMaterial;
    private Material baseMaterial;

    private Renderer[] renderers;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        randomRotation = new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f), Random.Range(0f, 100f)); //Valor para asteroide rodar aleatoriamente

        removeAsteroidZ = Camera.main.transform.position.z; //pegando o valor da posição Z

        renderers = GetComponentsInChildren<Renderer>();
        baseMaterial = renderers[0].material;
    }

    void Update()
    {
        if(transform.position.z < removeAsteroidZ)
        {
            AsteroidManager.Instance.aliveAsteroids.Remove(gameObject);
            Destroy(gameObject);
        }

        Vector3 moveVector = new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);
        rb.velocity = moveVector;

        transform.Rotate(randomRotation * Time.deltaTime);
    }


    public void ResetMaterial()
    {
        if (renderers == null)
            return;

        foreach(Renderer rend in renderers)
        {
            rend.material = baseMaterial;
        }
    }

    public void SetTargetMaterial() //
    {
        if (renderers == null)
            return;

        foreach (Renderer rend in renderers)
        {
            rend.material = targetMaterial;
        }

    }
    public void DestroyAsteroid()
    {
        AsteroidManager.Instance.aliveAsteroids.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().OnAsteroidImpact();
            DestroyAsteroid();
        }
    }

}
