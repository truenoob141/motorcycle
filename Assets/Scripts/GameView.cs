using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Motorcycle
{
    public class GameView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private VehicleBehaviour vehicle;
        [SerializeField] private TMP_Text wheelieLabel;
        [SerializeField] private TMP_Text distanceLabel;

        private bool isContact;
        private float contactTime;

        private bool pointer;
        private int pointerId;

        private void Update()
        {
            if (vehicle == null)
            {
                distanceLabel.text = "N/A";
                wheelieLabel.text = string.Empty;
                return;
            }

            // Distance
            distanceLabel.text = vehicle.Distance.ToString("0.");
            
            // Wheelie
            bool isLeftContactRightNot = vehicle.IsLeftContactRightNot();
            if (!isLeftContactRightNot)
            {
                isContact = false;
                wheelieLabel.text = string.Empty;
            }
            else if (isContact)
            {
                wheelieLabel.text = Time.time - contactTime > 0.1f ? "Wheelie!" : string.Empty;
            }
            else
            {
                isContact = true;
                contactTime = Time.time;
                wheelieLabel.text = string.Empty;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (pointer || vehicle == null)
                return;

            pointer = true;
            pointerId = eventData.pointerId;

            bool isRight = eventData.position.x > Screen.width * 0.5f;
            vehicle.EnableMove(isRight);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!pointer || pointerId != eventData.pointerId || vehicle == null)
                return;
            
            pointer = false;
            vehicle.DisableMove();
        }
    }
}