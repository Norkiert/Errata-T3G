using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

namespace Dialogues
{
    public static class DialogueInkKeeper
    {
        public static TextAsset CurrentText => texts.Count == 0 ? null : texts[texts.Count - 1];

        private static readonly List<TextAsset> texts = new List<TextAsset>();

        public static void AddNewText(TextAsset newText)
        {
            if (newText == null)
                Debug.LogWarning("AddNewText: Attempted to add empty text!");
            else
                texts.Add(newText);
        }

        public static void RemoveText(TextAsset newText)
        {
            if (newText == null)
                Debug.LogWarning("AddNewText: Attempted to remove empty text!");
            else
                texts.Remove(newText);
        }
    }
}