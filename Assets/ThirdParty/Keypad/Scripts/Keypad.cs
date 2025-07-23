using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private int keypadCombo = 12345;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private AudioSource audioSource;


        private string currentInput;
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        // Winstons modifications
        [Header("Candles")]
        public int litCandleCount = 0; // Count of lit candles
        [SerializeField] Candle[] candles;  // stores all candles in the scene that controll the keypad

        private void Awake()
        {
            ClearInput();
        }

        //Gets value from pressedbutton
        public void AddInput(string input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (displayingResult || accessWasGranted) return;
            
            if (currentInput.Length == 2)
            {
                currentInput += input;
                CheckCombo();
            }
            else
            {
                if (currentInput != null && currentInput.Length >= 3) // 9 max passcode size 
                {
                return;
                }
                currentInput += input;
            }
                    
        }
        public void CheckCombo()
        {
            if (currentInput == null || currentInput.Length != keypadCombo.ToString().Length)
            {
                Debug.LogWarning("Input length does not match combo length.");
                return;
            }

            string comboStr = keypadCombo.ToString();
            char[] comboChars = comboStr.ToCharArray();
            char[] inputChars = currentInput.ToCharArray();
            Array.Sort(comboChars);
            Array.Sort(inputChars);

            bool granted = new string(comboChars) == new string(inputChars);

            if (!displayingResult)
            {
                StartCoroutine(DisplayResultRoutine(granted));
            }
        }

        //mainly for animations 
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();
            displayingResult = false;
            if (granted) yield break;
            ClearInput();

        }

        private void AccessDenied()
        {
            onAccessDenied?.Invoke();
            audioSource.PlayOneShot(accessDeniedSfx);
            litCandleCount = 0; 

            if(candles != null)
            {
                foreach (var candle in candles)
                {
                    candle.extinguishCandle();
                }
            }
        }

        private void ClearInput()
        {
            currentInput = "";
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            onAccessGranted?.Invoke();
            audioSource.PlayOneShot(accessGrantedSfx);

            // light all candles
            if (candles == null || candles.Length == 0) return;
            
            foreach (var candle in candles)
            {
                candle.lightCandle();
            }
        }

        public void setKeyPadCombo(int newCombo)
        {
            keypadCombo = newCombo;
        }

    }
}