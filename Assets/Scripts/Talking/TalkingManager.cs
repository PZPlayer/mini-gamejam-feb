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
        [SerializeField] private Color _playerTextColor;
        [SerializeField] private Color _poseydonTextColor;
        [SerializeField] private Color _girlfriendTextColor;

        [SerializeField] private UnityEvent OnWinNeutral;
        [SerializeField] private UnityEvent OnWinGood;
        [SerializeField] private UnityEvent OnWinBad;

        [SerializeField] private TalkEvents currentDialogue;
        [SerializeField] private bool isTyping;
        [SerializeField] private bool isInteractive;
        [SerializeField] private string curWord;
        [SerializeField] private int talkIndex = 0;
        [SerializeField] private int dialogueIndex = 0;
        [SerializeField] private GameManager gameManager;
        private PhoneTalking phoneTalking;

        private void Start()
        {
            try
            {
                currentDialogue = _talks[gameManager.CurrentDialogue];
                dialogueIndex = gameManager.CurrentDialogue;
            }
            catch
            {
                if (gameManager.PoseidonLikeRate > 2)
                {
                    OnWinBad?.Invoke();
                }
                else if (gameManager.PoseidonLikeRate > -1)
                {
                    OnWinNeutral?.Invoke();
                }
                else
                {
                    OnWinBad?.Invoke();
                }

                Debug.Log("Win, all cleared!");
            }
            
            phoneTalking = GetComponent<PhoneTalking>();
            gameManager.TestIfWorks();
            print("Current Poseidon attitude score: " + gameManager.PoseidonLikeRate);
            currentDialogue.OnStart?.Invoke();
            MoveForward();
        }

        [Inject]
        public void GetGameManager(GameManager mangaer)
        {
            gameManager = mangaer;
        }

        public void SetPlayTime(int time)
        {
            gameManager.GamePlayTime = time;
        }

        public void SetDialogueIndex(int index)
        {
            dialogueIndex = index;
            gameManager.CurrentDialogue = dialogueIndex;
        }

        public void ChangeDialogue(ButtonChoice butn)
        {
            talkIndex = 0;
            currentDialogue = new TalkEvents();
            currentDialogue.talk = butn.leadTalk;

            isTyping = false;
            ClearButtons(butn);
        }

        public void ClearButtons(ButtonChoice butn)
        {
            for (int i = 0;  i < _buttonsSpawn.childCount; i++)
            {
                Destroy(_buttonsSpawn.GetChild(i).gameObject);
            }

            gameManager.PoseidonLikeRate += butn.choiceAttitude;

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
                if (currentDialogue.talk != _talks[dialogueIndex].talk)
                {
                    talkIndex = currentDialogue.talk.speechList[currentDialogue.talk.speechList.Count - 1].wantedIndex;
                    currentDialogue = _talks[dialogueIndex];
                }
                else
                {
                    _talks[dialogueIndex].OnEnd?.Invoke();
                    Debug.Log("NO MORE DIALOGUES");
                    return;
                }
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
                        newButton.GetComponent<Button>().onClick.AddListener(() => ClearButtons(button));
                    }
                }
            }

            if(isTyping)
            {
                isTyping = false;
            }
            else
            {
                if(currentDialogue.talk.speechList[talkIndex].entity == TalkingEntites.GFPhone)
                {
                    phoneTalking.WriteAMessage(currentDialogue.talk.speechList[talkIndex].speechText);
                    talkIndex++;
                }
                else
                {
                    await TypeText(currentDialogue.talk.speechList[talkIndex].speechText, currentDialogue.talk.speechList[talkIndex].entity);
                }
                
            }
        }

        private async UniTask TypeText(string text, TalkingEntites entites)
        {
            curWord = text;
            string myWord = text;
            string whoAmI = "";
            _talkingText.text = "";

            switch (entites)
            {
                case TalkingEntites.Girlfriend:
                    _talkingText.color = _girlfriendTextColor;
                    _talkingText.text = "[GF]: ";
                    whoAmI = "[GF]: ";
                    break;
                case TalkingEntites.Player:
                    _talkingText.color = _playerTextColor;
                    _talkingText.text = "[Player]: ";
                    whoAmI = "[Player]: ";
                    break;
                case TalkingEntites.Poseydon:
                    _talkingText.color= _poseydonTextColor;
                    _talkingText.text = "[Poseidon]: ";
                    whoAmI = "[Poseidon]: ";
                    break;
                default:
                    _talkingText.color = Color.black;
                    break;
            }

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

            if (myWord == curWord) { _talkingText.text = whoAmI + text; curWord = ""; talkIndex++; }
            
            isTyping = false;
        }
    }
}

