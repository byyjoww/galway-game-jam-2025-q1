using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scamazon.UI
{
    public class OfferView : View
    {
        public struct PresenterModel
        {
            public string OfferID { get; set; }
            public int OfferIndex { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string HyperlinkText { get; set; }
            public string DeliveryDate { get; set; }
            public string URL { get; set; }
            public Sprite ProductIcon { get; set; }
            public string ImageHeader1 { get; set; }
            public string ImageHeader2 { get; set; }
            public int Stars { get; set; }
            public string NumOfReviews { get; set; }
            public IEnumerable<ReviewView.PresenterModel> Reviews { get; set; }
            public bool CanBuy { get; set; }
            public UnityAction OnBuy { get; set; }
            public UnityAction OnSkip { get; set; }
        }

        [SerializeField] private GameObject panel = default;
        [SerializeField] private TMP_Text productName = default;
        [SerializeField] private TMP_Text productDescription = default;
        [SerializeField] private TMP_Text hyperlinkText = default;
        [SerializeField] private TMP_Text url = default;
        [SerializeField] private Image productIcon = default;
        [SerializeField] private TMP_Text imageHeader1 = default;
        [SerializeField] private TMP_Text imageHeader2 = default;
        [SerializeField] private TMP_Text numOfReviews = default;
        [SerializeField] private Transform reviewsRoot = default;
        [SerializeField] private ReviewView reviewTemplate = default;
        [SerializeField] private ButtonViewBase buy = default;
        [SerializeField] private ButtonViewBase skip = default;
        [SerializeField] private Image[] stars = default;

        [Header("Icons")]
        [SerializeField] private Sprite emptyStar = default;
        [SerializeField] private Sprite halfStar = default;
        [SerializeField] private Sprite fullStar = default;

        private Dictionary<string, ReviewView> instantiated = new Dictionary<string, ReviewView>();

        private void Awake()
        {
            reviewTemplate.gameObject.SetActive(false);
        }

        public void Setup(PresenterModel model)
        {
            panel.gameObject.SetActive(model.OfferID != "");
            productName.text = model.ProductName;
            productDescription.text = model.ProductDescription;
            hyperlinkText.text = model.HyperlinkText;
            url.text = model.URL;
            productIcon.sprite = model.ProductIcon;
            imageHeader1.text = model.ImageHeader1;
            imageHeader2.text = model.ImageHeader2;
            SetStars(model.Stars);
            numOfReviews.text = model.NumOfReviews;
            SetReviews(model.Reviews, true);
            SetButtonAction(buy, model.OnBuy, model.CanBuy);
            SetButtonAction(skip, model.OnSkip);
        }

        private void SetStars(int numOfStars)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                int starValue = numOfStars - (i * 2);
                if (starValue >= 2)
                {
                    stars[i].sprite = fullStar;
                }
                else if (starValue == 1)
                {
                    stars[i].sprite = halfStar;
                }
                else
                {
                    stars[i].sprite = emptyStar;
                }
            }
        }

        private void SetReviews(IEnumerable<ReviewView.PresenterModel> reviews, bool rebuild = false)
        {
            if (rebuild)
            {
                for (int i = instantiated.Count; i-- > 0;)
                {
                    var obj = instantiated.ElementAt(i);
                    Destroy(obj.Value.gameObject);
                }

                instantiated.Clear();
            }

            for (int i = 0; i < reviews.Count(); i++)
            {
                var pm = reviews.ElementAt(i);
                if (instantiated.TryGetValue(pm.Reviewer, out var view))
                {
                    view.Setup(pm);
                }
                else
                {
                    Create(pm);
                }
            }
        }

        private void Create(ReviewView.PresenterModel pm)
        {
            ReviewView rv = Instantiate(reviewTemplate, reviewsRoot);
            rv.Setup(pm);
            rv.gameObject.SetActive(true);
            instantiated.Add(pm.Reviewer, rv);
        }
    }
}