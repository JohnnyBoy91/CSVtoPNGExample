using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class CSVtoPNG : MonoBehaviour
{
    private string pngPath = "/outputpng.png";
    //i.e. 4 = 4x4
    private int pngSize = 4;
    void Start()
    {
        List<string[]> colorData = getData("testcsv", ", ");
        
        //size is length * width
        Color[] colorValues = new Color[pngSize * pngSize];

        //takes the csv data and applies it to the rgba values of each pixel
        //apply your own algorithm here or in your spreadsheet before exporting
        for (int i = 0; i < colorValues.Length; i++)
        {
            colorValues[i].r = float.Parse(colorData[i][0]);
            colorValues[i].g = float.Parse(colorData[i][1]);
            colorValues[i].b = float.Parse(colorData[i][2]);
            colorValues[i].a = float.Parse(colorData[i][3]);
        }

        //create new texture for png
        Texture2D newPNGTexture = new Texture2D(pngSize, pngSize, TextureFormat.RGBA32, false);

        //need this for writing textures in editor
#if UNITY_EDITOR
        newPNGTexture.alphaIsTransparency = true;
#endif
        //apply pixels
        newPNGTexture.SetPixels(colorValues);
        newPNGTexture.Apply();

        //encode to png & write to file
        byte[] _bytes = newPNGTexture.EncodeToPNG();
        if (!File.Exists(Application.streamingAssetsPath + pngPath)) File.Create(Application.streamingAssetsPath + pngPath);
        File.WriteAllBytes(Application.streamingAssetsPath + pngPath, _bytes);
    }

    //This reads the csv file
    private List<string[]> getData(string path, string splitStr = ", ")
    {
        if (path == "")
        {
            throw new Exception("should be pass csv path.");
        }
        List<string[]> data = new List<string[]>();
        TextAsset csv = Resources.Load(path) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            string[] items = line.Split(splitStr.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            data.Add(items);
        }
        return data;
    }

}
