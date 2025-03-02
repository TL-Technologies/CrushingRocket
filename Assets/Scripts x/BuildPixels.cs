using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPixels : MonoBehaviour
{
    public Texture2D[] textures;
    public Transform parent;

    public GameObject partObj;
    public float delta = .2f;
    public List<PartSc> parts = new List<PartSc>();

    public void Start()
    {
        if (PlayerPrefs.GetInt("Pixel") == -1 || PlayerPrefs.GetInt("Pixel") >= textures.Length)
            PlayerPrefs.SetInt("Pixel", Random.Range(0, textures.Length));


        Texture2D texture = textures[PlayerPrefs.GetInt("Pixel")];
        

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixel = texture.GetPixel(x, y);
                if (pixel.a>0)
                {
                    PartSc part = Instantiate(partObj, new Vector3((delta * (x - texture.width / 2)) + parent.position.x, delta * y + parent.position.y, parent.position.z), Quaternion.identity, parent).GetComponent<PartSc>();
                    part.SetColor(pixel);
                    parts.Add(part);
                }
            }
        }
    }

    public void NextPixelBuild()
    {
        foreach (PartSc item in parts)
        {
            Destroy(item.gameObject);
        }
        parts = new List<PartSc>();

        PlayerPrefs.SetInt("Pixel", PlayerPrefs.GetInt("Pixel")+1);

        if (PlayerPrefs.GetInt("Pixel") == -1 || PlayerPrefs.GetInt("Pixel") >= textures.Length)
            PlayerPrefs.SetInt("Pixel", 0);

        Texture2D texture = textures[PlayerPrefs.GetInt("Pixel")];

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color pixel = texture.GetPixel(x, y);
                if (pixel.a > 0)
                {
                    PartSc part = Instantiate(partObj, new Vector3((delta * (x - texture.width / 2)) + parent.position.x, delta * y + parent.position.y, parent.position.z), Quaternion.identity, parent).GetComponent<PartSc>();
                    part.SetColor(pixel);
                    parts.Add(part);
                }
            }
        }
    }

    public void CrushPart(PartSc part)
    {
        parts.Remove(part);
        if (parts.Count==0)
        {
            GameManager.instance.AllCrushed();
        }
    }
}
