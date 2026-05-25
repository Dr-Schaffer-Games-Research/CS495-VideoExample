using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public double rewindSeconds = 10.0;

    public GameObject finishButton;

    public DataMiner dataMiner;

    private void Awake()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }

    public void PauseVideo()
    {
        videoPlayer.Pause();
    }

    public void TogglePause()
    {
        if (videoPlayer.isPlaying)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }

    public void Rewind10Seconds()
    {
        double newTime = videoPlayer.time - rewindSeconds;

        if (newTime < 0)
            newTime = 0;

        videoPlayer.time = newTime;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        finishButton.SetActive(true);
    }

    public void finish()
    {
        dataMiner.LogData();
    }

    private void Update()
    {
        if (videoPlayer.isPlaying)
        {
            dataMiner.playTime += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}