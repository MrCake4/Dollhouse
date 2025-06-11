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

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
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
            switch (input)
            {
                case "enter":
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 9) // 9 max passcode size 
                    {
                        return;
                    }
                    currentInput += input;
                    break;
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
            yield return new WaitForSeconds(displayResultTime);
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

        void Update()
        {
            // When there are 3 or more lit candles check if the current input matches the keypad combo
            if (litCandleCount >= 3 && !accessWasGranted)
            {
                CheckCombo();
            }
        }
    }
}