using System;
using UnityEngine;

[Serializable]
public struct OfficeSizeData
{
    public string title;
    public OfficeType officeType;
    public int size;
    public int price;
    public GameObject prefab;
    public GameObject prefabStore;
}