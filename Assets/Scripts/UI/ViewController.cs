using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scamazon.UI
{
    public abstract class ViewController<T1> : IDisposable where T1 : View
    {
        protected T1 view = default;

        public ViewController(T1 view)
        {
            this.view = view;
        }

        public abstract void Init();

        public virtual void ShowView()
        {
            view.SetActive(true);
        }

        public virtual void HideView()
        {
            view.SetActive(false);
        }

        public abstract void Dispose();
    }

    public abstract class ViewController<T1, T2> : IDisposable where T1 : View
    {
        protected T1 view = default;
        protected T2 model = default;

        public ViewController(T1 view, T2 model)
        {
            this.view = view;
            this.model = model;
        }

        public abstract void Init();

        public virtual void ShowView()
        {
            view.SetActive(true);
        }

        public virtual void HideView()
        {
            view.SetActive(false);
        }

        public abstract void Dispose();
    }
}