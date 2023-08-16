using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpeed : MonoBehaviour
{
    public Text SpeedText;
    public Rigidbody Rb;

    private void Update()
    {
        SpeedText.text = Rb.velocity.magnitude.ToString(CultureInfo.InvariantCulture);
    }
}
