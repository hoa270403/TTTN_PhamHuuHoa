using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using System.Collections;  // Để sử dụng coroutine

public class CanvasController : MonoBehaviour
{
    public GameObject canvas; // GameObject chứa Canvas
    public SkeletonGraphic skeletonGraphic; // SkeletonGraphic sẽ hiển thị trên Canvas
    public string animationName = "Show"; // Animation muốn hiển thị
    public Button showButton; // Button để hiển thị Canvas
    private CanvasGroup buttonCanvasGroup; // CanvasGroup của Button
    private bool isButtonClicked = false;
    private void Start()
    {
        // Đảm bảo Canvas bắt đầu ẩn
        if (canvas != null)
        {
            canvas.SetActive(false);
        }

        // Kiểm tra và lấy CanvasGroup của Button
        if (showButton != null)
        {
            buttonCanvasGroup = showButton.GetComponent<CanvasGroup>();

            if (buttonCanvasGroup == null)
            {
                // Nếu chưa có CanvasGroup thì thêm nó vào Button
                buttonCanvasGroup = showButton.gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Đảm bảo button có thể click ngay từ đầu
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
            buttonCanvasGroup.alpha = 1f; // Đảm bảo Button sáng lên
        }

        // Đăng ký sự kiện khi Button được nhấn
        if (showButton != null)
        {
            showButton.onClick.AddListener(OnButtonClicked);
        }
    }
    private void Update()
    {
        // Kiểm tra phím 'P' có được nhấn không
        if (Input.GetKeyDown(KeyCode.P) && !isButtonClicked)
        {
            OnButtonClicked();
        }
    }
    // Hàm được gọi khi Button được nhấn
    private void OnButtonClicked()
    {
        // Hiển thị Canvas và chơi animation
        if (canvas != null)
        {
            canvas.SetActive(true);
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, false); // Play animation once
        }

        // Tạm thời vô hiệu hóa Button và cho nó mờ đi
        StartCoroutine(DisableButtonTemporarily());

        // Đăng ký sự kiện khi animation hoàn tất
        skeletonGraphic.AnimationState.Complete += OnAnimationComplete;
    }

    // Coroutine vô hiệu hóa Button trong 5 giây
    private IEnumerator DisableButtonTemporarily()
    {
        // Tạm thời vô hiệu hóa Button (mờ và không thể click)
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.interactable = false;
            buttonCanvasGroup.blocksRaycasts = false;
            buttonCanvasGroup.alpha = 0.5f; // Làm Button mờ đi
        }

        // Đợi 5 giây
        yield return new WaitForSeconds(5f);

        // Kích hoạt lại Button sau 5 giây
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
            buttonCanvasGroup.alpha = 1f; // Làm Button sáng lên
        }
    }

    // Hàm được gọi khi animation hoàn tất
    private void OnAnimationComplete(Spine.TrackEntry trackEntry)
    {
        // Kiểm tra xem animation hiện tại có phải là animation "Show" không
        if (trackEntry.Animation.Name == animationName)
        {
            // Ẩn Canvas sau khi animation hoàn tất
            canvas.SetActive(false);
            // Hủy bỏ đăng ký sự kiện Complete để tránh gọi lại sau này
            skeletonGraphic.AnimationState.Complete -= OnAnimationComplete;
        }
    }
}
