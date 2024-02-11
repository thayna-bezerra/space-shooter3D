using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator CamAnimation;

    public Joystick joystick;
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
        SetUpLimits();

        currentHealth = maxHealth;

        layerMask = LayerMask.GetMask("EnemyRaycast");
    }

    void Update()
    {
        GameController.gameController.CheckScore();

        MovePlayer();
        RotatePlayer();

        CalculateLimits();

        RaycastForAsteroids();
    }

    private void MovePlayer()
    {
        float horizontalMove = joystick.Horizontal; //Input.GetAxis("Horizontal");
        float verticalMove = joystick.Vertical; //Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(horizontalMove, verticalMove, 0f);

        rb.velocity = moveVector * moveSpeed;
    }

    private void RotatePlayer()
    {
        float currentPositionX = transform.position.x; //posição X atual (qual lado da tela a nave está)
        float newRotationZ;

        if(currentPositionX < 0)
        {
            newRotationZ = Mathf.Lerp(5f, -maxRotation, currentPositionX / minX);
        }

        else
        {
            newRotationZ = Mathf.Lerp(5f, maxRotation, currentPositionX / maxX);
        }

        Vector3 currentRotationVector3 = new Vector3(0f, 0f, newRotationZ);
        Quaternion newRotation = Quaternion.Euler(currentRotationVector3);
        transform.localRotation = newRotation;
    } //Rotacionar nave quando esta for para o canto da tela

    private void CalculateLimits() //Calculando Limite da tela 
    {
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

        transform.position = currentPos;
    }

    private void SetUpLimits() //Nave não sair da tela
    {
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottonCorners = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, camDistance)); //limites abaixo
        Vector2 topCorners = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, camDistance)); //limites acima

        Bounds gameObjectBouds = GetComponent<Collider>().bounds; //LIMITES
        float objectWidht = gameObjectBouds.size.x;

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


    private void RaycastForAsteroids()
    {
        List<GameObject> currentTargets = new List<GameObject>(); //alvo atual

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

        //verifica se os alvos anteriores e atuais são os mesmos
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

    public void OnAsteroidImpact()
    {
        currentHealth--;

        Handheld.Vibrate();

        //animator
        CamAnimation.Play("CamVibrating");

        SoundController.sounds.damage.Play();

        Instantiate(fxDamage, transform.position, Quaternion.identity);

        //mudando barra de vida
        gameController.ChangeHealthBar(maxHealth, currentHealth); //chamando metodo de outro script e definindo parametro
    }
}