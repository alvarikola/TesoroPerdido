using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwich : MonoBehaviour
{
    private Vector3 initialPosition;
    private float rotationSpeed = 50f;
    private float floatAmplitude = 0.5f;
    private float floatSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
