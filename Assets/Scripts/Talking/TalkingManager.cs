using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Jam.Talking
{
    [Serializable]
    public struct TalkEvents
    {
        public Talk talk;
        public UnityEvent OnEnd;
        public UnityEvent OnStart;
    }

    public class TalkingManager : MonoBehaviour
    {
        [SerializeField] private List<TalkEvents> _talks;
        [SerializeField] private TextMeshProUGUI _talkingText;
        [SerializeField] private Transform _buttonsSpawn;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _nextButton;

        [SerializeField] private TalkEvents currentDialogue;
        [SerializeField] private bool isTyping;
        [SerializeField] private bool isInteractive;
        [SerializeField] private string curWord;
        [SerializeField] private int talkIndex = 0;
        [SerializeField] private int dialogueIndex = 0;
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            currentDialogue = _talks[gameManager.CurrentDialogue];
            dialogueIndex = gameManager.CurrentDialogue;
            gameManager.TestIfWorks();
            MoveForward();
        }

        [Inject]
        public void GetGameManager(GameManager mangaer)
        {
            gameManager = mangaer;
        }

        public void SetDialogueIndex(int index)
        {
            dialogueIndex = index;
            gameManager.CurrentDialogue = dialogueIndex;
        }

        public void ChangeDialogue(ButtonChoice butn)
        {
            talkIndex = Mathf.Max(butn.wantedIndex, 0);
            currentDialogue = new TalkEvents();
            currentDialogue.talk = butn.leadTalk;

            isTyping = false;
            ClearButtons();
        }

        public void ClearButtons()
        {
            for (int i = 0;  i < _buttonsSpawn.childCount; i++)
            {
                Destroy(_buttonsSpawn.GetChild(i).gameObject);
            }

            isInteractive = false;
            MoveForward();
            _nextButton.SetActive(true);
        }

        private void OnDestroy()
        {
            foreach (TalkEvents even in _talks)
            {
                even.OnStart.RemoveAllListeners();
                even.OnEnd.RemoveAllListeners();
                Debug.Log("Cleared");
            }
        }

        public async void MoveForward()
        {
            if (currentDialogue.talk.speechList.Count <= talkIndex)
            {
                _talks[dialogueIndex].OnEnd?.Invoke();
                Debug.Log("NO MORE DIALOGUES");
                return;
            }

            if (currentDialogue.talk.speechList[talkIndex].buttons.Count != 0)
            {
                isInteractive = true;
                _nextButton.SetActive(false);
                List<ButtonChoice> buttons = currentDialogue.talk.speechList[talkIndex].buttons;
                foreach (ButtonChoice button in buttons)
                {
                    GameObject newButton = Instantiate(_buttonPrefab, _buttonsSpawn);
                    newButton.GetComponentInChildren<TextMeshProUGUI>().text = button.buttonText;
                    if (button.leadTalk != null)
                    {
                        newButton.GetComponent<Button>().onClick.AddListener(() => ChangeDialogue(button));
                    }
                    else
                    {
                        newButton.GetComponent<Button>().onClick.AddListener(ClearButtons);
                    }
                }
            }

            if(isTyping)
            {
                isTyping = false;
            }
            else
            {
                await TypeText(currentDialogue.talk.speechList[talkIndex].speechText);
            }
        }

        private async UniTask TypeText(string text)
        {
            curWord = text;
            string myWord = text;
            _talkingText.text = "";
            isTyping = true;

            for (int i = 0;  i < text.Length; i++)
            {
                _talkingText.text += text[i];

                if (!isTyping || myWord != curWord) break;

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds((text[i].ToString() != "," || text[i].ToString() != ".") ? 0.05f : 0.2f));
                }
                catch
                {
                    break;
                }
            }

            if (myWord == curWord) { _talkingText.text = text; curWord = ""; talkIndex++; }
            
            isTyping = false;
        }
    }
}

