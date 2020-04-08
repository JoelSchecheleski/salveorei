using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public int health;
    public float speed = 4;
    public Transform wallCheck; // checagem de limite 

    private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private bool facingRight = true;
    private bool tockedWall = false;

    // O início é chamado antes da primeira atualização de quadro
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // A atualização é chamada uma vez por quadro
    void Update()
    {
        // Verifica se o personagem "cobra" tocou em uma parede
        tockedWall = Physics2D.Linecast(transform.position,wallCheck.position, 1 <<  LayerMask.NameToLayer("Ground"));
        if (tockedWall) {
            Flip(); // Altera o lado
        }
    }

    void FixedUpdate(){
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y); // velocidade do personagem
    }

    void Flip() {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    // Verificador de colisões da Unity
    void OnTriggerEnter2D (Collider2D other){

        if (other.CompareTag("Attack")) {
            DamageEnemy();
        }
        
    }

    // Gera o dano no inimigo
    void DamageEnemy(){
        health--;
        StartCoroutine(DamageEffect());
        if (health < 1) {
            Destroy(gameObject);
        }
    }

    // Efeito de dano
    IEnumerator DamageEffect() {
        float actualSpeed = speed;
        speed = speed * -1; // salta pra tras
        sprite.color = Color.red;
        rb2D.AddForce(new Vector2(0f, 200f));
        yield return new WaitForSeconds(0.1f);
        speed = actualSpeed;
        sprite.color = Color.white;        
    }

}
