using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }

    // Verificador de colisões da Unity
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Enemy")) {
            playerScript.DamagePlayer();
        }
    }



}
