using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModelType
{
      EASY, NOMAL,HARD
}
public class ResourceManager : TSingleton<ResourceManager> {

    public string model;
    public int level = 1;
    void Awake()
    {
        model = ModelType.EASY.ToString();
    }
	// Use this for initialization
    public Sprite LoadSprites(int index)
    {
        Sprite sprites;
        sprites = Resources.Load<Sprite>("Images/"+model+"/item" + level + index);
        return sprites;
    }
}
