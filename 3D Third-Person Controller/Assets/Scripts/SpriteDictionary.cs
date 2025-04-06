using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDictionary : SingletonPatternBase<TextureDictionary>
{
    private Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
    
    public Texture2D GetTexture(string imagePath)
    {
        if (textureDictionary == null)
        {
            textureDictionary = new Dictionary<string, Texture2D>();
        }
        Texture2D texture = null;
        if (!textureDictionary.ContainsKey(imagePath))
        {
            texture = Resources.Load<Texture2D>(imagePath);
            textureDictionary.Add(imagePath, texture);
        }
        else
        {
            texture = textureDictionary[imagePath];
        }
        return texture;
    }
}
