using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WordsGridBuilder : MonoBehaviour
{
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GridLayoutGroup gridLayout;

    private WordsGridData gridData;
    private GameObject[,] objectsGrid = new GameObject[10,10];
    private char[,] charsGrid = new char[10, 10];
    private List<char> keyChars = new List<char>();
    private List<WordData> openedWords = new List<WordData>();
    private LettersInputManager inputManager 
    {  
        get 
        {
            if (_inputManager == null)
            {
                _inputManager = FindObjectOfType<LettersInputManager>();
            }
            return _inputManager;
        } 
        set { _inputManager = value; }
    }
    private LettersInputManager _inputManager;

    private void OnEnable()
    {
        SubscribeToInputManager();
    }

    private void OnDisable()
    {
        UnsubscribeFromInputManager();
    }

    private void Start()
    {
        CreateGridData();
        InitializeGridData();
        InstantiateGridView();

        inputManager.Initialize(keyChars.ToArray());
        //SubscribeToInputManager();
    }

    private void SubscribeToInputManager()
    {
        inputManager.onInputEntered += EnterInput;
    }

    private void UnsubscribeFromInputManager()
    {
        inputManager?.ClearEvent();
    }

    private void CreateGridData()
    {
        gridData = new WordsGridData();
        gridData.WordDatas = new WordData[5];

        WordData firstWord = new WordData();
        firstWord.Word = "����";
        firstWord.Direction = WordDirectionType.horizontal;
        firstWord.StartPosition = new Vector2(2,0);
        gridData.WordDatas[0] = firstWord;

        WordData secondWord = new WordData();
        secondWord.Word = "����";
        secondWord.Direction = WordDirectionType.vertical;
        secondWord.StartPosition = new Vector2(5, 0);
        gridData.WordDatas[1] = secondWord;

        WordData thirdWord = new WordData();
        thirdWord.Word = "���";
        thirdWord.Direction = WordDirectionType.horizontal;
        thirdWord.StartPosition = new Vector2(0, 1);
        gridData.WordDatas[2] = thirdWord;

        WordData fourthWord = new WordData();
        fourthWord.Word = "�����";
        fourthWord.Direction = WordDirectionType.vertical;
        fourthWord.StartPosition = new Vector2(2, 0);
        gridData.WordDatas[3] = fourthWord;

        WordData fifthWord = new WordData();
        fifthWord.Word = "���";
        fifthWord.Direction = WordDirectionType.horizontal;
        fifthWord.StartPosition = new Vector2(0, 4);
        gridData.WordDatas[4] = fifthWord;
    }

    private void InitializeGridData()
    {
        int maxXSize = 0;
        int maxYSize = 0;

        for (int i = 0; i < gridData.WordDatas.Length; i++)
        {
            var currentData = gridData.WordDatas[i];
            var pos = currentData.StartPosition;
            var roundedXPos = Mathf.RoundToInt(pos.x);
            var roundedYPos = Mathf.RoundToInt(pos.y);

            for (int c = 0; c < currentData.Word.Length; c++)
            {
                var currentLetter = currentData.Word[c];
                var currentXPos = currentData.Direction == WordDirectionType.horizontal ? roundedXPos + c : roundedXPos;
                var currentYPos = currentData.Direction == WordDirectionType.vertical ? roundedYPos + c : roundedYPos;

                if (maxXSize < currentXPos)
                {
                    maxXSize = currentXPos;
                }
                if (maxYSize < currentYPos)
                {
                    maxYSize = currentYPos;
                }

                charsGrid[currentXPos, currentYPos] = currentLetter;

                if (keyChars.Contains(currentLetter) == false)
                {
                    keyChars.Add(currentLetter);
                }
            }
        }
    }

    private void InstantiateGridView()
    {
        for (int x = 0; x < objectsGrid.GetLength(0); x++)
        {
            for (int y = 0; y < objectsGrid.GetLength(1); y++)
            {
                var newGO = new GameObject();
                newGO.transform.parent = slotsContainer;
                var image = newGO.AddComponent<Image>();
                if (charsGrid[x, y] == char.MinValue)
                {
                    image.enabled = false;
                }
                else
                {
                    var newGOForText = new GameObject();
                    newGOForText.transform.SetParent(newGO.transform);
                    var textMesh = newGOForText.AddComponent<TextMeshProUGUI>();
                    textMesh.text = charsGrid[x, y].ToString();
                    textMesh.color = UnityEngine.Color.black;
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.enableAutoSizing = true;
                    textMesh.enabled = false;
                    var rectTransform = newGOForText.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                }
                newGO.name = $"Slot{x},{y}";
                objectsGrid[x, y] = newGO;
            }
        }
    }

    private void EnterInput(string input)
    {
        bool wordMatched = false;
        for (int i = 0; i < gridData.WordDatas.Length; i++)
        {
            if (gridData.WordDatas[i].Word.ToLower() == input.ToLower())
            {
                wordMatched = true;
                OpenWord(gridData.WordDatas[i]);
            }
        }
        
        Debug.Log(wordMatched);
    }

    private void OpenWord(WordData wordData)
    {
        var pos = wordData.StartPosition;
        var roundedXPos = Mathf.RoundToInt(pos.x);
        var roundedYPos = Mathf.RoundToInt(pos.y);

        for (int c = 0; c < wordData.Word.Length; c++)
        {
            var currentLetter = wordData.Word[c];
            var currentXPos = wordData.Direction == WordDirectionType.horizontal ? roundedXPos + c : roundedXPos;
            var currentYPos = wordData.Direction == WordDirectionType.vertical ? roundedYPos + c : roundedYPos;

            objectsGrid[currentXPos, currentYPos].GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        }

        if (openedWords.Contains(wordData) == false)
        {
            openedWords.Add(wordData);
        }

        if (openedWords.Count == gridData.WordDatas.Length)
        {
            SceneManager.LoadScene(0);
        }
    }
}
