using UnityEngine;

public struct WordsGridData
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