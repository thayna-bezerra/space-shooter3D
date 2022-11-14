using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public VolumeControll vc;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        vc.volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");

        DontDestroyOnLoad(this.gameObject);
    }
}
