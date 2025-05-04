using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnnoyingPopup : MonoBehaviour
{

    public static string[] genericTitles = {
        "Win a free car!",
        "Free holiday to Spain!",
        "You are the winner of $1,000,000!",
        "Free beans for life!",
        "You won a new blender!",
        "Update your will for free!",
        "Singles in your area!",
        "Singles are waiting for you!",
        "Lifetime supply of easy singles!",
    };


    public Sprite[] bgSprites;
    public Image bgImage;

    public Image shockImage;
    public TMP_Text title;

    // Start is called before the first frame update
    void Start()
    {
        shockImage.sprite = bgSprites[Random.Range(0, 3)];
        title.text = genericTitles[Random.Range(0, genericTitles.Length)];

        title.color = randomTextColor();
        bgImage.color = randomBGColor();

        LeanTween.scale(title.gameObject, Vector3.one * 1.3f, 0.4f)
        .setEaseInOutSine()
        .setLoopPingPong();

        LeanTween.rotateZ(title.gameObject, 30f, 0.5f)
        .setEaseInOutSine()
        .setLoopPingPong();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-450, 450), Random.Range(-250, 250));

            shockImage.sprite = bgSprites[Random.Range(0, 3)];
            title.text = genericTitles[Random.Range(0, genericTitles.Length)];

            title.color = randomTextColor();
            bgImage.color = randomBGColor();
        }
    }


    Color randomBGColor()
    {
        return new Color(Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f, Random.Range(100, 255f) / 255f);
    }

    Color randomTextColor()
    {
        return new Color(Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f, Random.Range(0, 120f) / 255f);
    }

    public void onClose() {
        Destroy(gameObject);
    }
}
