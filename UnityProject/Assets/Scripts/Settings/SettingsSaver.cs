using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    public class SettingsSaver : MonoBehaviour
    {
        public TogglerButton arrowTogglerButton;
        private void Start()
        {
            LoadArrowTogglerState();
        }
        
        public void ArrowTogglerButtonClicked()
        {
            SwitchArrowTogglerState();
            LoadArrowTogglerState();
        }

        private void LoadArrowTogglerState()
        {
            if (SaveProgress.Instance.ArrowButtonsActive)
            {
                SwitchToActive(arrowTogglerButton);
            }
            else
            {
                SwitchToInactive(arrowTogglerButton);
            }
        }

        private void SwitchArrowTogglerState()
        {
            SaveProgress.Instance.ArrowButtonsActive = !SaveProgress.Instance.ArrowButtonsActive;
            SaveProgress.Save();
        }

        private void SwitchToActive(TogglerButton togglerButton)
        {
            togglerButton.ActiveState.SetActive(true);
            togglerButton.InactiveState.SetActive(false);
        }
        private void SwitchToInactive(TogglerButton togglerButton)
        {
            togglerButton.ActiveState.SetActive(false);
            togglerButton.InactiveState.SetActive(true);
        }
    }
}
