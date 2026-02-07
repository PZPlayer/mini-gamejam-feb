using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jam.UI
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private GameObject _transition;
        [SerializeField] private Transform _canvas;

        public async void TravelToScene(int sceneID)
        {
            print("GO");
            Instantiate(_transition, _canvas);
            await WaitUntilAnimation(1, sceneID);
        }

        private async UniTask WaitUntilAnimation(float delay, int id)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            SceneManager.LoadScene(id);
        }
    }
}

