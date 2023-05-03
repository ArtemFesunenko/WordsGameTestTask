using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextAsset gameConfig;
    [SerializeField] private TextAsset gridConfig;
    [SerializeField] private WordsGridBuilder wordsGridBuilder;
    [SerializeField] private LettersInputManager lettersInputManager;

    void Start()
    {
        if (wordsGridBuilder == null) 
            wordsGridBuilder = FindObjectOfType<WordsGridBuilder>();
        if (lettersInputManager == null)
            lettersInputManager = FindObjectOfType<LettersInputManager>();
        if (gameConfig == null)
        {
            string gameConfigPath = "gameConfig";
            gameConfig = Resources.Load<TextAsset>(gameConfigPath);
        }
        if (gridConfig == null)
        {
            string gridConfigPath = "Levels/gridConfig";
            gridConfig = Resources.Load<TextAsset>(gridConfigPath);
        }
        wordsGridBuilder.Initialize(JsonUtility.FromJson<WordsGridData>(gridConfig.text));
    }

#if UNITY_EDITOR
    [ContextMenu("Create Example Grid Config")]
    [MenuItem("WordsGame / Create Example Grid Config")]
#endif
    private void CreateExampleGridConfig()
    {
        var dataPath = Path.Combine(Application.dataPath, "gridConfig.json");
        var gridData = CreateGridData();
        Save(gridData, dataPath);
    }

    private WordsGridData CreateGridData()
    {
        var gridData = new WordsGridData();
        gridData.WordDatas = new WordData[5];

        WordData firstWord = new WordData();
        firstWord.Word = "����";
        firstWord.Direction = WordDirectionType.horizontal;
        firstWord.StartPosition = new Vector2(2, 0);
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

        return gridData;
    }

    public WordsGridData Load(string dataPath)
    {
        if (File.Exists(dataPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(dataPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                return JsonUtility.FromJson<WordsGridData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + dataPath + "\n" + e);
            }
        }
        return new WordsGridData();
    }

    public void Save(WordsGridData data, string dataPath)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));

            string dataToStorage = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(dataPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStorage);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + dataPath + "\n" + e);
        }
    }
}
