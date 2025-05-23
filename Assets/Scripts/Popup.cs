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
    public Image logo;

    int productCount = 20;
    public TMP_FontAsset[] fonts;
    public Sprite[] logos;

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
        logo.sprite = logos[Random.Range(0, logos.Length)];
        title.text = genericTitles[Random.Range(0, genericTitles.Length)];
        //price.text = "$"+Random.Range(9, 50)+".99";
        bottomTitle.text = genericTitles[Random.Range(0, genericTitles.Length)];


        title.color = randomTextColor();
        price.color = randomTextColor();
        bottomTitle.color = randomTextColor();
        bgImage.color = randomBGColor();
        //prodImage.sprite = Resources.Load<Sprite>("Products/prod_" + Random.Range(0, productCount));

        title.font = fonts[Random.Range(0, fonts.Length)];
        price.font = fonts[Random.Range(1, fonts.Length)];
        bottomTitle.font = fonts[Random.Range(0, fonts.Length)];

        //gameObject.transform.localScale = new Vector3(0, 0, 0);
        //LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.4f);

        LeanTween.scale(title.gameObject, Vector3.one * 1.3f, 0.4f)
        .setEaseInOutSine()
        .setLoopPingPong();

        LeanTween.scale(prodImage.gameObject, Vector3.one * 1.2f, 0.55f)
        .setEaseInOutSine()
        .setLoopPingPong();

        LeanTween.scale(bottomTitle.gameObject, Vector3.one * 1.2f, 0.3f)
        .setEaseInOutSine()
        .setLoopPingPong();

        LeanTween.rotateZ(prodImage.gameObject, 15f, 0.5f)
        .setEaseInOutSine()
        .setLoopPingPong();

    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-450, 450), Random.Range(-250, 250));

            Start();
        }
    }*/

    Color randomBGColor() {
        return new Color(Random.Range(150, 255f) / 255f, Random.Range(150, 255f) / 255f, Random.Range(150, 255f) / 255f);
    }

    Color randomTextColor()
    {
        return new Color(Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f);
    }
}
