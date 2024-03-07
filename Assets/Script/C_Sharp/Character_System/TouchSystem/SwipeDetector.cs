using UnityEngine;
using UnityEngine.Events;

namespace GDD.TouchSystem
{
    public class SwipeDetector : TouchDetector
    {
        [SerializeField] private UnityEvent<TouchValue> m_swipeEnd = new UnityEvent<TouchValue>();
        [SerializeField]private float _durationSwipe = 0.5f;

        public UnityEvent<TouchValue> swipeEnd
        {
            get => m_swipeEnd;
            set => m_swipeEnd = value;
        }

        public float durationSwipe
        {
            get => _durationSwipe;
            set => _durationSwipe = value;
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void OnTouchEnded(Touch touch)
        {
            // If Finded with fingerID
            if (_touchPool.ContainsKey(touch.fingerId))
            {
                print("Touchhhhhh!!!!!!!!!!!!!!!!!!!!!!!");
                TouchValue touchValue = Swipe(_touchPool[touch.fingerId], touch);
                m_swipeEnd?.Invoke(touchValue);
            }

            base.OnTouchEnded(touch);
        }

        public TouchValue Swipe(TouchIdentifier touchID, Touch touch)
        {
            float duration = Time.time - touchID.timeCreated;
            float angle = 0;
            Debug.Log("Duration : " + duration);

            if (duration < _durationSwipe)
            {
                Vector2 direction = touch.position - touchID.startPosition;
                Debug.Log("Delta : " + direction.normalized.ToString());
                angle = Mathf.Atan2(direction.x, direction.y) * -Mathf.Rad2Deg;
                print($"Angle is : {angle} || {angle * Mathf.Rad2Deg}");
            }

            return new TouchValue(duration, Vector2.Distance(touch.position, touchID.startPosition), angle, touch);
        }

        /*
        public bool Swipe(Touch touch, out float angle, out float distance)
        {
            
        }*/
    }
}