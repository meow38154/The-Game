using Agents.Players;
using DG.Tweening;
using UnityEngine;
using Utility;

namespace Agents.NPC
{
    public class Me : Agent, IGetPlayer
    {
        [SerializeField] private Vector3[] pos;
        [SerializeField] private float speed = 3f;
        [SerializeField] private Rigidbody rigid;

        [Header("Player Distance Pause")]
        [SerializeField] private float stopDistance = 12f;
        [SerializeField] private float resumeDistance = 10f;

        private Transform _moveTarget;
        private Tween _moveTween;
        private Tween _rotateTween;
        private bool _isPausedByDistance;

        public Rigidbody Rigid => rigid;
        public GameObject Player { get; set; }

        protected override void Awake()
        {
            base.Awake();

            if (rigid == null)
                rigid = GetComponent<Rigidbody>();

            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void Update()
        {
            CheckPlayerDistancePause();
        }

        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
        }

        public void MoveTarget(Transform target)
        {
            if (target == null)
                return;

            if (Player == null)
            {
                Debug.LogWarning("MoveTarget 실패: Player가 null");
                return;
            }

            if (rigid == null)
            {
                Debug.LogWarning("MoveTarget 실패: Rigidbody가 null");
                return;
            }

            _moveTarget = target;
            _isPausedByDistance = false;

            rigid.DOKill();
            transform.DOKill();

            Vector3 direction = target.position - transform.position;
            float targetY = direction.x >= 0f ? 180f : 0f;

            _rotateTween = rigid.DORotate(
                new Vector3(0f, targetY, 0f),
                0.2f
            ).SetEase(Ease.OutCubic);

            _moveTween = rigid.DOMove(target.position, speed)
                .SetSpeedBased()
                .SetEase(Ease.Linear)
                .SetUpdate(UpdateType.Fixed)
                .OnComplete(() =>
                {
                    _moveTarget = null;
                    _moveTween = null;
                    _rotateTween = null;
                    _isPausedByDistance = false;
                });

            CheckPlayerDistancePause();
        }

        private void CheckPlayerDistancePause()
        {
            if (_moveTarget == null || Player == null || _moveTween == null)
                return;

            float distance = Vector3.Distance(
                transform.position,
                Player.transform.position
            );

            if (!_isPausedByDistance && distance >= stopDistance)
            {
                _isPausedByDistance = true;

                _moveTween.Pause();
                _rotateTween?.Pause();
            }
            else if (_isPausedByDistance && distance <= resumeDistance)
            {
                _isPausedByDistance = false;

                _moveTween.Play();
                _rotateTween?.Play();
            }
        }

        public void Teleport(int index)
        {
            if (index < 0 || index >= pos.Length)
                return;

            rigid.DOKill();
            transform.DOKill();

            _moveTween = null;
            _rotateTween = null;
            _moveTarget = null;
            _isPausedByDistance = false;

            Vector3 teleportPosition = pos[index];

            rigid.position = teleportPosition;
            transform.position = teleportPosition;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);

            rigid?.DOKill();
            transform.DOKill();
        }
    }
}