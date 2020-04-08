using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 4;
    public int jumpForce; // Força do pulo
    public int health; // Vida do personagem rei
    public Transform groundCheck;

    public float attackRate; // quantidade de ataques por segundo
    public Transform spawnAttack;
    private float nextAttack = 0f; // controle de ataques
    public GameObject attackPrefab;

    private bool invunerable = false;
    private bool grounded = false; // Verifica se o personagem está tocando o chão
    private bool jumping = false; // Verifica se está pulando
    private bool facingRight = true;

    private SpriteRenderer sprite;
    private Rigidbody2D rb2D;
    private Animator anim;

    // O início é chamado antes da primeira atualização de quadro
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // A atualização é chamada uma vez por quadro
    void Update()
    {
        // Verifica se está tocando o chão
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && grounded) { // se estiver pulando
            jumping = true;
        }
        Animations(); // Verificação e animação

        // Se o personagem está no chão e deu um soco então dispara a animação de ataque
        if (Input.GetButtonDown("Fire1") && grounded && Time.time > nextAttack){
           Attack();     
        }
    }

    void FixedUpdate(){
        float move = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(move * speed, rb2D.velocity.y); // velocidade do personagem

        if (move < 0f && facingRight || (move > 0f && !facingRight)) { // verifica posição e muda o lado de visão
            Flip();
        }

        if (jumping) {
            rb2D.AddForce(new Vector2(0f, jumpForce));
            jumping = false; // força pra não pular novamente
        }
    }

    // Muda o personagem de lado
    void Flip(){
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    // Animações
    void Animations() {
        anim.SetFloat("velY", rb2D.velocity.y);

        // Pulando ou caindo
        anim.SetBool("jumpFall", !grounded);

        // Se movendo
        anim.SetBool("walk", rb2D.velocity.x != 0f && grounded);
    }

    // Ataque do personagem
    void Attack(){
        anim.SetTrigger("punch"); // trigger do animator
        nextAttack = Time.time + attackRate;

        // Cria o objeto de ataque
        GameObject cloneAttack = Instantiate(attackPrefab, spawnAttack.position, spawnAttack.rotation);

        if (!facingRight) {
            cloneAttack.transform.eulerAngles = new Vector3(180,0,180);
        }
    }


    // Efeito de dano do rei
    IEnumerator DamageEffect() {
        //TODO: Efeito de camera

        // Efeito do personagem
        for (float i = 0f; i < 1f; i += 0.1f){
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        invunerable = false;
    }

    // Dano sofrido pelo inimigo
    public void DamagePlayer(){
        if (!invunerable) {
            invunerable = true;
            health--; // decrementa a vida
            StartCoroutine(DamageEffect());
            
            if (health < 1) {
                // Destroy(gameObject);
                Debug.Log("Morreu");
            }
        }
    }
}
