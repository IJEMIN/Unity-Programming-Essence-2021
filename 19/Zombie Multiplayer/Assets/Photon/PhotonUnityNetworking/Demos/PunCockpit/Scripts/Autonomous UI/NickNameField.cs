﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NickNameField.cs" company="Exit Games GmbH">
//   Part of: Pun Cockpit Demo
// </copyright>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.Cockpit
{
    /// <summary>
    /// Nickname InputField.
    /// </summary>
    public class NickNameField : MonoBehaviour
    {
        public InputField PropertyValueInput;

        string _cache;

        bool registered;

        void OnEnable()
        {
            if (!registered)
            {
                registered = true;
                PropertyValueInput.onEndEdit.AddListener(OnEndEdit);
            }
        }

        void OnDisable()
        {
            registered = false;
            PropertyValueInput.onEndEdit.RemoveListener(OnEndEdit);
        }

        void Update()
        {
            if (PhotonNetwork.NickName != _cache)
            {
                _cache = PhotonNetwork.NickName;
                PropertyValueInput.text = _cache;
            }
        }

        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then submit form.
        public void OnEndEdit(string value)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Tab))
            {
                this.SubmitForm(value.Trim());
            }
            else
            {
                this.SubmitForm(value);
            }
        }

        public void SubmitForm(string value)
        {
            _cache = value;
            PhotonNetwork.NickName = _cache;
            //Debug.Log("PhotonNetwork.NickName = " + PhotonNetwork.NickName, this);
        }
    }
}