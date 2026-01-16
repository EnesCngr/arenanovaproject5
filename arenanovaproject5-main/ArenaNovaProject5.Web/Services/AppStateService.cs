using System;
using ArenaNovaProject5.Web.models;

namespace ArenaNovaProject5.Web.Services
{
    public class AppStateService
    {
        public ParentModel? CurrentUser { get; private set; }
        public ChildSubcollection? ActiveChild { get; private set; }
        public ChildProgressModel? ChildProgress { get; private set; }

        public event Action? OnChange;

        public void SetUserData(ParentModel user)
        {
            CurrentUser = user;
            NotifyStateChanged();
        }

        public void SetActiveChild(ChildSubcollection child)
        {
            ActiveChild = child;
            NotifyStateChanged();
        }

        public void SetChildProgress(ChildProgressModel progress)
        {
            ChildProgress = progress;
            NotifyStateChanged();
        }

        public void Clear()
        {
            CurrentUser = null;
            ActiveChild = null;
            ChildProgress = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}