using System.Collections;
using Agents.Players;
using Sound;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Boss
{
    public class MegaPig : MonoBehaviour, IGetPlayer
    {
        public GameObject Player { get; set; }

        [Header("Attack Prefabs")]
        [SerializeField] private GameObject removeAttack;
        [SerializeField] private GameObject errorArrow;
        [SerializeField] private GameObject errorArrowNotice;

        [Header("Scene Object")]
        [SerializeField] private GameObject laser;

        [Header("Fire Points")]
        [SerializeField] private Transform[] errorArrowFirePoint;

        [Header("Error Arrow")]
        [SerializeField] private float arrowNoticeTime = 1.2f;
        [SerializeField] private float arrowSpreadAngle = 30f;
        [SerializeField] private int arrowBurstCount = 10;
        [SerializeField] private float arrowBurstDelay = 0.2f;

        [Header("Remove Attack")]
        [SerializeField] private int removeAttackCount = 4;
        [SerializeField] private float removeAttackDelay = 0.3f;

        [Header("Laser")]
        [SerializeField] private float laserAimTime = 2f;
        [SerializeField] private float laserRotateSpeed = 120f;
        [SerializeField] private float laserKeepTime = 1f;
        [SerializeField] private Vector3 laserRotationOffset;

        [Header("Pattern Interval")]
        [SerializeField] private float nextPatternDelay = 0.2f;
        
        [Header("Sound")]
        [SerializeField] private SoundClipSO ah;

        public bool FightStart { get; set; }

        private bool _skill;
        private bool _isAnyNoticePlaying;

//        private Collider _laserHit;
        private ParticleSystem[] _laserParticles;

        private void Awake()
        {
            EventBus.Subscribe<PlayerGameObjectMessage>(HandleGetPlayer);

            if (laser == null) return;

 //           _laserHit = laser.GetComponentInChildren<Collider>(true);
            _laserParticles = laser.GetComponentsInChildren<ParticleSystem>(true);

     //       if (_laserHit != null)
      //          _laserHit.enabled = false;

            laser.SetActive(false);
        }

        public void HandleGetPlayer(PlayerGameObjectMessage message)
        {
            Player = message.player;
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<PlayerGameObjectMessage>(HandleGetPlayer);
        }

        private void Update()
        {
            if (!FightStart) return;
            if (_skill) return;
            if (Player == null) return;
            if (errorArrowFirePoint == null || errorArrowFirePoint.Length == 0) return;

            StartRandomPattern();
        }

        private void StartRandomPattern()
        {
            _skill = true;

            int attackIndex = Random.Range(0, 3);

            if (attackIndex == 0)
            {
                StartCoroutine(RemoveSkillStart());
            }
            else if (attackIndex == 1)
            {
                StartCoroutine(ErrorArrowFire());
            }
            else
            {
                StartCoroutine(LaserSkill());
            }
        }

        private IEnumerator RemoveSkillStart()
        {
            for (int i = 0; i < removeAttackCount; i++)
            {
                if (Player == null) break;

                GameObject ra = Instantiate(removeAttack);
                ra.transform.position = Player.transform.position;

                yield return new WaitForSeconds(removeAttackDelay);
            }

            yield return new WaitForSeconds(nextPatternDelay);

            _skill = false;
        }

        private IEnumerator ErrorArrowFire()
        {
            Transform firePoint = GetRandomFirePoint();

            if (firePoint == null)
            {
                _skill = false;
                yield break;
            }

            yield return PlayNoticeIfPossible(firePoint);
            

            for (int i = 0; i < arrowBurstCount; i++)
            {
                if (Player == null) break;

                Vector3 dir = Player.transform.position - firePoint.position;

                if (dir.sqrMagnitude > 0.001f)
                {
                    Quaternion baseRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

                    for (int j = -1; j < 2; j++)
                    {
                        GameObject ra = Instantiate(errorArrow);
                        ra.transform.position = firePoint.position;

                        ra.transform.rotation =
                            Quaternion.AngleAxis(j * arrowSpreadAngle, Vector3.up) * baseRot;
                    }
                }
                EventBus.Publish(SoundEvents.PlaySoundEvent.Init(transform.position, ah));

                yield return new WaitForSeconds(arrowBurstDelay);
            }
            _skill = false;
        }

        private IEnumerator LaserSkill()
        {
            Transform firePoint = GetRandomFirePoint();

            if (firePoint == null || laser == null)
            {
                _skill = false;
                yield break;
            }

            yield return PlayNoticeIfPossible(firePoint);

            laser.transform.position = firePoint.position;
            laser.transform.rotation = Quaternion.Euler(0, 0, 0);

//            if (_laserHit != null)
//                _laserHit.enabled = false;

            laser.SetActive(true);
            PlayLaserParticles();

            float timer = 0f;

            while (timer < laserAimTime)
            {
                LaserRotateMove();

                timer += Time.deltaTime;
                yield return null;
            }

//            if (_laserHit != null)
//                _laserHit.enabled = true;

            yield return new WaitForSeconds(laserKeepTime);
//
//            if (_laserHit != null)
//                _laserHit.enabled = false;

            StopLaserParticles();

            yield return new WaitForSeconds(0.1f);

            laser.SetActive(false);
            _skill = false;
        }

        private IEnumerator PlayNoticeIfPossible(Transform firePoint)
        {
            if (firePoint == null)
            {
                yield return new WaitForSeconds(arrowNoticeTime);
                yield break;
            }

            if (_isAnyNoticePlaying)
            {
                yield return new WaitForSeconds(arrowNoticeTime);
                yield break;
            }

            _isAnyNoticePlaying = true;

            GameObject notice = Instantiate(errorArrowNotice);
            notice.transform.position = firePoint.position;

            yield return new WaitForSeconds(arrowNoticeTime);

            if (notice != null)
                Destroy(notice);

            _isAnyNoticePlaying = false;
        }

        private void LaserRotateMove()
        {
            if (Player == null) return;
            if (laser == null) return;

            Vector3 dir = Player.transform.position - laser.transform.position;

            if (dir.sqrMagnitude <= 0.001f) return;

            Quaternion targetRot =
                Quaternion.LookRotation(dir.normalized, Vector3.up)
                * Quaternion.Euler(laserRotationOffset);

            laser.transform.rotation = Quaternion.RotateTowards(
                laser.transform.rotation,
                targetRot,
                laserRotateSpeed * Time.deltaTime
            );
        }

        private void PlayLaserParticles()
        {
            if (_laserParticles == null) return;

            foreach (ParticleSystem particle in _laserParticles)
            {
                if (particle != null)
                    particle.Play();
            }
        }

        private void StopLaserParticles()
        {
            if (_laserParticles == null) return;

            foreach (ParticleSystem particle in _laserParticles)
            {
                if (particle != null)
                    particle.Stop();
            }
        }

        private Transform GetRandomFirePoint()
        {
            if (errorArrowFirePoint == null || errorArrowFirePoint.Length == 0)
                return null;

            return errorArrowFirePoint[Random.Range(0, errorArrowFirePoint.Length)];
        }
    }
}