using Scamazon.Virus;
using System;

namespace Scamazon.UI
{
    public class AntivirusViewController : ViewController<AntivirusView, Antivirus>
    {
        public AntivirusViewController(AntivirusView view, Antivirus model) : base(view, model)
        {

        }

        public override void Init()
        {
            model.OnVirusDetected += OnDetected;
            model.OnVirusQuarantined += OnQuarantine;
        }

        private void OnDetected(Virus.Virus virus)
        {
            view.ShowDetected();
        }

        private void OnQuarantine(Virus.Virus virus)
        {
            view.ShowQuarantined();
        }

        public override void Dispose()
        {
            model.OnVirusDetected -= OnDetected;
            model.OnVirusQuarantined -= OnQuarantine;
        }
    }
}