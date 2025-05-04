using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.UI
{
    public class StartGameView : View
    {
        public struct PresenterModel
        {
            public UnityAction OnStart { get; set; }
            public UnityAction OnQuit { get; set;}
        }

        [SerializeField] private ButtonViewBase startGame = default;
        [SerializeField] private ButtonViewBase quit = default;

        public void Setup(PresenterModel model)
        {
            SetButtonAction(startGame, model.OnStart);
            SetButtonAction(quit, model.OnQuit);
        }
    }
}