using UnityEngine;
using UnityEngine.UI;

namespace Scamazon.UI
{
    public class ButtonViewBase : Navigatable
    {
        [SerializeField] private Button buttonComponent = default;

        public Button Button => buttonComponent;

        public override GameObject NavigatableObj => buttonComponent.gameObject;

        private void OnValidate()
        {
            buttonComponent ??= GetComponentInChildren<Button>();
        }
    }
}