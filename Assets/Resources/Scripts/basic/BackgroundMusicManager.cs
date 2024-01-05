using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //check for existing background music
        BackgroundMusicManager[] objs = GameObject.FindObjectsOfType<BackgroundMusicManager>();
        //if first object not this destroy this
        if (objs[0] != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

}
