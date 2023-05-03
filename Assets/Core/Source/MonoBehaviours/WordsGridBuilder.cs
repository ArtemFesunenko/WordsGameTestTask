using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WordsGridBuilder : MonoBehaviour
{
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject gridCellPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    private WordsGridData gridData;
    private GameObject[,] objectsGrid = new GameObject[10,10];
    private char[,] charsGrid = new char[10, 10];
    private List<char> keyChars = new List<char>();
    private List<WordData> openedWords = new List<WordData>();
    private int actualXSize;
    private int actualYSize;
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
        
    }

    private void SubscribeToInputManager()
    {
        inputManager.onInputEntered += EnterInput;
    }

    private void UnsubscribeFromInputManager()
    {
        inputManager?.ClearEvent();
    }

    public void Initialize(WordsGridData gridData)
    {
        this.gridData = gridData;
        InitializeGridData();
        InstantiateGridView();

        inputManager.Initialize(keyChars.ToArray());
    }

    private void InitializeGridData()
    {
        actualXSize = 0;
        actualYSize = 0;

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

                if (actualXSize <= currentXPos)
                {
                    actualXSize = currentXPos;
                }
                if (actualYSize <= currentYPos)
                {
                    actualYSize = currentYPos;
                }

                charsGrid[currentXPos, currentYPos] = currentLetter;

                if (keyChars.Contains(currentLetter) == false)
                {
                    keyChars.Add(currentLetter);
                }
            }
        }

        actualXSize++;
        actualYSize++;
    }

    private void InstantiateGridView()
    {
        gridLayout.constraintCount = actualXSize;
        var rectTransform = GetComponent<RectTransform>();
        float cellSize = rectTransform.rect.width / actualXSize - gridLayout.spacing.x * actualXSize;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        for (int x = 0; x < actualXSize; x++)
        {
            for (int y = 0; y < actualYSize; y++)
            {
                var newGO = Instantiate(gridCellPrefab, slotsContainer);
                var images = newGO.GetComponentsInChildren<Image>();
                var textMesh = newGO.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.enabled = false;
                if (charsGrid[x, y] == char.MinValue)
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        images[i].enabled = false;
                    }
                }
                else
                {
                    textMesh.text = charsGrid[x, y].ToString();
                }
                newGO.name = $"Cell{x},{y}";
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

        if (wordMatched == false)
        {
            audioSource.PlayOneShot(clip);
            GridShakeAnimation(0.15f, 10f, 10);
        }
    }

    private async void OpenWord(WordData wordData)
    {
        var pos = wordData.StartPosition;
        var roundedXPos = Mathf.RoundToInt(pos.x);
        var roundedYPos = Mathf.RoundToInt(pos.y);

        for (int c = 0; c < wordData.Word.Length; c++)
        {
            var currentXPos = wordData.Direction == WordDirectionType.horizontal ? roundedXPos + c : roundedXPos;
            var currentYPos = wordData.Direction == WordDirectionType.vertical ? roundedYPos + c : roundedYPos;

            var textMesh = objectsGrid[currentXPos, currentYPos].GetComponentInChildren<TextMeshProUGUI>();
            textMesh.enabled = true;
            await TextSizeAnimationTask(textMesh, Vector2.one * 1.5f, 0.1f);
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

    async Task TextSizeAnimationTask(TextMeshProUGUI textMesh, Vector2 targetScale, float duration)
    {
        var taskTime = duration * 1000;
        float interpolator;
        var startScale = textMesh.transform.localScale;
        while (taskTime > 0)
        {
            interpolator = Mathf.InverseLerp(duration, 0, taskTime / 1000);
            textMesh.transform.localScale = Vector2.Lerp(targetScale, startScale, interpolator);
            await Task.Delay(1);
            taskTime--;
        }
    }

    async Task GridShakeAnimation(float duration, float strength, int shakesAmount = 10)
    {
        var startPosition = transform.position;
        var taskTime = duration * 1000;
        int tick = Mathf.RoundToInt(taskTime) / shakesAmount;
        while (taskTime > 0)
        {
            transform.position = startPosition + new Vector3(UnityEngine.Random.insideUnitCircle.x, UnityEngine.Random.insideUnitCircle.y, transform.position.z) * strength;
            await Task.Delay(tick);
            taskTime -= tick;
        }
        transform.position = startPosition;
    }
}
