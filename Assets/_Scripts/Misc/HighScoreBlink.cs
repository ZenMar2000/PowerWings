using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Oscillator))]
public class HighScoreBlink : MonoBehaviour
{
    private TMP_Text textToMakeBlink;
    private Oscillator oscillator;
    // Start is called before the first frame update
    void Start()
    {
        textToMakeBlink = GetComponent<TMP_Text>();
        oscillator = GetComponent<Oscillator>();
        oscillator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        textToMakeBlink.alpha = 0.6f - oscillator.Progress;
    }
}
