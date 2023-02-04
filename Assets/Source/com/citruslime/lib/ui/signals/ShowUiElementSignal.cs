
using System;
using com.citruslime.lib.ui.vo;
using UnityEngine;

namespace com.citruslime.ui
{
    public class ShowUiElementSignal
    {
        public IUiViewVO ViewVO { get; private set; }

        public bool IsSaveVisibleUiElementToStack { get; private set; }

        public Action<GameObject> OnUiLoadedCallback { get; private set; }

        public ShowUiElementSignal (
                    IUiViewVO viewVO, 
                    Action<GameObject> onUiLoadedCallback, 
                    bool saveVisibleUiElementToStack = true
                ) 
        {
            ViewVO = viewVO;
            OnUiLoadedCallback = onUiLoadedCallback;
            IsSaveVisibleUiElementToStack = saveVisibleUiElementToStack;
        }

    }
    
}