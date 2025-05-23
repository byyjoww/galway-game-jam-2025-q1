﻿using Scamazon.Audio;
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
            public bool IsNull { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string HyperlinkText { get; set; }
            public string DeliveryDate { get; set; }
            public string URL { get; set; }
            public string ProductPrice { get; set; }
            public Sprite ProductIcon { get; set; }
            public string OfferPrice { get; set; }
            public string ImageHeader1 { get; set; }
            public string ImageHeader2 { get; set; }
            public int Stars { get; set; }
            public string NumOfReviews { get; set; }
            public IEnumerable<ReviewView.PresenterModel> Reviews { get; set; }
            public bool CanBuy { get; set; }
            public UnityAction OnBuy { get; set; }
            public UnityAction OnSkip { get; set; }
        }

        [SerializeField] private PanelPopIn panel = default;
        [SerializeField] private TMP_Text productName = default;
        [SerializeField] private TMP_Text productDescription = default;
        [SerializeField] private TMP_Text hyperlinkText = default;
        [SerializeField] private GameObject tooltip = default;
        [SerializeField] private TMP_Text url = default;
        [SerializeField] private TMP_Text price = default;
        [SerializeField] private Image productIcon = default;
        [SerializeField] private TMP_Text deliveryDate = default;
        [SerializeField] private TMP_Text imagePrice = default;
        [SerializeField] private TMP_Text imageHeader1 = default;
        [SerializeField] private TMP_Text imageHeader2 = default;
        [SerializeField] private TMP_Text numOfReviews = default;
        [SerializeField] private Transform reviewsRoot = default;
        [SerializeField] private ReviewView reviewTemplate = default;
        [SerializeField] private ButtonViewBase buy = default;
        [SerializeField] private ButtonViewBase skip = default;
        [SerializeField] private ButtonViewBase hover = default;
        [SerializeField] private Image[] stars = default;

        [Header("Icons")]
        [SerializeField] private Sprite emptyStar = default;
        [SerializeField] private Sprite halfStar = default;
        [SerializeField] private Sprite fullStar = default;

        private bool isEmpty = true;
        private Dictionary<string, ReviewView> instantiated = new Dictionary<string, ReviewView>();

        private void Awake()
        {
            reviewTemplate.gameObject.SetActive(false);
        }

        public void Setup(PresenterModel model)
        {
            panel.gameObject.SetActive(!model.IsNull);
            if (!panel.gameObject.activeSelf)
            {
                isEmpty = true;
                return;
            }
                        
            isEmpty = false;
            productName.text = model.ProductName;
            productDescription.text = model.ProductDescription;
            hyperlinkText.text = model.HyperlinkText;
            url.text = model.URL;
            price.text = model.ProductPrice;
            productIcon.sprite = model.ProductIcon;
            deliveryDate.text = model.DeliveryDate;
            imagePrice.text = model.OfferPrice;
            imageHeader1.text = model.ImageHeader1;
            imageHeader2.text = model.ImageHeader2;
            SetStars(model.Stars);
            numOfReviews.text = model.NumOfReviews;
            SetReviews(model.Reviews, true);
            SetButtonAction(buy, model.OnBuy, model.CanBuy);
            SetButtonAction(skip, model.OnSkip);
            SetHoverTooltip();
            PlayShowSFX();
        }

        private void SetHoverTooltip()
        {
            tooltip.SetActive(false);
            hover.Init(audioPlayer);
            hover.onPointerEnter.RemoveAllListeners();
            hover.onPointerEnter.AddListener(delegate
            {
                tooltip.SetActive(true);
            });

            hover.onPointerExit.RemoveAllListeners();
            hover.onPointerExit.AddListener(delegate
            {
                tooltip.SetActive(false);
            });
        }

        //private void Update()
        //{
        //    if (tooltip.activeSelf)
        //    {
        //        tooltip.transform.position = hyperlinkText.transform.position + new Vector3(10f, -20f, 0f);
        //    }
        //}

        public void Destroy()
        {            
            if (isEmpty)
            {
                DestroySelf();
            }
            else
            {
                PlayHideSFX();
                transform.SetParent(transform.parent.parent);
                panel.PlayPopOut();
                Invoke(nameof(DestroySelf), 1f);
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
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