using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUD : MonoBehaviour
{
    // I love making everything a singleton
    private static HUD instance;
    private void Awake()
    {
        instance = this;
    }

    public TMPro.TMP_Text redBallCounter;
    public GameObject packageSprite;
    public GameObject socialMedia;
    public TMPro.TMP_Text timeText;
    public TMPro.TMP_Text deliveriesLeftText;

    int redBalls;
    int time;

    float timer;

    public static int Time
    {
        get => instance.time;
        set
        {
            instance.time = value;
            instance.SetTimeText();
        }
    }

    private void Start()
    {
        packageSprite.SetActive(false);
        redBallCounter.text = "0";
        socialMedia.SetActive(false);
    }

    public static void IncreaseRedBalls()
    {
        //instance.redBalls++;
        //instance.redBallCounter.text = instance.redBalls.ToString();
    }

    public static void ShowPackage()
    {
        instance.packageSprite.SetActive(true);
    }

    public static void HidePackage()
    {
        instance.packageSprite.SetActive(false);
    }

    public static void SetPackagesLeftToday(int left)
    {
        instance.deliveriesLeftText.text = $"Left today: {left}";
    }

    void SetTimeText()
    {
        int seconds = time % 60;
        int minutes = time / 60;
        timeText.text = $"{16 + minutes}:{seconds:00}";
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            socialMedia.SetActive(!socialMedia.activeSelf);

        timer += UnityEngine.Time.deltaTime;
        if (timer >= 1f)
        {
            Time++;
            timer = 0f;
        }
    }
}
