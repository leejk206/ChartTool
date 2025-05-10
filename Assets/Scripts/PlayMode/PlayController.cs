using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public int BPM;
    public float secondsPerBeat;
    public float speedY;
    // = beatHeight * BPM / 60

    void Start()
    {
        secondsPerBeat = 60f / BPM;
        speedY = 25 / secondsPerBeat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
