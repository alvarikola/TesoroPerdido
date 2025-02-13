using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            GameManager.instance.CollectItem(); // Llamar al GameManager
            Destroy(gameObject); // Destruir el objeto
        }
    }
}
