using System;
using com.citruslime.lib.ui.vo;
using UnityEngine;

namespace com.citruslime.lib.ui.signal
{
    public class HideUiElementSignal
    {
        public IUiViewVO ViewVO { get; private set; }

        public Action OnUiHiddenCallback { get; private set; }

        public HideUiElementSignal (IUiViewVO viewVO, Action onUiHiddenCallback) 
        {
            ViewVO = viewVO;
            OnUiHiddenCallback = onUiHiddenCallback;
        }
        
    }

}