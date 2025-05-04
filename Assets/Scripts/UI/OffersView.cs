using Scamazon.Audio;
using SLS.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scamazon.UI
{
    public class OffersView : View
    {
        public struct PresenterModel
        {
            public IEnumerable<OfferView.PresenterModel> Offers { get; set; }
        }

        [SerializeField] private Transform offersRoot = default;
        [SerializeField] private OfferView offerTemplate = default;

        [Header("Audio")]
        [SerializeField] private SoundlistSO onBuy = default;

        private Dictionary<string, OfferView> instantiated = new Dictionary<string, OfferView>();

        private void Awake()
        {
            offerTemplate.gameObject.SetActive(false);
        }

        public void Setup(PresenterModel model)
        {
            SetOffers(model.Offers, false);
        }

        public void SetOffers(IEnumerable<OfferView.PresenterModel> offers, bool rebuild = false)
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
            else
            {
                var currentAvailableOffers = offers.Select(x => x.OfferID).ToArray();
                var currentInstantiatedOffers = instantiated.Keys;
                var toRemove = currentInstantiatedOffers.Except(currentAvailableOffers).ToArray();
                for (int i = toRemove.Length; i-- > 0;)
                {
                    var id = toRemove.ElementAt(i);
                    var obj = instantiated[id];
                    instantiated.Remove(id);
                    obj.Destroy();
                }
            }

            for (int i = 0; i < offers.Count(); i++)
            {
                var pm = offers.ElementAt(i);
                if (instantiated.TryGetValue(pm.OfferID, out var view))
                {
                    // view.Setup(pm);
                }
                else
                {
                    Create(pm);
                }
            }
        }

        public void PlayPurchaseSFX()
        {
            PlayOneShotAudio(onBuy);
        }

        private void Create(OfferView.PresenterModel pm)
        {
            OfferView rv = Instantiate(offerTemplate, offersRoot);
            rv.Setup(pm);
            rv.SetAudioPlayer(audioPlayer);
            rv.SetButtonAudio(buttonSfx);
            rv.gameObject.SetActive(true);
            rv.transform.SetSiblingIndex(pm.OfferIndex);
            instantiated.Add(pm.OfferID, rv);
        }
    }
}