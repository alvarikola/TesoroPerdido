using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalItems = 2; // Número total de objetos
    private int collectedItems = 0;
    //public GameObject finalScreen; // Pantalla final (UI)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CollectItem()
    {
        collectedItems++;
        Debug.Log("Objetos recogidos: " + collectedItems);

        if (collectedItems == totalItems)
        {
            //ShowFinalScreen();
            Debug.Log("tienes todos los objetos");
        }
    }

    void ShowFinalScreen()
    {
        Debug.Log("tienes todos los objetos");
        //finalScreen.SetActive(true); // Mostrar la pantalla final
    }
}

