using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControl : MonoBehaviour
{
    private GameObject collisionLight;

    private Vector3 lightPosition;

    public Color laserColor = Color.red;
    public int laserDistance = 50;
    public float initialWidth = 0.02f;
    public float finalWidth = 0.10f;

    public Material material;


    void Start()
    {
        collisionLight = new GameObject(); //criar objeto na scene
       
        collisionLight.AddComponent<Light>();
        collisionLight.GetComponent<Light>().color = laserColor;
        collisionLight.GetComponent<Light>().intensity = 1;
        collisionLight.GetComponent<Light>().bounceIntensity = 1;
        collisionLight.GetComponent<Light>().range = finalWidth; //diminuindo tamanho do laser

        lightPosition = new Vector3(0, 0, finalWidth);

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>(); //criando linha do laser
        lineRenderer.material = material;

        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        lineRenderer.startWidth = initialWidth;
        lineRenderer.endWidth = finalWidth;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector3 finalPoint = transform.position + transform.forward * laserDistance;
        RaycastHit CollisionPoint;

        if(Physics.Raycast(transform.position, transform.forward, out CollisionPoint, laserDistance))
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, CollisionPoint.point);
            collisionLight.transform.position = (CollisionPoint.point - lightPosition);
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, finalPoint);
            collisionLight.transform.position = finalPoint;
        }
    }
}
