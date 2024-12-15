using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndVideo : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;
    [SerializeField]
    GameObject _EndUI;

    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        gameObject.SetActive(false);
        _EndUI.SetActive(true);
    }
}
