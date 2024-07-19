using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
    public GameObject officeContent;
    public GameObject furniturePanel;
    public GameObject editPanel;
    public GameObject storeWindow;
    public GameObject job;

    [Header("Buttons")]
    public ClickButtonDefault buttonBuyOffice;
    public ClickButtonDefault buttonBackContentToMain;
    public ClickButtonDefault buttonBackStoreToMain;
    public ClickButtonDefault sellFurniture;
    public ClickButtonDefault buttonJob;
    public ClickButtonDefault buttonOffices;

    [Header("Texts")] 
    public TextMeshProUGUI money;
    public TextMeshProUGUI income;

    [Header("Tilemaps")] 
    public Tilemap tilemapFloor;
    public Tilemap tilemapHighlight;
    public Tilemap tilemapFurniture;

    [Header("Effects")] 
    public Image failBackground;
    public Image backgroundContent;
    
}