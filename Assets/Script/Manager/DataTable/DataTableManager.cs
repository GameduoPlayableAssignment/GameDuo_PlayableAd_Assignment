using Helper;
using Model.Dao;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace Manager.DataTable
{
    public class DataTableManager : Singleton<DataTableManager>
    {
        private string _jsonText = string.Empty;

        public void ParseData(Action completeCallback)
        {
            StartCoroutine(ParseDataCo(completeCallback));
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator ParseDataCo(Action completeCallback)
        {
            yield return ParseDataByJsonTextCo<List<DummyDao>>();

            completeCallback?.Invoke();
        }

        private IEnumerator ParseDataByJsonTextCo<T>()
        {
            // ex: MonsterDao (참고: typeof(T) 값은 System.Collections.Generic.List`1[Model.Dao.MonsterDao])
            string daoClassName = typeof(T).GenericTypeArguments[0].Name;
        
            // ex: Monster
            string classNameWithoutDao = daoClassName.Replace("Dao", string.Empty);
        
            // ex: GameData/Monster.json 
            string jsonFilePath = $"GameData/{classNameWithoutDao}.json";
        
            if (Application.platform == RuntimePlatform.Android)
            {
                using UnityWebRequest www = UnityWebRequest.Get(CommonHelper.GetPath(jsonFilePath));
                www.SendWebRequest();
                while (!www.isDone) { }
                _jsonText = www.downloadHandler.text;
            }
            else
            {
                _jsonText = File.ReadAllText(CommonHelper.GetPath(jsonFilePath));
            }

            try
            {
                MethodInfo method = Type.GetType($"Model.Table.{classNameWithoutDao}Table")?.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public);
                method?.Invoke(null, new object[] { JsonConvert.DeserializeObject<T>(_jsonText) });
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#else
                PopupHelper.ShowClientErrorCodePopup(ErrorCode.CLIENT_RAW_DATA_EXCEPTION, e.Message);
#endif
                throw;
            }

            yield return null;
        }
    }
}