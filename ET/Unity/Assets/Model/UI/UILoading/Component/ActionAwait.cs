using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace ETModel
{
    public static class ActionExtensions
    {
        public static ActionAwait GetAwaiter(this ActionEvent actionEvent)
        {
            return new ActionAwait(actionEvent);
        }
    }
    public class ActionEvent
    {
        private Action action;
        private static readonly List<ActionEvent> mAll = new List<ActionEvent>();
        
        public ActionEvent()
        {
            mAll.Add(this);
        }
        public void Dispatch()
        {
            //if (action == null)
            //{
            //    Debug.Log("ActionEvent Dispatch action == null");
            //}
            action?.Invoke();
        }
        public void Clear()
        {
            action = null;
        }
        public static void ClearAll()
        {
            mAll.ForEach(x=>x.Clear());
            mAll.Clear();
        }
        public void AddListener(Action listener)
        {
            action += listener;
        }
        public void RemoveListener(Action listener)
        {
            action -= listener;
        }
    }
    public class ActionAwait : INotifyCompletion
    {
        private ActionEvent actionEvent;
        private bool isActioned = false;
        private Action continuation;

        public ActionAwait(ActionEvent actionEvent)
        {
            this.actionEvent = actionEvent;
            if (actionEvent == null)
            {
                
                return;
            }
            actionEvent.AddListener(OnAction);
        }
        public bool IsCompleted
        {
            get
            {
                return isActioned;
            }
        }

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            if (actionEvent == null)
            {
                isActioned = true;
                continuation();
            }
        }
        public void GetResult()
        {

        }

        private void OnAction()
        {
            //Debug.Log("OnAction");
            isActioned = true;
            actionEvent.RemoveListener(OnAction);
            continuation();
        }
    }
}