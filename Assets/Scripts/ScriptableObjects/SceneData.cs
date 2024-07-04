using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    public Transform officeTransform;
    public GameObject canvasMain;
    public GameObject officeContent;
    public Button buttonCreateOffice;
    public Button buttonCreateFurniture;
    public TextMeshProUGUI money;

    [Header("Tilemaps")]
    public Tilemap tilemapFloor;
    public Tilemap tilemapHighlight;
    public Tilemap tilemapFurniture;
}