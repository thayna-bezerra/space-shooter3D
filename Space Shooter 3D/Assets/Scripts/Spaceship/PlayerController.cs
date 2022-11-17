using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator CamAnimation;

    public float moveSpeed = 10f;
    public float maxRotation = 25f;

    private Rigidbody rb;
    private float minX, maxX, minY, maxY;

    public int maxHealth = 4;
    private int currentHealth;

    public GameController gameController;

    public Transform[] missleSpawnPoints;
    public GameObject bulletPrefab;

    public float fireInterval = 2f;
    private bool canFire = true;

    public GameObject fxDamage;

    private Vector3 raycastDirection = new Vector3(0f, 0f, 1f);
    public float raycastDst = 100f;
    int layerMask;

    private List<GameObject> previousTargets = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetUpBoundries();

        currentHealth = maxHealth;

        layerMask = LayerMask.GetMask("EnemyRaycast");
    }

    void Update()
    {
        GameController.gameController.CheckScore();
        MovePlayer();
        RotatePlayer();

        CalculateBoundries();

        RaycastForAsteroids();
    }

    private void RaycastForAsteroids()
    {
        List<GameObject> currentTargets = new List<GameObject>();

        foreach (Transform missleSpawnPoint in missleSpawnPoints)
        {
            RaycastHit hit;
            Ray ray = new Ray(missleSpawnPoint.position, raycastDirection);

            if(Physics.Raycast(ray, out hit, raycastDst, layerMask))
            {
                GameObject target = hit.transform.gameObject;
                currentTargets.Add(target);
            }
        }

        bool listsChanged = false;

        //check if the previous and current targets are the same
        if (currentTargets.Count != previousTargets.Count)
        {
            listsChanged = true;
        }

        else
        {
            for(int i = 0; i < currentTargets.Count; i++)
            {
                if(currentTargets[i] != previousTargets[i])
                {
                    listsChanged = true;
                }
            }
        }

        if(listsChanged == true)
        {
            AsteroidManager.Instance.UpdateAsteroids(currentTargets);
            previousTargets = currentTargets;
        }
    }

    public void FireRockets()
    {
        if (canFire)
        {
            foreach(Transform t in missleSpawnPoints)
            {

                SoundController.sounds.shooter.Play();
                Instantiate(bulletPrefab, t.position, Quaternion.identity);
            }

            canFire = false;

            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(fireInterval);

        canFire = true;
    }

    private void RotatePlayer()
    {
        float currentX = transform.position.x;
        float newRotationZ;

        if(currentX<0)
        {
            newRotationZ = Mathf.Lerp(5f, -maxRotation, currentX / minX);
        }
        else
        {
            newRotationZ = Mathf.Lerp(5f, maxRotation, currentX / maxX);
        }

        Vector3 currentRotationVector3 = new Vector3(0f, 0f, newRotationZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;

    }

    private void CalculateBoundries() //calculando espaço tela
    {
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        transform.position = currentPos;
    }

    private void SetUpBoundries() //p n sair da tela
    {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottonCorners = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, camDistance));
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, camDistance));

        Bounds gameObjectBouds = GetComponent<Collider>().bounds;
        float objectWidht = gameObjectBouds.size.x;
        float objectHeight = gameObjectBouds.size.y;

        minX = bottonCorners.x + objectWidht;
        maxX = topCorners.x - objectWidht;

        maxY = bottonCorners.y + objectWidht;
        maxY = topCorners.y - objectWidht;

        //chama asteroide
        AsteroidManager.Instance.maxX = maxX;
        AsteroidManager.Instance.minX = minX;
        AsteroidManager.Instance.maxY = maxY;
        AsteroidManager.Instance.minY = minY;

    }

    private void MovePlayer()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(horizontalMove, verticalMove, 0f);

        rb.velocity = moveVector * moveSpeed;
    }

    public void OnAsteroidImpact()
    {
        currentHealth--;
        
        //Handheld.Vibrate();

        //animator
        CamAnimation.Play("CamVibrating");

        SoundController.sounds.damage.Play();

        Instantiate(fxDamage, transform.position, Quaternion.identity);

        //mudando barra de vida
        gameController.ChangeHealthBar(maxHealth, currentHealth); //chamando metodo de outro script e definindo parametro

        if(currentHealth == 0)
        {
            OnPlayerDeath();
        }
    }

    public void OnPlayerDeath()
    {
        print("died");
    }
}