using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public BallScript BallScript;
    private TextMeshPro hpText;
    private void Start()
    {
        hpText = GetComponent<TextMeshPro>();
        BallScript = transform.parent.GetComponent<BallScript>();
    }
    private void FixedUpdate()
    {
        if (BallScript != null)
        {
            hpText.text = BallScript.currentHP.ToString();
        }
    }
}
