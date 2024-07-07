using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    [Header("Lists")]
    public Transform listOffices;
    public Transform listOfficesStore;
    public Transform listFurnitures;
    
    [Header("Windows")]
    public GameObject canvasMain;
    public GameObject canvasContent;
    public GameObject officeContent;
    public GameObject furnitureWindow;
    public GameObject storeWindow;
    
    [Header("Buttons")]
    public Button buttonBuyOffice;
    public Button buttonBackContentToMain;
    public Button buttonBackStoreToMain;
    
    [Header("Texts")]
    public TextMeshProUGUI money;

    [Header("Tilemaps")]
    public Tilemap tilemapFloor;
    public Tilemap tilemapHighlight;
    public Tilemap tilemapFurniture;

    [Header("Effects")] 
    public Image failBackground;
}