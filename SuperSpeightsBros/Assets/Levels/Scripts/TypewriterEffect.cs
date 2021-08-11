using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public float writingSpeed = 50f; 
    public void Run(string textToType, TMP_Text textLabel)
    {
        StartCoroutine(routine: TypeText(textToType,textLabel));
    }
    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;
        yield return new WaitForSeconds(2);
        float t = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * writingSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            yield return null;
            textLabel.text = textToType.Substring(0, charIndex);
        }

        textLabel.text = textToType;
    }

}
