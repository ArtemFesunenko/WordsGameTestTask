using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct GridData
{
    public WordData[] WordDatas;
}

public struct WordData
{
    public Vector2 StartPosition;
    public WordDirectionType Direction;
    public string Word;
}

public enum WordDirectionType
{
    horizontal,
    vertical
}

public class WordsGridBuilder : MonoBehaviour
{
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GridLayoutGroup gridLayout;

    private GridData gridData;
    private GameObject[,] objectsGrid = new GameObject[10,10];
    private char[,] charsGrid = new char[10, 10];
    private List<char> keyChars = new List<char>();

    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        InitializeGridData();

        int maxXSize = 0;
        int maxYSize = 0;

        for (int i = 0; i < gridData.WordDatas.Length; i++)
        {
            var currentData = gridData.WordDatas[i];
            var pos = currentData.StartPosition;
            var roundedXPos = Mathf.RoundToInt(pos.x);
            var roundedYPos = Mathf.RoundToInt(pos.y);
            Debug.Log(currentData.Word);

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
                Debug.Log($"Position: {currentXPos},{currentYPos} letter: {charsGrid[currentXPos, currentYPos].ToString()}");

                if (keyChars.Contains(currentLetter) == false)
                {
                    keyChars.Add(currentLetter);
                }
            }
        }

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
                    Debug.Log(charsGrid[x, y].ToString());
                    //Debug.Log(charsGrid[x, y].ToString());
                    var newGOForText = new GameObject();
                    newGOForText.transform.SetParent(newGO.transform);
                    var textMesh = newGOForText.AddComponent<TextMeshProUGUI>();
                    textMesh.text = charsGrid[x, y].ToString();
                    textMesh.color = UnityEngine.Color.black;
                    textMesh.alignment = TextAlignmentOptions.Center;
                    textMesh.enableAutoSizing = true;
                    var rectTransform = newGOForText.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                }
                newGO.name = $"Slot{x},{y}";
                objectsGrid[x,y] = newGO;
            }
        }

        var lettersInput = FindObjectOfType<LettersInput>();
        lettersInput.Initialize(keyChars.ToArray());
    }

    private void InitializeGridData()
    {
        gridData = new GridData();
        gridData.WordDatas = new WordData[5];

        WordData firstWord = new WordData();
        firstWord.Word = "банк";
        firstWord.Direction = WordDirectionType.horizontal;
        firstWord.StartPosition = new Vector2(2,0);
        gridData.WordDatas[0] = firstWord;

        WordData secondWord = new WordData();
        secondWord.Word = "клан";
        secondWord.Direction = WordDirectionType.vertical;
        secondWord.StartPosition = new Vector2(5, 0);
        gridData.WordDatas[1] = secondWord;

        WordData thirdWord = new WordData();
        thirdWord.Word = "бал";
        thirdWord.Direction = WordDirectionType.horizontal;
        thirdWord.StartPosition = new Vector2(0, 1);
        gridData.WordDatas[2] = thirdWord;

        WordData fourthWord = new WordData();
        fourthWord.Word = "бланк";
        fourthWord.Direction = WordDirectionType.vertical;
        fourthWord.StartPosition = new Vector2(2, 0);
        gridData.WordDatas[3] = fourthWord;

        WordData fifthWord = new WordData();
        fifthWord.Word = "бак";
        fifthWord.Direction = WordDirectionType.horizontal;
        fifthWord.StartPosition = new Vector2(0, 4);
        gridData.WordDatas[4] = fifthWord;
    }
}
