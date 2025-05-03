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

    int productCount = 20;
    public TMP_FontAsset[] fonts;

    public static string[] genericTitles = { 
        "Great Deal!", 
        "Buy now!",
        "Limited Offer!", 
        "Dont miss out!", 
        "Save now!",
        "Best Offer!",
        "Order today!",
        "Best Gift!",
        "Amazing!",
        "Must Have!",
        "New Offer!"
    };


    // Start is called before the first frame update
    void Start()
    {

        title.text = genericTitles[Random.Range(0, genericTitles.Length)];
        price.text = "$"+Random.Range(9, 50)+".99";
        bottomTitle.text = genericTitles[Random.Range(0, genericTitles.Length)];


        title.color = randomTextColor();
        price.color = randomTextColor();
        bottomTitle.color = randomTextColor();
        bgImage.color = randomBGColor();
        prodImage.sprite = Resources.Load<Sprite>("prod_" + Random.Range(0, productCount));

        title.font = fonts[Random.Range(0, fonts.Length)];
        price.font = fonts[Random.Range(1, fonts.Length)];
        bottomTitle.font = fonts[Random.Range(0, fonts.Length)];

        gameObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-450, 450), Random.Range(-250, 250));

            Start();
        }
    }

    Color randomBGColor() {
        return new Color(Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f);
    }

    Color randomTextColor()
    {
        return new Color(Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f);
    }
}
