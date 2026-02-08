using UnityEngine;
using System.Collections.Generic;
using System;

namespace Jam.Talking
{
    [Serializable]
    public enum TalkingEntites { Player, Poseydon, Girlfriend, GFPhone, Event}

    [Serializable]
    public struct ButtonChoice
    {
        public int choiceAttitude;
        public Talk leadTalk;
        public string buttonText;
    }

    [Serializable]
    public struct Speech
    {
        public TalkingEntites entity;
        public string speechText;
        public int wantedIndex;
        public List<ButtonChoice> buttons;
    }

    [CreateAssetMenu(fileName = "Talk", menuName = "Scriptable Objects/Talk")]
    public class Talk : ScriptableObject
    {
        public List<Speech> speechList;
    }
}

