using I2.Loc;
using UnityEngine;

namespace Base
{
    public class SceneBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (!LocalStorage.IsSetLanguage || LocalizationManager.GetAllLanguages().IndexOf(LocalizationManager.CurrentLanguage) <= 1)
            {
                LocalizationManager.CurrentLanguage = $"{Application.systemLanguage}";
                LocalStorage.IsSetLanguage = true;
            }
        }

        protected virtual void Start() { }
    }
}