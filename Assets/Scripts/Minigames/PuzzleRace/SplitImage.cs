using UnityEngine;
using System.Collections.Generic;
 
public class SplitImage : MonoBehaviour
{
    public Texture2D exampleImage;
    public List<Texture2D> imageTiles;
 
    public void Start()
    {
        Split(exampleImage, 15, 15); //120, 120 is size of new tiles
    }
 
    public void Split(Texture2D image, int width, int height)
    {
        bool perfectWidth = image.width % width == 0;
        bool perfectHeight = image.height % height == 0;
 
        int lastWidth = width;
        if(!perfectWidth)
        {
            lastWidth = image.width - ((image.width / width) * width);
        }
 
        int lastHeight = height;
        if(!perfectHeight)
        {
            lastHeight = image.height - ((image.height / height) * height);
        }
 
        int widthPartsCount = image.width / width + (perfectWidth ? 0 : 1);
        int heightPartsCount = image.height / height + (perfectHeight ? 0 : 1);
 
        for (int i = 0; i < widthPartsCount; i++)
        {
            for(int j = 0; j < heightPartsCount; j++)
            {
                int tileWidth = i == widthPartsCount - 1 ? lastWidth : width;
                int tileHeight = j == heightPartsCount - 1 ? lastHeight : height;
 
                Texture2D g = new Texture2D(tileWidth, tileHeight);
                g.SetPixels(image.GetPixels(i * width, j * height, tileWidth, tileHeight));
                g.Apply();
                imageTiles.Add(g);
            }
        }
    }
}