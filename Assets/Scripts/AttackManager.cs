using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public float speed;
    public float timeDestroy;

    // O início é chamado antes da primeira atualização de quadro
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // A atualização é chamada uma vez por quadro
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
                
    }
}
