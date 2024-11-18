using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selectable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lookPercentageLabel;

    [HideInInspector] public float lookPercentage;

    public void Update()
    {
        lookPercentageLabel.text = lookPercentage.ToString("F3");
    }
}
