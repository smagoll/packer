using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    public Transform listOffices;
    public Transform listFurnitures;
    public GameObject canvasMain;
    public GameObject canvasContent;
    public GameObject officeContent;
    public Button buttonCreateOffice;
    public TextMeshProUGUI money;

    [Header("Tilemaps")]
    public Tilemap tilemapFloor;
    public Tilemap tilemapHighlight;
    public Tilemap tilemapFurniture;
}