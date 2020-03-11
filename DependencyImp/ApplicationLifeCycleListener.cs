using System;

namespace TeamZero.Core.Unity
{
    public abstract class ApplicationLifeCycleListener : IApplicationLifeCycleListener
    {
        public event Action ApplicationLaunch;
        public event Action ApplicationEarlyResume;
        public event Action ApplicationResume;
        public event Action ApplicationPause;
        public event Action ApplicationLatePause;
        public event Action ApplicationLoseFocus;
        public event Action ApplicationQuit;

        private void Start()
        {
            ProcessOnLaunch();
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                ProcessOnPause();
            else
                ProcessOnResume();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            ProcessOnFocus(hasFocus);
        }

        private void OnApplicationQuit()
        {
            ProcessOnQuit();
        }

        private void OnDestroy()
        {
            ProcessOnQuit();
        }

        private void ProcessOnLaunch()
        {
            ApplicationLaunch?.Invoke();
            ProcessOnResume();
        }

        private bool _paused = true;
        private void ProcessOnResume()
        {
            if (_paused)
            {
                _paused = false;
                ApplicationEarlyResume?.Invoke();
                ApplicationResume?.Invoke();
            }
        }

        private void ProcessOnPause()
        {
            if (!_paused)
            {
                _paused = true;
                ApplicationPause?.Invoke();
                ApplicationLatePause?.Invoke();
            }
        }
        
        private void ProcessOnFocus(bool hasFocus)
        {
            if(!hasFocus)
                ApplicationLoseFocus?.Invoke();
        }

        private bool _quiting = false;
        private void ProcessOnQuit()
        {
            if (!_quiting)
            {
                _quiting = true;
                ProcessOnPause();
                ApplicationQuit?.Invoke();
            }
        }
    }
}
