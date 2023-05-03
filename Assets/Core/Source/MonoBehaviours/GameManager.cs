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

    private GameConfigData gameConfigData;

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
        gameConfigData = JsonUtility.FromJson<GameConfigData>(gameConfig.text);

        if (gridConfig == null)
        {
            string gridConfigPath = "Levels/gridConfig";
            gridConfig = Resources.Load<TextAsset>(gridConfigPath);
        }
        wordsGridBuilder.Initialize(JsonUtility.FromJson<WordsGridData>(gridConfig.text));
    }

#if UNITY_EDITOR
    [ContextMenu("Create Example Game Config")]
    [MenuItem("WordsGame / Create Example Game Config")]
#endif
    private void CreateExampleGameConfig()
    {
        var dataPath = Path.Combine(Application.dataPath, "gameConfig.json");
        GameConfigData newGameConfigData = new GameConfigData();
        newGameConfigData.ErrorSoundEnabled = true;
        newGameConfigData.ErrorScreenShakeEnabled = true;
        newGameConfigData.ErrorScreenShakeDuration = 0.15f;
        newGameConfigData.ErrorScreenShakeStrength = 10;
        newGameConfigData.ErrorScreenShakeShakesAmount = 10;
        File.WriteAllText(dataPath, JsonUtility.ToJson(newGameConfigData));
    }

#if UNITY_EDITOR
    [ContextMenu("Create Example Grid Config")]
    [MenuItem("WordsGame / Create Example Grid Config")]
#endif
    private void CreateExampleGridConfig()
    {
        var dataPath = Path.Combine(Application.dataPath, "gridConfig.json");
        var gridData = CreateGridData();
        SaveGridData(gridData, dataPath);
    }

    private WordsGridData CreateGridData()
    {
        var gridData = new WordsGridData();
        gridData.WordDatas = new WordData[5];

        WordData firstWord = new WordData();
        firstWord.Word = "банк";
        firstWord.Direction = WordDirectionType.horizontal;
        firstWord.StartPosition = new Vector2(2, 0);
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

        return gridData;
    }

    public WordsGridData LoadGridData(string dataPath)
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

    public void SaveGridData(WordsGridData data, string dataPath)
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
