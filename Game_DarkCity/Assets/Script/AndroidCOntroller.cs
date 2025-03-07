using Spine.Unity.Examples;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
    public SpineboyBeginnerModel characterModel; // Tham chiếu đến model nhân vật

    public Button leftButton;
    public Button rightButton;
    public Button jumpButton;

    private bool isMovingLeft = false; // Trạng thái giữ nút trái
    private bool isMovingRight = false; // Trạng thái giữ nút phải

    private void Start()
    {
        // Gán sự kiện giữ cho các nút
        AddHoldEvent(leftButton, () => isMovingLeft = true, () => isMovingLeft = false);
        AddHoldEvent(rightButton, () => isMovingRight = true, () => isMovingRight = false);

        // Nút nhảy vẫn sử dụng onClick do không cần giữ
        jumpButton.onClick.AddListener(() => Jump());
    }

    private void Update()
    {
        // Kiểm tra trạng thái và di chuyển liên tục
        if (isMovingLeft)
        {
            characterModel.currentSpeed = -1f; // Cập nhật giá trị speed khi di chuyển trái
            MoveLeft();
        }
        else if (isMovingRight)
        {
            characterModel.currentSpeed = 1f; // Cập nhật giá trị speed khi di chuyển phải
            MoveRight();
        }
        else
        {
            characterModel.currentSpeed = 0f; // Nếu không giữ nút nào thì tốc độ = 0
        }

        // Debugging to check currentSpeed
        Debug.Log($"Current Speed: {characterModel.currentSpeed}");
    }

    private void MoveLeft()
    {
        characterModel.TryMove(-1f); // Di chuyển sang trái
    }

    private void MoveRight()
    {
        characterModel.TryMove(1f); // Di chuyển sang phải
    }

    private void Jump()
    {
        characterModel.TryJump(); // Thực hiện nhảy
    }

    // Hàm tiện ích để thêm sự kiện giữ và thả cho nút
    private void AddHoldEvent(Button button, System.Action onHoldStart, System.Action onHoldEnd)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // Thêm sự kiện khi giữ nút
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((data) => { onHoldStart?.Invoke(); });
        trigger.triggers.Add(pointerDownEntry);

        // Thêm sự kiện khi thả nút
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((data) => { onHoldEnd?.Invoke(); });
        trigger.triggers.Add(pointerUpEntry);
    }
}
