using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-1)]
public class WorldGenerator : MonoBehaviour
{
    [Header("PROCEDURAL")]
    
    [SerializeField] private GameObject groundTile;
    [SerializeField] private GameObject greenTile;

    [SerializeField] private int height = 60;
    public int Height => height/2;
    [SerializeField] private int width = 60;
    public int Width => width/2;
    
    [SerializeField] private int maxHeight = 20;
    [SerializeField] private float scale = 2;
    
    private int AllocatedHeight;
    private int AllocatedWidth;

    [Header("TEXTURE GENERATED")] 
    
    [SerializeField] private Texture2D mapTexture;
    [SerializeField] private Color groundColor;
    [SerializeField] private Color grassColor; 
    [SerializeField] private GameObject mfTile;
    [SerializeField] private Texture2D MFDOOMTexture;


    private List<GameObject> worldTiles = new List<GameObject>();
    private float offsetX;
    private float offsetY;
    private Transform worldParent;
    private PlayerInput playerInput;
    
    
    private float CalculatePerlinCoordinate(int x, int y)
    {
      
        return Mathf.PerlinNoise(((float) x / width+ offsetX) * scale , ((float) y / height + offsetY) * scale);
    }
    
    private void GenerateTiles()
    {
        DeleteTiles();
        offsetX = Random.Range(0, 9999);
        offsetY = Random.Range(0, 9999);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var perlinCoordinate = CalculatePerlinCoordinate(x, z);
                var maxWorldHeight = (int) (perlinCoordinate * maxHeight);
                for (int y = 0; y < maxWorldHeight; y++)
                {
                    var tilePosition = new Vector3(x,y,z);
                    var tile = Instantiate(y == maxWorldHeight - 1 ? greenTile : groundTile, tilePosition,
                        Quaternion.identity);
                    GroupTile(tile);
                }
            }
        }
    }
    private void DeleteTiles()
    {
        if(worldTiles.Count<=0) return;
        foreach (var tiled in worldTiles)
        {
            Destroy(tiled);
        }
        worldTiles.Clear();
    }
    
    

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        worldParent = new GameObject("World").transform;
        AllocatedHeight = height;
        AllocatedWidth = width;
 
        GenerateTiles();
    }

    private void Update()
    {
        if (playerInput.Generate)
        {
            height = AllocatedHeight;
            width = AllocatedWidth;
            GenerateTiles();
        }

        if (playerInput.GenerateTexture)
        {
            height = mapTexture.height;
            width = mapTexture.width;
            GenerateTiles(mapTexture);
        }

        if (playerInput.GenerateMFDOOM)
        {
            height = MFDOOMTexture.height;
            width = MFDOOMTexture.width;
            GenerateMFDOOM(MFDOOMTexture);

        }
    }

    private void GenerateTile(Color definedColor,Vector3 position)
    {
         GroupTile(Instantiate(definedColor == groundColor ? groundTile : greenTile, position, Quaternion.identity));
    }

    private void GenerateTiles(Texture2D mapTexture)
    {
        DeleteTiles();
        for (int x = 0; x < mapTexture.height; x++)
        {
            for (int y = 0; y < mapTexture.width; y++)
            {
                var newPosition = new Vector3(x,0f,y);
                var newColor = mapTexture.GetPixel(x, y);
                GenerateTile(newColor,newPosition);
            }
        }
    }

    private void GenerateMFDOOM(Texture2D mapTexture)
    {
        DeleteTiles();
        for (int x = 0; x < mapTexture.height; x++)
        {
            for (int y = 0; y < mapTexture.width; y++)
            {
                var newPosition = new Vector3(x,0f,y);
                var newColor = mapTexture.GetPixel(x, y);
                var tile = Instantiate(mfTile, newPosition, Quaternion.identity);
                tile.GetComponent<Renderer>().material.SetColor("_Color",newColor);
                GroupTile(tile);
            }
        }
    }

    private void GroupTile(GameObject tile)
    {
        tile.transform.SetParent(worldParent);
        worldTiles.Add(tile);
    }
    
   

   
}
