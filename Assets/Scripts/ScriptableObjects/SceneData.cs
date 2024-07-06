using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    public Transform listOffices;
    public Transform listFurnitures;
    public GameObject canvasMain;
    public GameObject canvasContent;
    public GameObject officeContent;
    public GameObject furnitureWindow;
    public Button buttonCreateOffice;
    public Button buttonBackToMain;
    public TextMeshProUGUI money;

    [Header("Tilemaps")]
    public Tilemap tilemapFloor;
    public Tilemap tilemapHighlight;
    public Tilemap tilemapFurniture;
}