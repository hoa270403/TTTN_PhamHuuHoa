using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Thêm không gian tên cho sự kiện

namespace Spine.Unity.Examples
{
    public class CharacterControlUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public SpineboyBeginnerModel character; // Tham chiếu đến SpineboyBeginnerModel để điều khiển nhân vật
        public Button leftButton; // Nút di chuyển trái
        public Button rightButton; // Nút di chuyển phải
        public Button jumpButton; // Nút nhảy

        private bool isMovingLeft = false;
        private bool isMovingRight = false;
        void OnValidate()
        {
            if (character == null)
                character = GetComponent<SpineboyBeginnerModel>();
        }
        // Khi nhấn nút trái
        public void OnLeftButtonDown()
        {
            isMovingLeft = true;
            character.TryMove(-1); // Di chuyển sang trái
        }

        // Khi thả nút trái
        public void OnLeftButtonHoldUp()
        {
            isMovingLeft = false;
            character.TryMove(0); // Dừng di chuyển
        }

        // Khi nhấn nút phải
        public void OnRightButtonDown()
        {
            isMovingRight = true;
            character.TryMove(1); // Di chuyển sang phải
        }

        // Khi thả nút phải
        public void OnRightButtonHoldUp()
        {
            isMovingRight = false;
            character.TryMove(0); // Dừng di chuyển
        }

        // Khi nhấn nút nhảy
        public void OnJumpButtonDown()
        {
            character.TryJump(); // Nhảy
        }

        // Gọi khi người chơi nhấn nút
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == leftButton.gameObject)
            {
                OnLeftButtonDown();
            }
            else if (eventData.pointerCurrentRaycast.gameObject == rightButton.gameObject)
            {
                OnRightButtonDown();
            }
            else if (eventData.pointerCurrentRaycast.gameObject == jumpButton.gameObject)
            {
                OnJumpButtonDown();
            }
        }

        // Gọi khi người chơi thả nút
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == leftButton.gameObject)
            {
                OnLeftButtonHoldUp();
            }
            else if (eventData.pointerCurrentRaycast.gameObject == rightButton.gameObject)
            {
                OnRightButtonHoldUp();
            }
        }

        // Update mỗi frame để kiểm tra nếu người chơi vẫn giữ nút
        void Update()
        {
            //if (Time.timeScale == 0) return;
            if (isMovingLeft)
            {
                character.TryMove(-1); // Tiếp tục di chuyển sang trái
            }
            else if (isMovingRight)
            {
                character.TryMove(1); // Tiếp tục di chuyển sang phải
            }
        }
    }
}
