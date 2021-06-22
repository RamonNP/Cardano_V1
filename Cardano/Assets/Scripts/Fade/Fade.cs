using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public GameObject painelFume;
    public Image fume;
    public Color[] corTransicao;
    public float step;
    private bool emTrasicao;

/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
void Start()
{
    StartCoroutine("fadeO");
}
    public void fadeIn(){
        if(!emTrasicao){
            painelFume.SetActive(true);
            StartCoroutine("fadeI");
        }
    }

    public void fadeOut(){
        StartCoroutine("fadeO");
    }
    IEnumerator fadeI(){
        yield return new WaitForSeconds(0.5f);
        emTrasicao = true;
        for (float i = 0; i <= 1; i+=step)
        {
            fume.color = Color.Lerp(corTransicao[0], corTransicao[1], i);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator fadeO(){
        //print("NEROO");
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i <= 1; i+=step)
        {
            fume.color = Color.Lerp(corTransicao[1], corTransicao[0], i);
            yield return new WaitForEndOfFrame();
        }
        painelFume.SetActive(false);
        emTrasicao = false;
    }
}
