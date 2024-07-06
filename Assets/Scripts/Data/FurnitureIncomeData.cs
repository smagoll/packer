using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct FurnitureIncomeData
{
    public string title;
    public int id;
    public int income;
    public int price;
    public GameObject prefab;
    public Tile prefabTile;
}