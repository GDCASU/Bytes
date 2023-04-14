using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typeWriterSpeed = 50f;
    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeTextCo(textToType, textLabel));
    }

    private IEnumerator TypeTextCo(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        float elapsedTime = 0;
        int charIndex = 0;

        while (charIndex < textToType.Length)
        {
            elapsedTime += Time.deltaTime * typeWriterSpeed;
            charIndex = Mathf.FloorToInt(elapsedTime);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textLabel.text = textToType;
    }
}
