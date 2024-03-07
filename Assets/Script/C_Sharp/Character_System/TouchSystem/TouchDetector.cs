using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GDD.TouchSystem
{
    public class TouchDetector : MonoBehaviour
    {
        [SerializeField] protected UnityEvent m_startAction;
        [SerializeField] protected UnityEvent m_stopAction;
        protected TouchIdentifier m_touchIdentifier;
        protected Dictionary<int, TouchIdentifier> _touchPool = new Dictionary<int, TouchIdentifier>();
        protected int _lastIndex = 0;
        public Camera _Camera;

        public UnityEvent startAction
        {
            get => m_startAction;
            set => m_stopAction = value;
        }

        public UnityEvent stopAction
        {
            get => m_stopAction;
            set => m_stopAction = value;
        }

        public TouchIdentifier touchIdentifier
        {
            get => m_touchIdentifier;
        }

        private void OnGamestateChanged(GameState gameState)
        {
            enabled = gameState == GameState.Play;
        }

        private void Awake()
        {
            Game_State_Manager.Instance.OnGameStateChange += OnGamestateChanged;
        }

        protected virtual void Start()
        {
            if(_Camera == null)
                _Camera = this.GetComponent<Camera>();
        }

        protected virtual void Update()
        {
            for (int i = 0; i < Input.touches.Length; i++)
            {
                Touch t = Input.GetTouch(i);
                
                if (CheckTouchOtherSystem(t) || PointerOverUIElement.OnPointerOverUIElement(t))
                {
                    continue;
                }

                switch (t.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBegan(t);
                        break;
                    case TouchPhase.Ended:
                        OnTouchEnded(t);
                        break;
                    case TouchPhase.Moved:
                        OnTouchMoved(t);
                        break;
                    case TouchPhase.Stationary:
                        OnTouchStay(t);
                        break;
                    case TouchPhase.Canceled:
                        OnTouchCancel(t);
                        break;
                }
            }
        }

        public TouchIdentifier GetTouchIdentifierWithTouch(Touch touch)
        {
            //If Finded with fingerID
            if (_touchPool.ContainsKey(touch.fingerId))
            {
                _touchPool[touch.fingerId].deltaPosition = touch.deltaPosition;
                
                return _touchPool[touch.fingerId];
            }

            //Get a new Touch.
            TouchIdentifier t = new TouchIdentifier(touch.fingerId, Time.time, touch.position);
            t.deltaPosition = touch.deltaPosition;
            _lastIndex++;
            _touchPool.Add(touch.fingerId, t);
            return t;
        }

        public void RemoveTouchIdentifierWithTouch(Touch touch)
        {
            RemoveTouchIdentifierWithTouch(touch, _touchPool);
        }

        public bool RemoveTouchIdentifierWithTouch(Touch touch, Dictionary<int, TouchIdentifier> listTouch)
        {
            if (_touchPool.ContainsKey(touch.fingerId))
            {
                return listTouch.Remove(touch.fingerId);
            }

            return false;
        }

        public virtual void OnTouchBegan(Touch touch)
        {
            GetTouchIdentifierWithTouch(touch);
            TouchValue touchValue = new TouchValue(0, 0, 0, new Touch());
            m_startAction?.Invoke();
        }

        public virtual void OnTouchEnded(Touch touch)
        {
            RemoveTouchIdentifierWithTouch(touch);
            TouchValue touchValue = new TouchValue(0, 0, 0, new Touch());
            m_stopAction?.Invoke();
        }

        public virtual void OnTouchMoved(Touch touch)
        {
            _touchPool[touch.fingerId].deltaPosition = touch.deltaPosition;
        }

        public virtual void OnTouchStay(Touch touch)
        {
            _touchPool[touch.fingerId].deltaPosition = touch.deltaPosition;
        }

        public virtual void OnTouchCancel(Touch touch)
        {
            RemoveTouchIdentifierWithTouch(touch);
        }
        
        public bool CheckTouchOtherSystem(Touch _touch)
        {
            bool isTouch = false;

            foreach (var touch in Joystick._touches)
            {
                if(touch.fingerId == _touch.fingerId)
                {
                    isTouch = true;
                    break;
                }
            }

            return isTouch;
        }
        
        void OnDestroy()
        {
            Game_State_Manager.Instance.OnGameStateChange -= OnGamestateChanged;
        }
    }
}