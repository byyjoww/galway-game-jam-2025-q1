using UnityEngine;

namespace Scamazon.UI
{
    public class StartGameViewController : ViewController<StartGameView, App.App>
    {
        public StartGameViewController(StartGameView view, App.App model) : base(view, model)
        {

        }

        public override void Init()
        {
            ShowView();
            UpdateView();
        }

        private void UpdateView()
        {
            view.Setup(new StartGameView.PresenterModel
            {
                OnStart = delegate
                {
                    HideView();
                    model.StartGame();
                },

                OnQuit = delegate
                {
                    HideView();
                    Application.Quit();
                },
            });
        }

        public override void Dispose()
        {

        }
    }
}