using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimacionTexto : MonoBehaviour
{
    public string frase;
    public Text texto;

    void Start()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Reloj()
    {
        foreach (char caracter in frase)
        {
            texto.text = texto.text + caracter;
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator Delay()
    {
	//Dentro del WaitForSeconds pon el delay que quieras
        yield return new WaitForSeconds(0.5f);
	StartCoroutine(Reloj());
    }
}
