using System;
using UnityEngine;
using com.citruslime.lib.ui.vo;
using System.Collections.Generic;
using com.citruslime.lib.ui.util;
using com.citruslime.lib.ui.view;
using com.citruslime.lib.ui.vo.alert;
using com.citruslime.ui;
using com.citruslime.lib.ui.signal;
using com.citruslime.lib.coroutine;
using com.citruslime.lib.assetmanagement;
using com.citruslime.lib.dependencyHero;
using com.citruslime.lib.ui.enums;

namespace com.citruslime.lib.ui.manager
{
    /// <summary>
    /// UI Manager class to manage the UI elements in the game
    /// </summary>
    public class UiManager 
    {
        private SignalBus signalBus = null;

        // injected asset factory to instantiate classes

        private AlertViewPool alertViewPool = null;

        private List<GameObject> usedAlertViews = null;

        // reference to coroutine service
        private CoroutineService coroutineService = null;

       // private EasyTouchInputService inputService = null;

        // get count of number of elements to laod
        private int itemsToLoadCount = 0;

        // dictionary to maintain reference to all UI available in the pool
        private Dictionary<string, GameObject> uiElementsPoolMap = null;

        // dictionary to maintain reference to all UI being used
        private Dictionary<string, GameObject> uiElementsUsedMap = null;

        // stack to store order in which UI was created
        private Stack<IUiViewVO> uiViewVoStack = null;

        // name of game object which helps with layering
        private string uiContainerGoPrefabName = "UILayersContainer";

        // actual reference to loaded game object
        private GameObject uiContainerGo = null;
        
        // various layers, self explanatory
        private GameObject layerHudsGo         = null;
        private GameObject layerPanelsGo       = null;
        private GameObject layerPopupsGo       = null;
        private GameObject layerAlertsGo       = null;
        private GameObject layerEffectsGo      = null;
        private GameObject layerDialogsGo      = null;
        private GameObject layerOverlaysGo     = null;
        private GameObject layerScreenSpaceGo  = null;
        private GameObject layerContextMenusGo = null;

        private GameObject alertsContainerGo  = null;

        // game object to hold pooled UI elements
        private GameObject uiElementsPool = null;

        // is there a dialog or popup currently being displayed
        private GameObject currentDialogOrPopupInFocus = null;

        //reset state save when app focus manager goes back to bootstrap
        private bool loaded = false;

        // informs the loading system of current status of loading
        public float LoadingProgress { get { return (float) ( (float) uiElementsPoolMap.Count / (float) itemsToLoadCount ) * 100f; } }

        // status message for loading screen
        public string LoadingMessage { get { return "Preparing UI ..."; } }

        //this says if the ui manager is not reset by app focus manager sequence 
        //used for checks in metagame on Dispose where certain ui view gets closed
        public bool IsLoaded { get { return loaded; }}


        private AssetFactory assetFactory = null;
        
        // injectable constructor

   

        public UiManager()
        {
            DependencyInjector.Instance.InjectDependencies(this);
            uiViewVoStack = new Stack<IUiViewVO>();
            signalBus = new SignalBus();
            usedAlertViews = new List<GameObject>();

            uiElementsPoolMap = new Dictionary<string, GameObject>();
            uiElementsUsedMap = new Dictionary<string, GameObject>();

            StartLoading( );

            signalBus.ShowTransmissionSignalEvent  +=  onShowUiElement;
            signalBus.HideTransmissionSignalEvent += onHideUiElement ;
        }

        [Inject]
        void inject(AssetFactory _assetFactory)
        {
            assetFactory = _assetFactory;
        }
        /// <summary>
        /// Start loading this service and all needed elements. Plugging into Ordered Init Service
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onFailure"></param>
        public void StartLoading ()// Action onSuccess, Action<string> onFailure - plug this in after ordered init class is made 
        {

            

            

            //onSuccess();

            GameObject uiManagerContainer = assetFactory.Instantiate("UI/containers/[UIManager]");
            GameObject uiContainerGo = assetFactory.Instantiate("UI/containers/UILayersContainer");
            uiElementsPool =  assetFactory.Instantiate("UI/containers/UIElementsPool");
            
            layerDialogsGo = uiContainerGo.transform.Find ("LayerModalDialogs").gameObject;
            uiContainerGo.transform.SetParent(uiManagerContainer.transform);
            uiElementsPool.transform.SetParent(uiManagerContainer.transform);
            
            GameObject.DontDestroyOnLoad(uiManagerContainer);


            loaded = true;
            // // first instantiate the root asset
            // assetFactory.Instantiate (
            //         // layer container prefab
            //         uiContainerGoPrefabName,
            //         // callback
            //         (assetName, containerGo) =>
            //         {
            //             // check if container has been loaded
            //             if (containerGo != null)
            //             {
            //                 // cache container reference
            //                 uiContainerGo = containerGo;
                            
            //                 // extract references to all layers
            //                 layerHudsGo = uiContainerGo.transform.Find ("LayerHuds").gameObject;
            //                 layerPanelsGo = uiContainerGo.transform.Find ("LayerPanels").gameObject;
            //                 layerPopupsGo = uiContainerGo.transform.Find ("LayerModalPopups").gameObject;
            //                 layerAlertsGo = uiContainerGo.transform.Find ("LayerAlerts").gameObject;
            //                 layerEffectsGo = uiContainerGo.transform.Find ("LayerEffects").gameObject;
            //                 layerDialogsGo = uiContainerGo.transform.Find ("LayerModalDialogs").gameObject;
            //                 layerOverlaysGo = uiContainerGo.transform.Find ("LayerOverlays").gameObject;
            //                 layerScreenSpaceGo = uiContainerGo.transform.Find ("LayerScreenSpace").gameObject;
            //                 layerContextMenusGo = uiContainerGo.transform.Find ("LayerContextMenus").gameObject;
                            
            //                 // get reference to pooling layer
            //                 uiElementsPool = uiContainerGo.transform.Find ("UiElementsPool").gameObject;
                            
            //                 // the container in which alerts will be placed
            //                 alertsContainerGo = uiContainerGo.transform.Find ("LayerAlerts/AlertsContainer").gameObject;

            //                 // setup the screen space layer once it is loaded
            //                 setupScreenSpaceLayer (null);

            //                 // hide all the layers
            //                 layerHudsGo.SetActive (true);
            //                 layerPanelsGo.SetActive (true);
            //                 layerPopupsGo.SetActive (true);
            //                 layerAlertsGo.SetActive (true);
            //                 layerEffectsGo.SetActive (true);
            //                 layerDialogsGo.SetActive (true);
            //                 layerOverlaysGo.SetActive (true);
            //                 layerScreenSpaceGo.SetActive (true);
            //                 layerContextMenusGo.SetActive (true);
                    
            //                 alertsContainerGo.SetActive (true);

            //                 uiElementsPool.SetActive (false);

            //                 // loading completed
            //                 loaded = true;

            //                 onSuccess();
            //             }
            //             else
            //             {
            //                 // if not we have a problem
            //                 onFailure ( $"Could Not Load UI Container Game Object '{assetName}'" );
            //             }
            //         }
           // );
        }

        //going to background processing
        public void GoingToBackground (Action onSuccess, Action<string> onFailure)
        {
            onSuccess();
        }

        //coming from background processing make sure you pause the game when this happens , maybe make an event 
        public void ReturningToForeground (Action onSuccess, Action<string> onFailure)
        {
            onSuccess();
        }

        //resetting everything we made by the service, what service man
        //removing dont destroy on load and other stuff
        public void Reset (Action onSuccess, Action<string> onFailure)
        {
            //reset all we have make sure to ask solo on how pacman does Ui and 
            uiElementsPoolMap.Clear();
            uiElementsUsedMap.Clear();
            uiViewVoStack.Clear();
            
            GameObject.Destroy (uiContainerGo);

            //we are not loaded anymore
            loaded = false;

            log ( "Ui Manager Reset Done ..." );
            
            onSuccess?.Invoke();
        }

        /// <summary>
        /// Convenience Function
        /// </summary>
        /// <param name="viewVo"></param>
        /// <param name="onUiLoadedCallback"></param>
        public void Show (IUiViewVO viewVo, Action<GameObject> onUiLoadedCallback) 
        {
            Show (viewVo, true, onUiLoadedCallback);
        }

        /// <summary>
        /// Method to show the UI element requested
        /// </summary>
        /// <param name="viewVo"></param>
        /// <param name="isSaveVisibleDialogOrPopupToStack"></param>
        /// <param name="onUiLoadedCallback"></param>
        public void Show (
                        IUiViewVO viewVo, 
                        bool isSaveVisibleDialogOrPopupToStack=true, 
                        Action<GameObject> onUiLoadedCallback=null
                    ) 
        {
            // check if the UI element has already been loaded
            if ( viewVo.UiType != UiTypesEnum.Alert 
                    && !isUiElementAlreadyLoaded (viewVo) ) 
            {
                    
                    GameObject uiElementGo = assetFactory.Instantiate( "UI" +"/" + "prefabs" +  "/" + viewVo.UiType + "/" + viewVo.UiElementName);
                    uiElementsPoolMap.Add ( viewVo.UiElementName, uiElementGo );
                    processUiElement (viewVo, isSaveVisibleDialogOrPopupToStack, onUiLoadedCallback);
                // // safety check
                // if ( assetFactory != null 
                //         && uiElementsPool != null ) 
                // {

                //     var a = Resources.Load(viewVo.UiElementName);

                //      Instantiate(a);
                    // // load the asset from the asset bundle
                    // assetFactory.Instantiate ( 
                    //         viewVo.UiElementName, 
                    //         Vector3.zero, 
                    //         Quaternion.identity, 
                    //         uiElementsPool.transform, 
                    //         (uiAssetName, uiElementGo) => 
                    //         {
                    //             // save loaded element into the pool
                    //             if (uiElementGo != null) 
                    //             {
                    //                 // deactivate the UI Element
                    //                 uiElementGo.SetActive (false);
                    //                 // cache it in the pool
                    //                 uiElementsPoolMap.Add ( viewVo.UiElementName, uiElementGo );
                    //                 // now that it is loaded and cached, process the UI Element
                    //                 processUiElement (viewVo, isSaveVisibleDialogOrPopupToStack, onUiLoadedCallback);
                    //             }
                    //             else 
                    //             {
                    //                 log ( $"Failed to load UI element '{uiAssetName}'" );
                    //             }
                    //         } 
                    //     );
                // }
            }
            else 
            {
                // UI element has already been loaded successfully, now process it
                processUiElement (viewVo, isSaveVisibleDialogOrPopupToStack, onUiLoadedCallback);
            }
        }

        /// <summary>
        /// Hide the UI element specified
        /// </summary>
        /// <param name="viewVo"></param>
        public void Hide (IUiViewVO viewVo, Action onUiElementHidden = null) 
        {
            // safety check bacchoon , nahi to code toot jayega 
            if (viewVo != null) 
            {
                // get the element name
                string uiElementName = viewVo.UiElementName;

                // check if it is currently used
                if ( uiElementsUsedMap.ContainsKey (uiElementName) ) 
                {
                    // get a reference to the UI element
                    GameObject uiElementGo = uiElementsUsedMap [uiElementName];

                    // get a reference to the view component
                    IUiView viewComponent = uiElementGo.transform.GetComponent<IUiView>();
                    
                    // tell the view component that the dialog is being closed
                    viewComponent.Close();

                    // if we are dealing with an alert view
                    if ( viewComponent.UiViewVo.UiType == UiTypesEnum.Alert ) 
                    {
                        //alertViewPool.Despawn ( (DefaultAlertView) viewComponent );
                    }

                    // trigger callback if we have one
                    onUiElementHidden?.Invoke();
                }
                else 
                {
                    log ( $"Unable to find '{uiElementName}' with id - '{viewVo.ID}'" );
                }
            }
        }
        
        /// <summary>
        /// Gets the size of the user interface layer.
        /// </summary>
        /// <returns>The user interface layer size.</returns>
        /// <param name="uiType">Layer (UI type).</param>
        public Vector2 GetUiLayerCanvasSize (UiTypesEnum uiType)
        {
            GameObject layerObject = getUiElementLayer (uiType);

            return (layerObject.transform as RectTransform).sizeDelta;
        }

        /// <summary>
        /// Process given UI element according to type and display the UI
        /// </summary>
        /// <param name="viewVo"></param>
        /// <param name="isSaveVisibleDialogOrPopupToStack"></param>
        /// <param name="onUiLoadedCallback"></param>
        private void processUiElement (
                            IUiViewVO viewVo, 
                            bool isSaveVisibleDialogOrPopupToStack=true, 
                            Action<GameObject> onUiLoadedCallback=null
                        ) 
        {
            // reference to UI element, to be returned
            GameObject uiElement = null;

            // check the type of the ui element
            if ( viewVo.UiType == UiTypesEnum.Dialog 
                    || viewVo.UiType == UiTypesEnum.Popup ) 
            {
                // should the currently visible dialog be saved to stack?
                if ( isSaveVisibleDialogOrPopupToStack ) 
                {
                    // if dialog or popup, do some processing
                    saveCurrentDialogOrPopupToStack();
                }

                // cache reference to the current dialog or popup
                currentDialogOrPopupInFocus = showUiElement (viewVo);
                // now get reference to be returned
                uiElement = currentDialogOrPopupInFocus;

                // if callback is provided, trigger it
                onUiLoadedCallback?.Invoke (uiElement);
            }
            else 
            {
                // else just process the ui element as normal
                uiElement = showUiElement (viewVo);

                // if callback is provided, trigger it
                onUiLoadedCallback?.Invoke (uiElement);
            }
        }

        /// <summary>
        /// Show the specified UI Element which is achieved by passing the View VO
        /// </summary>
        /// <param name="viewVo"></param>
        /// <returns>Reference to Prefab of currently shown UI Element</returns>
        private GameObject showUiElement (IUiViewVO viewVo)
        {
            // ui element to return
            GameObject uiElementGo =  null;

            // check if the view vo is not null
            if ( viewVo != null ) 
            {
                // get the name of the UI Element
                string uiElementName = viewVo.UiElementName;
                
                // get the layer into which this UI element has to be placed in
                GameObject layer = getUiElementLayer (viewVo.UiType);

                // check if this UI element is in the Pool
                if ( viewVo.UiType == UiTypesEnum.Alert 
                        || uiElementsPoolMap.ContainsKey (uiElementName) ) 
                {
                    // check if we are dealing with an alert
                    if ( viewVo.UiType == UiTypesEnum.Alert )
                    {
                        //uiElementGo = alertViewPool.Spawn ( (DefaultAlertViewVO) viewVo ).gameObject;
                    }
                    else 
                    {
                        // ok we are not dealing with an alert
                        // yes, it is in the pool, get reference to it
                        uiElementGo = uiElementsPoolMap [uiElementName];
                        // now remove it from the pool
                        uiElementsPoolMap.Remove (uiElementName);
                    }

                    // parent it to the correct layer, probably this is cause for 
                    // some performance hit, open to suggestions to improve this
                    uiElementGo.transform.SetParent (layer.transform);
                    
                    // get the reference to the View Component to initialise it and also set the VO
                    IUiView viewComponent = uiElementGo.transform.GetComponent<IUiView>();

                    // check if the UI is supposed to be an overlay or in screen space
                    if ( !viewComponent.IsScreenSpaceCamera ) 
                    {
                        // parent it to the correct layer, probably this is cause for 
                        // some performance hit, open to suggestions to improve this
                        uiElementGo.transform.SetParent (layer.transform);
                    }
                    else 
                    {
                        // parent it to the correct layer, probably this is cause for 
                        // some performance hit, open to suggestions to improve this
                        uiElementGo.transform.SetParent (layerScreenSpaceGo.transform);

                        // set the camera if needed, in case UI element is in screen space camera
                        viewComponent.SetScreenSpaceCamera (Camera.main);
                    }

                    // now force the view component to stretch to parent container dimentions
                    viewComponent.StretchToParentSize();                
                    // set the View VO object in the UI element, 
                    // this also forces view to update itself
                    viewComponent.UiViewVo = viewVo;
                    // now initialise the view
                    viewComponent.InitialiseView (hideUiElement);
                    // now update the view
                    viewComponent.UpdateView ();
                    // now tell the view component to show itself
                    viewComponent.Show();

                    // add this UI Element to the UI Map only if it is not an Alert
                    if ( viewVo.UiType != UiTypesEnum.Alert ) 
                    {
                        // now add UI element to the used map
                        uiElementsUsedMap.Add (uiElementName, uiElementGo);
                    }
                    else 
                    {
                        usedAlertViews.Add (uiElementGo);
                    }
                }
                else 
                {
                    // check if the used map contains this element, we did not find it in the unused pool
                    if ( viewVo.UiType != UiTypesEnum.Alert 
                            && uiElementsUsedMap.ContainsKey (uiElementName) ) 
                    {
                        // we have it in used pool, now get a reference
                        uiElementGo = uiElementsUsedMap [uiElementName];
                        
                        // get reference to the view component
                        IUiView viewComponent = uiElementGo.transform.GetComponent<IUiView>();
                        // now set / update the View VO
                        viewComponent.UiViewVo = viewVo;
                    }
                    else 
                    {
                        // we could not find the UI Element anywhere, hence log it
                        log ( $"Could not find the UI element '{uiElementName}' in used or unused pool", LogType.Error );
                    }
                }
            }

            // we have a valid UI element which has been launched
            if ( uiElementGo != null 
                    && viewVo.UiType != UiTypesEnum.Alert ) 
            {
                // fire signal to notify that UI Element has been opened
                //signalBus.Fire ( new UiElementOpenedSignal (viewVo.UiType) );
            }

            return uiElementGo;
        }

        // private void setupScreenSpaceLayer (SceneInitializedSignal signal) 
        // {
        //     if ( layerScreenSpaceGo != null ) 
        //     {
        //         Canvas canvas = layerScreenSpaceGo.GetComponent<Canvas>();

        //         if (canvas != null) 
        //         {
        //             canvas.renderMode    = RenderMode.ScreenSpaceCamera;
        //             canvas.worldCamera   = Camera.main;
        //             canvas.planeDistance = 10;
        //         }
        //     }
        // }

        /// <summary>
        /// Hide the UI Element specified by the View VO
        /// </summary>
        /// <param name="uiViewVo"></param>
        private void hideUiElement (IUiViewVO uiViewVo, bool isPopTopmostUiFromStack=true) 
        {
            bool isUiElementClosed = false;

            // get the element name
            string uiElementName = uiViewVo.UiElementName;

            // check if it is currently used
            if ( uiViewVo.UiType == UiTypesEnum.Alert 
                    || uiElementsUsedMap.ContainsKey (uiElementName) ) 
            {
                // get a reference to the UI element
                GameObject uiElementGo = null;

                if ( uiViewVo.UiType != UiTypesEnum.Alert ) 
                {
                    // get a reference to the UI element
                    uiElementGo = uiElementsUsedMap [uiElementName];
                    // remove it from the used pool
                    uiElementsUsedMap.Remove (uiElementName);
                    // return the game object to the pool of UI elements
                    uiElementsPoolMap.Add (uiElementName, uiElementGo);
                } 
                else 
                {
                    uiElementGo = getAlertView (uiViewVo);

                    if ( uiViewVo.UiType == UiTypesEnum.Alert ) 
                    {
                       // alertViewPool.Despawn ( uiElementGo.GetComponent<DefaultAlertView>() );

                        usedAlertViews.Remove (uiElementGo);
                    }
                }

                // get the reference to the View Component
                IUiView viewComponent = uiElementGo.transform.GetComponent<IUiView>();
                // tell the view component to hide itself
                viewComponent.Hide();

                // remove it from the used pool
                uiElementsUsedMap.Remove (uiElementName);
                // set the transform / parent to the pool layer
                uiElementGo.transform.SetParent (uiElementsPool.transform);
                // deactivate the game object
                uiElementGo.SetActive (false);

                // flag to indicate that UI Element has been closed
                isUiElementClosed = true;

                // now check if the view is a dialog or popup
                if ( uiViewVo.UiType == UiTypesEnum.Dialog 
                        || uiViewVo.UiType == UiTypesEnum.Popup ) 
                {
                    // mark variable as null
                    currentDialogOrPopupInFocus = null;

                    // now if we are supposed to, pop the previous 
                    // dialog or popup which is in the stack
                    if ( isPopTopmostUiFromStack 
                            && uiViewVoStack.Count > 0 ) 
                    {
                        // now show this dialog / popup
                        Show ( uiViewVoStack.Pop() );
                    }
                }
            }
            else 
            {
                // if a dialog or popup not currently visible has asked to be closed,
                // if this UI Element is found in the stack
                if ( uiViewVoStack.Contains (uiViewVo) ) 
                {
                    // create a temporary stack
                    Stack<IUiViewVO> tempStack = new Stack<IUiViewVO>();
                    // pop all elements from stack till we find given element
                    while ( uiViewVoStack.Count > 0 ) 
                    {
                        // pop all elements in the stack
                        IUiViewVO viewVo = uiViewVoStack.Pop();
                        // now check for specific view Id
                        if ( viewVo.ID != uiViewVo.ID ) 
                        {
                            // if not found put it in the temp stack
                            tempStack.Push (viewVo);
                        }
                        else 
                        {
                            // we found what we needed, now exit from this loop
                            break;
                        }
                    }
                    // now copy back everything from temp stack to original stack
                    while ( tempStack.Count > 0 ) 
                    {
                        uiViewVoStack.Push ( tempStack.Pop() );
                    }

                    // flag to indicate that UI element has been closed
                    isUiElementClosed = true;                    
                }
                else 
                {
                    log ( $"Unable to find '{uiElementName}' with id - '{uiViewVo.ID}'" );
                }
            }

            // if we have closed the UI Element, trigger signal
            if (isUiElementClosed) 
            {

               
                // fire the signal notifying that element has been closed
                //signalBus.Fire ( new UiElementClosedSignal (uiViewVo.UiType) );
            }
        }

        /// <summary>
        /// Method to copy the currently active dialog or popup to stack
        /// </summary>
        private void saveCurrentDialogOrPopupToStack() 
        {
            // check that we have a valid popup or dialog visible
            if ( currentDialogOrPopupInFocus != null ) 
            {
                // now get it's view component
                IUiView viewComponent = currentDialogOrPopupInFocus.transform.GetComponent<IUiView>();
                
                log ( $"{viewComponent.UiViewVo.UiType}" );
                
                // make sure that we have a dialog or a popup
                if ( viewComponent != null 
                        && ( viewComponent.UiViewVo.UiType == UiTypesEnum.Dialog 
                                || viewComponent.UiViewVo.UiType == UiTypesEnum.Popup ) ) 
                {
                    // hide / close this UI
                    hideUiElement (viewComponent.UiViewVo, false);
                    // now push VO of this UI on top of stack
                    uiViewVoStack.Push (viewComponent.UiViewVo);
                    // clear reference to this UI
                    currentDialogOrPopupInFocus = null;
                }
            }
        }

        // get the correct layer that this UI element should go into convert this into dictinory like solo did it for the IAP
        private GameObject getUiElementLayer (UiTypesEnum uiType) 
        {
            GameObject layer = null;

            switch (uiType) 
            {
                case UiTypesEnum.Alert:
                        layer = alertsContainerGo;
                        break;
                
                case UiTypesEnum.Dialog:
                        layer = layerDialogsGo;
                        break;
                
                case UiTypesEnum.Hud:
                        layer = layerHudsGo;
                        break;
                
                case UiTypesEnum.Overlay:
                        layer = layerOverlaysGo;
                        break;
                
                case UiTypesEnum.Panel:
                        layer = layerPanelsGo;
                        break;
                
                case UiTypesEnum.Popup:
                        layer = layerPopupsGo;
                        break;
                
                case UiTypesEnum.ContextMenu:
                        layer = layerContextMenusGo;
                        break;
                
                case UiTypesEnum.None:
                default:
                        log ( $"Invalid Ui Type '{uiType.ToString()}'. No Ui Layer supports this Type.", LogType.Error );
                        layer = null;
                        break;
            }

            return layer;
        }

        /// <summary>
        /// Check if given UI element has been loaded or not , if not please load is asynchronously 
        /// </summary>
        /// <param name="viewVo"></param>
        /// <returns>Bool indicating if UI element has been loaded or not</returns>
        private bool isUiElementAlreadyLoaded (IUiViewVO viewVo) 
        {
            // flag indicating if the element is loaded or not
            bool isLoaded = false;

            // if the UI element is found in one of these two maps then it has been loaded
            isLoaded = ( uiElementsUsedMap.ContainsKey (viewVo.UiElementName) 
                            || uiElementsPoolMap.ContainsKey (viewVo.UiElementName) );

            return isLoaded;
        }
        /// <summary>
        /// the get alertview is used to get the view Vo of alerts only , do not use this to call 
        /// other UI Types as these exist independetly
        /// </summary>
        /// <param name="viewVo"></param>
        /// <returns></returns>
        private GameObject getAlertView (IUiViewVO viewVo) 
        {
            GameObject alertView = null;

            if ( viewVo != null ) 
            {
                alertView = usedAlertViews.Find ( (alert) => alert.GetComponent<BaseAlertView>().UiViewVo.ID == viewVo.ID );
            }

            return alertView;
        }

        /// <summary>
        /// Method to show UI Element specified in signal
        /// </summary>
        /// <param name="signal"></param>
        private void onShowUiElement (ShowUiElementSignal signal) 
        {
            if ( signal != null 
                    && signal.ViewVO != null ) 
            {
                Show ( 
                    signal.ViewVO, 
                    signal.IsSaveVisibleUiElementToStack, 
                    signal.OnUiLoadedCallback 
                );
            }
            else 
            {
                log ( "Signal or View VO is NULL." );
            }
        }

        /// <summary>
        /// Method to hide UI Element specified in signal
        /// </summary>
        /// <param name="signal"></param>
        private void onHideUiElement (HideUiElementSignal signal) 
        {
            Hide (signal.ViewVO, signal.OnUiHiddenCallback);
        }



        private void log (string message, LogType type = LogType.Log)
        {
           // LogHelper.Log ( GetType().Name, message, LogHelper.COLOR_LIME, type );
        }

    }

}
