using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Scamazon.UI
{
    public class CurrencyView : View
    {
        public struct PresenterModel
        {
            public string Value { get; set; }
        }

        [SerializeField] private TMP_Text value = default;

        internal void Setup(PresenterModel model)
        {
            value.text = model.Value;
        }
    }
}