using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    private bool isWalking;
    public bool lookLeft;
    public GameObject player;
    public Animator petAnimator;

    public float speed = 1;
    public float keepDistance = 0.3f;

    float input_x;
    float input_y;
    float lastDirectionX;
    float lastDirectionY;

    Vector2 petPos;
    Vector2 playerPos;

    private void Start()
    {
        petAnimator = this.transform.GetChild(0).GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        petPos = transform.position;
        playerPos = SetDirection(1, 1, player.transform.position);
        
        transform.position = Vector2.MoveTowards(petPos, playerPos, speed * Time.deltaTime);
    }

    private void Update()
    {
        //quando morre destroi o objeto player, isso faz com que popule a variavel novamente
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);

        if (isWalking)
        {
            VerificaFlipPatrol(input_x);
        }

        if (input_x > 0 || input_x < 0)
            lastDirectionX = input_x;

        if (input_y > 0 || input_y < 0)
            lastDirectionY = input_y;

        petAnimator.SetBool("isWalking", isWalking);

        petPos = transform.position;
        playerPos = SetDirection(lastDirectionX, lastDirectionY, player.transform.position);

        transform.position = Vector2.MoveTowards(petPos, playerPos, speed * Time.deltaTime);
    }

    Vector2 SetDirection(float input_x, float input_y, Vector2 playerPos)
    {
        if(input_x < 0)
        {
            playerPos.x += keepDistance;
        }
        else if(input_x > 0)
        {
            playerPos.x -= keepDistance;
        }

        if (input_y < 0)
        {
            playerPos.y += keepDistance;
        }
        else if (input_y > 0)
        {
            playerPos.y -= keepDistance;
        }

        return playerPos;
    }
     private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        lookLeft = !lookLeft;
    }
    public void VerificaFlipPatrol(float x) {
         if (x> 0 && lookLeft == true)
            {
                Flip();
            } else if(x < 0 && lookLeft == false)
            {
                Flip();
            }
    }
}