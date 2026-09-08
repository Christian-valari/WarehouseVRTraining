using UnityEngine;
using Valari.Managers;

namespace ImmersiveTraining.TrainingInteractions
{
    /// <summary>
    /// Presents the path-efficiency route overlay when the M1_08 modal opens. Place on the M1_08
    /// tutorial panel. Gated on the OrderPicking module having started (the tutorial panels are saved
    /// active, so OnEnable also fires once at scene load) — when the step genuinely opens it stops the
    /// route recording and shows the overlay for review. The step still completes via the existing
    /// grab-to-confirm marker.
    /// </summary>
    public class RouteReviewPresenter : MonoBehaviour
    {
        [SerializeField] private RouteRecorder _recorder;
        [SerializeField] private RouteOverlayController _overlay;

        private bool _armed;
        private bool _done;

        private void Awake()
        {
            TutorialManager.OnTrainingStartedEvent -= HandleTrainingStarted;
            TutorialManager.OnTrainingStartedEvent += HandleTrainingStarted;
        }

        private void OnDestroy()
        {
            TutorialManager.OnTrainingStartedEvent -= HandleTrainingStarted;
        }

        private void HandleTrainingStarted(TrainingID id)
        {
            if (id != TrainingID.OrderPicking) return;
            _armed = true;
            _done = false;
        }

        private void OnEnable()
        {
            if (_done) return;
            if (!_armed) return; // ignore the scene-load / pre-module enable

            if (_recorder != null) _recorder.StopRecording();
            if (_overlay != null) _overlay.ShowOverlay();
            _done = true;
        }
    }
}
