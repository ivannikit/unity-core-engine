using System;
using UnityEngine;

namespace TeamZero
{
    [DisallowMultipleComponent]
    public class ApplicationDelegate : MonoBehaviour
    {
        private bool _isShuttingDown = false;
        private bool _isApplicationPaused = true;

        public event Action Launch;
        public event Action Resume;
        public event Action Pause;
        public event Action Quit;

        private void Start()
        {
            OnLaunch();
            OnResume();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                OnPause();
            else
                OnResume();
        }

        private void OnApplicationQuit()
        {
            if (!_isShuttingDown)
            {
                _isShuttingDown = true;
                OnPause();
                OnQuit();
            }
        }

        private void OnDestroy() => OnPause();

        private void OnLaunch() => Launch?.Invoke();

        private void OnResume()
        {
            if (_isApplicationPaused)
            {
                _isApplicationPaused = false;
                Resume?.Invoke();
            }
        }

        private void OnPause()
        {
            if (!_isApplicationPaused)
            {
                _isApplicationPaused = true;
                Pause?.Invoke();
            }
        }

        private void OnQuit() => Quit?.Invoke();
    }
}
