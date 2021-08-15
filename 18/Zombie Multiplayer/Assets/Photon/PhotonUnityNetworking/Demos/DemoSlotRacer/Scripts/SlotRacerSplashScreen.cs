﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlotRacerSpashScreen.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in SlotRacer Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Photon.Pun.Demo.SlotRacer
{
    /// <summary>
    /// Slot racer splash screen. Inform about the slotRacer demo and the Cockpit control setup
    /// Gets deleted as soon as the scene plays
    /// </summary>
    [ExecuteInEditMode]
    public class SlotRacerSplashScreen : MonoBehaviour
    {
#pragma warning disable 0414
        private string PunCockpit_scene = "PunCockpit-Scene";
#pragma warning restore 0414

        public Text WarningText;
        public GameObject SplashScreen;

        private void Start()
        {
            if (Application.isPlaying)
            {
                Destroy(this.SplashScreen);
                Destroy(this);
            }
        }

        public void Update()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (this.WarningText == null)
                {
                    return;
                }

                bool _found = false;
                bool _enabled = false;

                foreach (EditorBuildSettingsScene _scene in EditorBuildSettings.scenes)
                {
                    if (_scene.path.EndsWith(this.PunCockpit_scene + ".unity"))
                    {
                        _found = true;
                        _enabled = _scene.enabled;
                        break;
                    }
                }

                if (_found && _enabled)
                {
                    this.WarningText.text = string.Empty;
                    return;
                }

                if (_found && !_enabled)
                {
                    this.WarningText.text = "<Color=Green>INFORMATION:</Color>\nThis demo can run with the PunCockpit Scene." +
                                            "\nFor this, the Scene '" +
                                            this.PunCockpit_scene +
                                            "' needs to be enabled to the build settings." +
                                            "\nElse, you'll get only a minimal Menu to connect";
                    return;
                }

                if (!_found)
                {
                    this.WarningText.text = "<Color=Green>INFORMATION:</Color>\nThis demo can run with the PunCockpit Scene." +
                                            "\n For this, the Scene '" +
                                            this.PunCockpit_scene +
                                            "' needs to be added to the build settings." +
                                            "\nElse, you'll get only a minimal Menu to connect";
                }
            }
            #endif
        }
    }
}