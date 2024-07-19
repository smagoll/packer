using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct FurnitureIncomeData
{
    public string title;
    public int id;
    public float income;
    public float price;
    public Tile prefabTile;
    public Sprite icon;
}