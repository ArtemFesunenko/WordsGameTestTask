using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LettersInput : MonoBehaviour
{
    [SerializeField] private Vector2 letterSize;
    [SerializeField] private float positioningCircleRadius;

    void Start()
    {

    }

    public void Initialize(char[] chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            var newGO = new GameObject();
            newGO.AddComponent<Image>();
            newGO.transform.parent = transform;
            Vector2 circlePosition = new Vector2();
            circlePosition.x = positioningCircleRadius * Mathf.Cos(2 * Mathf.PI * i / chars.Length);
            circlePosition.y = positioningCircleRadius * Mathf.Sin(2 * Mathf.PI * i / chars.Length);
            newGO.transform.localPosition = circlePosition;
            newGO.GetComponent<RectTransform>().sizeDelta = letterSize;
            var newGOForText = new GameObject();
            newGOForText.transform.SetParent(newGO.transform);
            var textMesh = newGOForText.AddComponent<TextMeshProUGUI>();
            textMesh.text = chars[i].ToString();
            textMesh.color = UnityEngine.Color.black;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.enableAutoSizing = true;
            var rectTransform = newGOForText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }        
    }
}
