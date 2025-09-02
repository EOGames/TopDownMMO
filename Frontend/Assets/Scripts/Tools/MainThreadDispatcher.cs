using UnityEngine;

    
    using System;
    using System.Collections.Generic;

    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> actions = new Queue<Action>();

        public static MainThreadDispatcher Instance;

        void Awake()
        {

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

       

        public void Enqueue(Action action)
        {
            lock (actions)
            {
                actions.Enqueue(action);
            }
        }

        void Update()
        {
            lock (actions)
            {
                while (actions.Count > 0)
                {
                    actions.Dequeue()?.Invoke();
                }
            }
        }
    }

