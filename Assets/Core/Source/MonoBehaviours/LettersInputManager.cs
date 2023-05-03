using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LettersInputManager : MonoBehaviour
{
    public event Action<string> onInputEntered;

    [SerializeField] private Vector2 letterSize;
    [SerializeField] private float positioningCircleRadius;
    [SerializeField] private LetterInputHandler letterUIPrefab;

    private LetterInputHandler[] letterInputHandlers;
    private bool inputStarted;
    private List<int> activatedLettersList = new List<int>();
    private char[] keyChars;

    private void OnEnable()
    {
        SubscribeToHandlersEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromHandlersEvents();
    }

    public void Initialize(char[] chars)
    {
        keyChars = chars;
        letterInputHandlers = new LetterInputHandler[chars.Length];

        for (int i = 0; i < chars.Length; i++)
        {
            int currentIndex = i;

            var newLetterHandler = Instantiate(letterUIPrefab, transform);
            
            Vector2 circlePosition = new Vector2();
            circlePosition.x = positioningCircleRadius * Mathf.Cos(2 * Mathf.PI * currentIndex / chars.Length);
            circlePosition.y = positioningCircleRadius * Mathf.Sin(2 * Mathf.PI * currentIndex / chars.Length);
            newLetterHandler.transform.localPosition = circlePosition;
            newLetterHandler.GetComponent<RectTransform>().sizeDelta = letterSize;

            var textMesh = newLetterHandler.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = chars[currentIndex].ToString();

            letterInputHandlers[currentIndex] = newLetterHandler;
        }        
        
        SubscribeToHandlersEvents();
    }

    private void SubscribeToHandlersEvents()
    {
        if (letterInputHandlers == null) return;
        for (int i = 0; i < letterInputHandlers.Length; i++)
        {
            int currentIndex = i;

            var inputHandler = letterInputHandlers[currentIndex];
            inputHandler.OnPointerDownEvent += () => PointerDownHandler(currentIndex);
            inputHandler.OnPointerUpEvent += () => PointerUpHandler(currentIndex);
            inputHandler.OnPointerEnterEvent += () => PointerEnterHandler(currentIndex);
        }
    }

    private void UnsubscribeFromHandlersEvents()
    {
        for (int i = 0; i < letterInputHandlers.Length; i++)
        {
            var inputHandler = letterInputHandlers[i];
            inputHandler?.ClearEvents();
        }
    }

    private void PointerDownHandler(int letterIndex)
    {
        StartInput();
        UpdateInput(letterIndex);
    }

    private void PointerUpHandler(int letterIndex)
    {
        EndInput();
    }

    private void PointerEnterHandler(int letterIndex)
    {
        UpdateInput(letterIndex);
    }

    private void StartInput()
    {
        inputStarted = true;
    }

    private void EndInput()
    {
        inputStarted = false;
        var resultChars = ConvertIndexesToChars(activatedLettersList.ToArray());
        EnterInput(String.Concat(resultChars));
        activatedLettersList.Clear();
        for (int i = 0; i < letterInputHandlers.Length; i++)
        {
            letterInputHandlers[i].SetActive(false);
        }
    }

    private void UpdateInput(int letterIndex)
    {
        if (inputStarted == false) return;

        var lastActivatedLetter = -1;
        var penultActivatedLetter = -1;
        if (activatedLettersList.Count > 1)
        {
            lastActivatedLetter = activatedLettersList[activatedLettersList.Count - 1];
            penultActivatedLetter = activatedLettersList[activatedLettersList.Count - 2];
        }

        if (penultActivatedLetter == letterIndex)
        {
            letterInputHandlers[lastActivatedLetter].SetActive(false);
            activatedLettersList.Remove(lastActivatedLetter);
        }
        else
        {
            if (activatedLettersList.Contains(letterIndex)) return;
            activatedLettersList.Add(letterIndex);
            letterInputHandlers[letterIndex].SetActive(true);
        }
    }

    private char[] ConvertIndexesToChars(int[] indexes)
    {
        var resultChars = new char[indexes.Length];

        for (int i = 0; i < indexes.Length; i++)
        {
            resultChars[i] = keyChars[indexes[i]];
        }

        return resultChars;
    }

    private void EnterInput(string input)
    {
        onInputEntered?.Invoke(input);
    }

    public void ClearEvent()
    {
        onInputEntered = null;
    }
}
