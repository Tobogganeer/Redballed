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
        set => instance.time = value;
    }

    private void Start()
    {
        packageSprite.SetActive(false);
        redBallCounter.text = "0";
        socialMedia.SetActive(false);
    }

    public static void IncreaseRedBalls()
    {
        instance.redBalls++;
        instance.redBallCounter.text = instance.redBalls.ToString();
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

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            socialMedia.SetActive(!socialMedia.activeSelf);

        timer += UnityEngine.Time.deltaTime;
        if (timer >= 1f)
        {
            time++;
            timer = 0f;

            int seconds = time % 60;
            int minutes = time / 60;
            timeText.text = $"{16 + minutes}:{seconds:00}";
        }
    }
}
