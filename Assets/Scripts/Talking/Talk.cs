using UnityEngine;
using System.Collections.Generic;
using System;

namespace Jam.Talking
{
    [Serializable]
    public enum TalkingEntites { Player, Poseydon, Girlfriend, Event}

    [Serializable]
    public struct ButtonChoice
    {
        public Talk leadTalk;
        public int wantedIndex;
        public string buttonText;
    }

    [Serializable]
    public struct Speech
    {
        public TalkingEntites entity;
        public string speechText;
        public List<ButtonChoice> buttons;
    }

    [CreateAssetMenu(fileName = "Talk", menuName = "Scriptable Objects/Talk")]
    public class Talk : ScriptableObject
    {
        public List<Speech> speechList;
    }
}

