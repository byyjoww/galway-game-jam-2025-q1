using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text price;
    public TMP_Text bottomTitle;
    public Image bgImage;
    public Image prodImage;

    int fontCount = 4;
    int productCount = 20;
    public TMP_FontAsset[] fonts;

    public static string[] genericTitles = { 
        "Great Deal!", 
        "Buy now!",
        "Limited Offer!", 
        "Dont miss out!", 
        "Save now!" 
    };


    // Start is called before the first frame update
    void Start()
    {
        title.text = genericTitles[Random.Range(0, 5)];
        price.text = "$"+Random.Range(9, 50)+".99";
        bottomTitle.text = genericTitles[Random.Range(0, 5)];


        title.color = randomColor();
        price.color = randomColor();
        bottomTitle.color = randomColor();
        bgImage.color = new Color(Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f);
        prodImage.sprite = Resources.Load<Sprite>("prod_" + Random.Range(0, productCount));

        title.font = fonts[Random.Range(0, fontCount)];
        price.font = fonts[Random.Range(0, fontCount)];
        bottomTitle.font = fonts[Random.Range(0, fontCount)];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Start();
        }
    }

    Color randomColor() {
        return new Color(Random.Range(0, 255f) / 255f, Random.Range(0, 255f) / 255f, Random.Range(0, 255f) / 255f);
    }
}
