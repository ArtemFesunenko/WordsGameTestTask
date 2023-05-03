using UnityEngine;

[System.Serializable]
public struct WordsGridData
{
    public WordData[] WordDatas;
}

[System.Serializable]
public struct WordData
{
    public Vector2Int StartPosition;
    public WordDirectionType Direction;
    public string Word;
}

[System.Serializable]
public enum WordDirectionType
{
    horizontal,
    vertical
}