using System;
using com.citruslime.lib.ui.vo;
using UnityEngine;

namespace com.citruslime.lib.ui.view
{
    public interface IUiView
    {
        IUiViewVO UiViewVo { get; set; }

        bool IsScreenSpaceCamera { get; }

        bool InitialiseView (Action<IUiViewVO, bool> OnClosed);

        void Show();

        void Hide();

        void UpdateView();

        void SetScreenSpaceCamera (Camera camera);

        void Close (bool isShowUiFromStack=true);

        void StretchToParentSize();

        void TriggerAnimateIn (Action callback);

        void TriggerAnimateOut (Action callback);
    }

}
