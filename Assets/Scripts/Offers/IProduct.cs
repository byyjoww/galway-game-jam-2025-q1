using UnityEngine;

namespace Scamazon.Offers
{
    public interface IProduct
    {
        string ProductName { get; }
        string Description { get; }
        Sprite Icon { get; }
        int Score { get; }
    }
}
