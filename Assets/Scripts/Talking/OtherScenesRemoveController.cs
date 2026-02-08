using UnityEngine;
using Zenject;

namespace Jam.Talking
{
    public class OtherScenesRemoveController : MonoBehaviour
    {
        private GameManager gameManager;

        [Inject]
        public void Initialize(GameManager manager)
        {
            gameManager = manager;
        }

        public void AddToDialogue()
        {
            gameManager.CurrentDialogue += 1;
            print(transform.name + " HAve added dialogue");
        }
    }
}

