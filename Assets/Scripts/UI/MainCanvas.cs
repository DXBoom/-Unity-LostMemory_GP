using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance;

    public Text TextLevel;
    public Text WinLoseText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        TextLevel.text = GameManager.Instance.Level.ToString();
    }
}
