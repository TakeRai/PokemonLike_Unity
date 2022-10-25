using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] List<Sprite> backgrounds;

    public Image image;
    // Start is called before the first frame update
    

    public void BGchoose()
    {
        int r = Random.Range(0, backgrounds.Count - 1);

        image.sprite = backgrounds[r];

    }
}
