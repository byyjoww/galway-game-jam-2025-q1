using UnityEngine;

namespace Scamazon.Offers
{
    [CreateAssetMenu(fileName = "ProductSO", menuName = "Scriptable Objects/Product")]
    public class ProductSO : ScriptableObject, IProduct
    {
        [SerializeField] private string productName = default;
        [SerializeField] private string description = default;
        [SerializeField] private Sprite icon = default;
        [SerializeField] private int score = default;

        public string ProductName => productName;
        public string Description => description;
        public Sprite Icon => icon;
        public int Score => score;
    }
}
