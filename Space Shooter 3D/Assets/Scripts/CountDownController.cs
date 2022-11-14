using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownController : MonoBehaviour
{
    [Space(10)]

    [Header("Contador")]
    public int countdownTime;
    public Text countdownText;

    [Space(10)]

    [Header("Objetos que ser�o Pausados")]
    public GameObject GameController; //Objeto com o Script GameController
    public GameObject Player; //Objeto com o Script PlayerMove
    public GameObject Asteroid; //Objeto com o Script PlayerMove

    void Start() 
    { 
        StartCoroutine(CountdownToStart()); 
    } //Chamando o m�todo do IEnumerator "CountdownToStart()" assim q inicia

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0) //contador definido 3 -> enquanto 3 menor que 0
        {
            //desativar estes scripts ENQUANTO  3 menor que 0
            GameController.GetComponent<GameController>().enabled = false;
            Player.GetComponent<PlayerController>().enabled = false;
            Asteroid.GetComponent<AsteroidManager>().enabled = false;

            //Passar o valor da variavel Countdown para o Text
            countdownText.text = countdownTime.ToString();

            //como "tornar o tempo real", esperar 1 segundo
            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        //ap�s sair do la�o de repeti��o, o text apresentar� "GO"
        countdownText.text = "GO!";

        //ap�s sair do la�o de repeti��o, REATIVAR os scripts e o sprite do Boss
        GameController.GetComponent<GameController>().enabled = true;
        Player.GetComponent<PlayerController>().enabled = true;
        Asteroid.GetComponent<AsteroidManager>().enabled = true;

        //esperar 1 segundo at� desativar o Text do countdown na tela
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}
