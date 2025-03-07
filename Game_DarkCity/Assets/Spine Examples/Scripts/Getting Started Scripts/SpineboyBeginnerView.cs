using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Spine.Unity.Examples
{
    public class SpineboyBeginnerView : MonoBehaviour
    {

        #region Inspector
        [Header("Components")]
        public SpineboyBeginnerModel model;
        public SkeletonAnimation skeletonAnimation;

        public AnimationReferenceAsset run, idle, aim, shoot, jump, death,power,hit; // Thêm animation chết.
        public EventDataReferenceAsset footstepEvent;

        [Header("Audio")]
        public float footstepPitchOffset = 0.2f;
        public float gunsoundPitchOffset = 0.13f;
        public AudioSource footstepSource, gunSource, jumpSource,hitSource,deadSource;

        [Header("Effects")]
        public ParticleSystem gunParticles;
        #endregion
        [Header("Power Effect")]
        public ParticleSystem powerParticles; // Hiệu ứng hạt khi sử dụng chiêu thức
        public AudioSource powerSound;        // Âm thanh khi sử dụng chiêu thức

        SpineBeginnerBodyState previousViewState;

        public float powerCooldown = 5f; // Thời gian hồi chiêu
        private float powerCooldownTime = 0f; // Thời gian còn lại cho cooldown

        public Text cooldownText; // UI Text để hiển thị đếm ngược

        void Start()
        {
            if (skeletonAnimation == null) return;
            model.ShootEvent += PlayShoot;
            model.StartAimEvent += StartPlayingAim;
            model.StopAimEvent += StopPlayingAim;
            model.DeathEvent += PlayDeath; // Lắng nghe sự kiện chết từ model.
            model.PowerEvent += PlayPower;
            model.HitEvent += PlayHit;

            skeletonAnimation.AnimationState.Event += HandleEvent;
            if (model != null)
            {
                model.DeathEvent += PlayDeath; // Lắng nghe sự kiện chết từ Model
            }

        }

        void HandleEvent(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data == footstepEvent.EventData)
                PlayFootstepSound();
        }

        void Update()
        {
            // Kiểm tra nếu chuột đang trỏ vào UI
            // Không thực hiện bất kỳ hành động nào nếu đang trong trạng thái Power
            if (model.state == SpineBeginnerBodyState.Power)
                return;

            // Nếu chuột đang trên UI, không xử lý bắn
            

            if ((skeletonAnimation.skeleton.ScaleX < 0) != model.facingLeft)
            {    // Detect changes in model.facingLeft
                Turn(model.facingLeft);
            }

            // Detect changes in model.state
            var currentModelState = model.state;

            if (previousViewState != currentModelState)
            {
                PlayNewStableAnimation();
            }
            if (Input.GetKeyDown(KeyCode.P)) // Nhấn phím 'P' để kích hoạt Power
            {
                if (model != null) // Kiểm tra nếu model được gắn
                {
                    model.TryPower();
                }
            }
            //
            // Cập nhật thời gian hồi chiêu
            if (powerCooldownTime > 0)
            {
                powerCooldownTime -= Time.deltaTime; // Giảm thời gian còn lại
                if (cooldownText != null)
                {
                    cooldownText.text = Mathf.Ceil(powerCooldownTime).ToString(); // Cập nhật đếm ngược
                }
            }
            else
            {
                if (cooldownText != null)
                {
                    cooldownText.text = "Ready"; // Hiển thị khi chiêu đã sẵn sàng
                }
            }
            previousViewState = currentModelState;
        }

        void PlayNewStableAnimation()
        {
            var newModelState = model.state;
            Animation nextAnimation;

            if (newModelState == SpineBeginnerBodyState.Dead)
            { // Chơi animation chết
                nextAnimation = death;
            }
            else if (newModelState == SpineBeginnerBodyState.Jumping)
            {
                jumpSource.Play();
                nextAnimation = jump;
            }
            else if (newModelState == SpineBeginnerBodyState.Hit)
            {
                nextAnimation = hit;
                //skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, false);

                
            }
            else if (newModelState == SpineBeginnerBodyState.Power)
            {
                nextAnimation = power; // Animation cho chiêu thức
                StartCoroutine(EndPowerAnimation()); // Kết thúc chiêu thức sau một khoảng thời gian
            }
            else
            {
                if (newModelState == SpineBeginnerBodyState.Running)
                {
                    nextAnimation = run;
                }
                else
                {
                    nextAnimation = idle;
                }
            }
            
            ////
           
            
            skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
        }
        
        void PlayFootstepSound()
        {
            footstepSource.Play();
            footstepSource.pitch = GetRandomPitch(footstepPitchOffset);
        }

        [ContextMenu("Check Tracks")]
        void CheckTracks()
        {
            var state = skeletonAnimation.AnimationState;
            Debug.Log(state.GetCurrent(0));
            Debug.Log(state.GetCurrent(1));
        }

        #region Transient Actions
        public void PlayShoot()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                
                return;
            }

            var shootTrack = skeletonAnimation.AnimationState.SetAnimation(1, shoot, false);
            shootTrack.AttachmentThreshold = 1f;
            shootTrack.MixDuration = 0f;
            var empty1 = skeletonAnimation.state.AddEmptyAnimation(1, 0.5f, 0.1f);
            empty1.AttachmentThreshold = 1f;

            var aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, false);
            aimTrack.AttachmentThreshold = 1f;
            aimTrack.MixDuration = 0f;
            var empty2 = skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
            empty2.AttachmentThreshold = 1f;

            gunSource.pitch = GetRandomPitch(gunsoundPitchOffset);
            gunSource.Play();
            gunParticles.Play();
        }

        public void StartPlayingAim()
        {
            var aimTrack = skeletonAnimation.AnimationState.SetAnimation(2, aim, true);
            aimTrack.AttachmentThreshold = 1f;
            aimTrack.MixDuration = 0f;
        }

        public void StopPlayingAim()
        {
            var empty2 = skeletonAnimation.state.AddEmptyAnimation(2, 0.5f, 0.1f);
            empty2.AttachmentThreshold = 1f;
        }

        public void PlayDeath()
        {
            if (skeletonAnimation == null || death == null) return;

            // Phát animation chết
            skeletonAnimation.AnimationState.SetAnimation(0, death, false);
            deadSource.Play();
            // Tạm thời dùng Coroutine để kiểm tra
            StartCoroutine(ReloadSceneAfterDelay(2f));
        }
        public void PlayHit()
        {
            if (skeletonAnimation == null || hit == null) return;
            model.state = SpineBeginnerBodyState.Hit;
            hitSource.Play();
            skeletonAnimation.AnimationState.SetAnimation(0, hit, false);
            StartCoroutine(EndPowerAnimation());

        }
        public void PlayPower()
        {
            if (skeletonAnimation == null || power == null) return;
            // Đặt trạng thái thành Power
            model.state = SpineBeginnerBodyState.Power;
            skeletonAnimation.AnimationState.SetAnimation(0, power, false);

            // Thêm hiệu ứng âm thanh hoặc hạt
            powerParticles.Play(); // Gắn ParticleSystem vào biến powerParticles
            powerSound.Play(); // Gắn AudioSource vào biến powerSound
                               // Kết thúc Power sau thời gian nhất định
            StartCoroutine(EndPowerAnimation());
        }
        void StartPowerCooldown()
        {
            powerCooldownTime = powerCooldown; // Đặt lại thời gian hồi chiêu
        }

        private IEnumerator ReloadSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //anim power 
        private IEnumerator EndPowerAnimation()
        {
            yield return new WaitForSeconds(2f); // Đợi animation kết thúc (thời gian tuỳ thuộc vào độ dài animation)
            model.state = SpineBeginnerBodyState.Idle; // Trở về trạng thái Idle
                                                       // Gọi hàm để cập nhật lại animation
            PlayNewStableAnimation();
        }



        public void Turn(bool facingLeft)
        {
            skeletonAnimation.Skeleton.ScaleX = facingLeft ? -1f : 1f;
        }
        #endregion

        #region Utility
        public float GetRandomPitch(float maxPitchOffset)
        {
            return 1f + Random.Range(-maxPitchOffset, maxPitchOffset);
        }
        #endregion
    }
}
