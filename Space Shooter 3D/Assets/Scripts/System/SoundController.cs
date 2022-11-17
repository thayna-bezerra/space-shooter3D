using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController sounds;

    [Space(10)]

    [Header("Efeitos Sonoros")]
    public AudioSource explosion;
    public AudioSource shooter;
    public AudioSource damage;
    public AudioSource click;
    public AudioSource panelFinal;

    private void Awake()
    {
        sounds = this;

    } //inicializar //instanciar classe

}