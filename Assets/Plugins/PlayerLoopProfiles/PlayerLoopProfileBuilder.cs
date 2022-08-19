using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using static PlayerLoopProfiles.PlayerLoopProfile;

namespace PlayerLoopProfiles
{
    /// <summary>
    /// Builder class to make instantiating of PlayerLoopProfile's easier and more convinient 
    /// </summary>
    public class PlayerLoopProfileBuilder 
    {
        public static List<Type> InputSystems = new Type[]
        {
            typeof(Initialization.SynchronizeInputs),
            typeof(EarlyUpdate.UpdateInputManager),
            typeof(EarlyUpdate.ProcessRemoteInput),
            typeof(FixedUpdate.NewInputFixedUpdate),
            typeof(PreUpdate.CheckTexFieldInput),
            typeof(PreUpdate.NewInputUpdate),
            typeof(PostLateUpdate.InputEndFrame),
            typeof(PostLateUpdate.ResetInputAxis),
        }.ToList();

        private List<Type> filteredSystems = new List<Type>();
        private FilterType filteredType = PlayerLoopProfile.FilterType.REMOVE;
        private List<PlayerLoopSystem> additionalSystems = new List<PlayerLoopSystem>();
        private Action<string> interactionAction = null;
        private List<string> ignoredInteraction = new List<string>();
        private Action timeoutAction = null;
        private float timeoutLength = 1;
        private Dictionary<Type, ActiveUiEvaluation> UITest = new Dictionary<Type, ActiveUiEvaluation>();
        private Dictionary<Type, ActiveUiToolkitEvaluation> UiToolkitTest = new Dictionary<Type, ActiveUiToolkitEvaluation>();
        private PlayerLoopSystem baseSystem = new PlayerLoopSystem();
        private bool keepInteractionSystems = true;


        /// <summary>
        /// The elements of this collection should correspond to the types returned PlayerLoopSystem.type and can be on any layer of the PlayerLoopSystem. 
        /// For example one entry could be 'PostLateUpdate', a system with children, or 'PostLateUpdate.PresentAfterDraw', a subsystem without any additional children.
        /// By default this is an empty collection. 
        /// </summary>
        public PlayerLoopProfileBuilder FilterSystems(params Type[] pSystems)
        {
            filteredSystems = pSystems.ToList();
            return this;
        }

        /// <summary>
        /// The elements of this collection should correspond to the types returned PlayerLoopSystem.type and can be on any layer of the PlayerLoopSystem. 
        /// For example one entry could be 'PostLateUpdate', a system with children, or 'PostLateUpdate.PresentAfterDraw', a subsystem without any additional children.
        /// By default this is an empty collection. 
        /// </summary>
        public PlayerLoopProfileBuilder FilterSystems(List<Type> pSystems)
        {
            filteredSystems = pSystems;
            return this;
        }

        /// <summary>
        /// Once the final system is queried, a loop is iterating through all sub systems inside the 'BaseSystem'. 
        /// All subsystems get compared to the elements of the profiles 'FilterSystems'.
        /// On a match the 'FilterType' determines if the affected system gets removed or kept.
        /// By default the 'FilterType' is set to FilterType.REMOVE, so without specifing a 'FilterType' nor 'FilterSystems' no subsystem gets removed (in other words all subsytems are kept).
        /// </summary>
        public PlayerLoopProfileBuilder FilterType(FilterType pType)
        {
            filteredType = pType;
            return this;
        }

        /// <summary>
        /// Used to add custom PlayerLoopSystems, or to ensure the passed system is present after filtering.
        /// Systems of the 'AdditionalSystems' variable get added to the resulting system after filtering, meaning they wont be filtered in any way.
        /// </summary>
        public PlayerLoopProfileBuilder AdditionalSystems(params PlayerLoopSystem[] pSystems)
        {
            additionalSystems = pSystems.ToList();
            return this;
        }

        /// <summary>
        /// Used to add custom PlayerLoopSystems, or to ensure the passed system is present after filtering.
        /// Systems of the 'AdditionalSystems' variable get added to the resulting system after filtering, meaning they wont be filtered in any way.
        /// </summary>
        public PlayerLoopProfileBuilder AdditionalSystems(List<PlayerLoopSystem> pSystems)
        {
            additionalSystems = pSystems;
            return this;
        }

        /// <summary>
        /// When this profile is active any UI interaction will invoke the actions of the 'InteractionCallback' variable.
        /// IMPORTANT: If you decide to use this method of getting the user interaction ensure 'KeepInteractionSystems' is set to true or ensure
        /// this PlayerLoopProfile does not remove input related systems, as removing them results in no interaction being recognized
        /// </summary>
        public PlayerLoopProfileBuilder InteractionCallback(Action<string> pAction)
        {
            interactionAction = pAction;
            return this;
        }

        /// <summary>
        /// Any interaction in this collection will not reset the interaction timeout, but the 'InteractionCallback' is still invoked.
        /// meaning if the user only moves the mouse the 'TimeoutCallback' will still be called after 'TimeoutDuration' seconds.
        /// By default no interactions are ignored.
        /// In the example InteractionType.POINT is in this collection mouse movement will not reset the timeout timer,
        /// </summary>
        public PlayerLoopProfileBuilder IgnoredInteractions(params string[] pInteraction)
        {
            ignoredInteraction = pInteraction.ToList();
            return this;
        }

        /// <summary>
        /// Any interaction in this collection will not reset the interaction timeout, but the 'InteractionCallback' is still invoked.
        /// meaning if the user only moves the mouse the 'TimeoutCallback' will still be called after 'TimeoutDuration' seconds.
        /// By default no interactions are ignored.
        /// In the example InteractionType.POINT is in this collection mouse movement will not reset the timeout timer,
        /// </summary>
        public PlayerLoopProfileBuilder IgnoredInteractions(List<string> pInteraction)
        {
            ignoredInteraction = pInteraction;
            return this;
        }

        /// <summary>
        /// After the amount of seconds specified by 'TimeoutDuration' passes with no user interaction this action is invoked.
        /// This timeout is reset on every user interaction which is not included in the 'IgnoredInteractions' variable
        /// </summary>
        public PlayerLoopProfileBuilder TimeoutCallback(Action pAction)
        {
            timeoutAction = pAction;
            return this;
        }

        /// <summary>
        /// Specifies the time in seconds it takes for 'TimeoutCallback' to be called after the last user interaction.
        /// </summary>
        public PlayerLoopProfileBuilder TimeoutDuration(float pLength)
        {
            timeoutLength = pLength;
            return this;
        }

        /// <summary>
        /// Defines sepcial behaviours for active Ui elements. 
        /// Before the 'TimeoutCallback' is invoked the system gets the active object via the EventSystem,
        /// checks if the type matches the one passed into this method, if so the passed ActiveUiEvaluation delegate is called with the selected object.
        /// If the delegate returns true the timeout gets reset, otherwise 'TimeoutCallback' will be invoked.
        /// </summary>
        public PlayerLoopProfileBuilder ActiveUiEvaluation(Type pType, ActiveUiEvaluation pSystems)
        {
            UITest.TryAdd(pType, pSystems);
            return this;
        }

        /// <summary>
        /// Defines sepcial behaviours for active Ui elements. 
        /// Before the 'TimeoutCallback' is invoked the system gets the active object via the FocusController,
        /// checks if the type matches the one passed into this method, if so the passed ActiveUiToolkitEvaluation delegate is called with the selected object.
        /// If the delegate returns true the timeout gets reset, otherwise 'TimeoutCallback' will be invoked.
        /// </summary>
        public PlayerLoopProfileBuilder ActiveUiEvaluation(Type pType, ActiveUiToolkitEvaluation pSystems)
        {
            UiToolkitTest.TryAdd(pType, pSystems);
            return this;
        }

        /// <summary>
        /// Used to set a custom base PlayerLoopSystem. By default the default unity PlayerLoopSystem 'PlayerLoop.GetDefaultPlayerLoop()' is used.
        /// This is the base/foundation on which the other PlayerLoopSystem modifying variables act upon.
        /// </summary>
        public PlayerLoopProfileBuilder BaseSystem(PlayerLoopSystem pBaseSystem)
        {
            baseSystem = pBaseSystem;
            return this;
        }

        /// <summary>
        /// A convenience variable. When set tu true the profile keeps various input related systems to still receive user inputs no matter the given filters
        /// By default 'KeepInteractionSystems' is set to true to prevent accidental removal
        /// </summary>
        public PlayerLoopProfileBuilder KeepInteractionSystems(bool pKeepInteraction)
        {
            keepInteractionSystems = pKeepInteraction;
            return this;
        }

        /// <summary>
        /// Build into a proper PlayerLoopProfile object
        /// </summary>
        public PlayerLoopProfile Build()
        {
            if (baseSystem.subSystemList == null || baseSystem.subSystemList.Length == 0)
            {
                baseSystem = PlayerLoop.GetDefaultPlayerLoop();
            }

            return new PlayerLoopProfile(
                filteredSystems, 
                filteredType, 
                additionalSystems, 
                baseSystem, 
                interactionAction, 
                ignoredInteraction,
                timeoutAction, 
                timeoutLength,
                UITest,
                UiToolkitTest,
                keepInteractionSystems
            );
        }
    }
}