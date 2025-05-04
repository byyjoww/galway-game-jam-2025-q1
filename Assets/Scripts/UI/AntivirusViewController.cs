using Scamazon.Virus;

namespace Scamazon.UI
{
    public class AntivirusViewController : ViewController<AntivirusView, Antivirus>
    {
        public AntivirusViewController(AntivirusView view, Antivirus model) : base(view, model)
        {

        }

        public override void Init()
        {
            model.OnVirusQuarantined += OnQuarantine;
        }

        private void OnQuarantine(Virus.Virus virus)
        {
            view.ShowQuarantined();
        }

        public override void Dispose()
        {
            model.OnVirusQuarantined -= OnQuarantine;
        }
    }
}