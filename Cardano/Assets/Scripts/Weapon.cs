using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Monster monster;

    private void OnTriggerEnter2D(Collider2D collision2d)
    {
        Player player;
        int dmg;
        int enemyDef;
        int result;
        //print(collision2d.gameObject.tag);
        switch (collision2d.gameObject.tag)
        {
            case "AlvoMonstro":
                    player = NetworkManager.instance.playerLocalInstance.GetComponent<Player>();
                    monster = collision2d.gameObject.transform.parent.GetComponent<Monster>();
                    dmg = player.manager.CalculateDamage(player.entity, player.entity.damage);
                    enemyDef = player.manager.CalculateDefense(monster.entity, monster.entity.defense);
                    result = dmg - enemyDef;

                    //Debug.Log("Player dmg: " + result.ToString());
                    //knockBack
                    if (!monster.entity.dead){
                        Vector2 difference = collision2d.transform.parent.position - transform.position;
                        collision2d.transform.parent.position = new Vector2(collision2d.transform.parent.position.x+difference.x, collision2d.transform.parent.position.y+difference.y);
                    }

                    if (result < 0)
                        result = 0;
        
                    //monster.entity.CurrentHealth -= result;
                    monster.entity.ReceberDano(result);
                    monster.animator.SetTrigger("hurt");
                    monster.entity.target = player.gameObject;
                    if(!this.gameObject.CompareTag("Arma")) {
                        Destroy(this.gameObject);
                    } 
                break;
            case "AlvoPlayer":
                    player = NetworkManager.instance.playerLocalInstance.GetComponent<Player>();
                    dmg = player.manager.CalculateDamage(monster.entity, monster.entity.damage);
                    int targetDef = player.manager.CalculateDefense(monster.entity.target.GetComponent<Player>().entity, monster.entity.target.GetComponent<Player>().entity.defense);
                    int dmgResult = dmg - targetDef;
 
                    if (!monster.entity.target.GetComponent<Player>().entity.dead){
                        Vector2 difference = collision2d.transform.parent.position - transform.position;
                        collision2d.transform.parent.position = new Vector2(collision2d.transform.parent.position.x+difference.x, collision2d.transform.parent.position.y+difference.y);
                    }

                    if (dmgResult < 0)
                        dmgResult = 0;
 
                    //Debug.Log("Inimigo atacou o player, Dmg: " + dmgResult);
                    monster.entity.target.GetComponent<Player>().entity.ReceberDano(dmgResult);
                    monster.entity.target.GetComponent<PlayerController>().playerAnimator.SetTrigger("hurt");
                    if(monster.entity.target.GetComponent<Player>().entity.id == NetworkManager.instance.playerLocalInstance.GetComponent<Player>().entity.id){
                        NetworkManager.instance.playerLocalInstance.GetComponent<PlayerController>().ShakeShoot();
                    }
                break;
        }

    }
}
