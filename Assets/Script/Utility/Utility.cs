using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public partial class Utility
{
    /// <summary>
    /// 캔바스 기준 좌표를 월드좌표로 변환해서 가져오기
    /// </summary>
    public static Vector2 GetScreenToWorldPosition(Canvas canvas, Vector3 position)
    {
        Vector3 screenPoint = new Vector3(position.x, position.y, 100.0f);
        return canvas.worldCamera.ScreenToWorldPoint(screenPoint);
    }

    /// <summary>
    /// 랜덤 리스트 섞기
    /// </summary>
    public static List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();
        List<E> inputListCopy = new (inputList);
        //System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputListCopy.Count > 0)
        {
            //randomIndex = r.Next(0, inputList.Count);
            randomIndex = Random.Range(0, inputListCopy.Count);
            randomList.Add(inputListCopy[randomIndex]);
            inputListCopy.RemoveAt(randomIndex);
        }
        
        return randomList;
    }

    /// <summary>
    /// hex값으로 컬러 값 가져오기
    /// </summary>
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
        byte a = 255; //assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
        }

        return new Color32(r, g, b, a);
    }

    public static string ColorToHex(Color color)
    {
        Color32 c = color;
        return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);
    }


    public static void ChangeLayer(Transform trans, LayerMask layerMask)
    {
        int layerIndex = layerMask;
        trans.gameObject.layer = layerIndex;
        foreach (Transform t in trans)
        {
            t.gameObject.layer = layerIndex;
            ChangeLayer(t, layerMask);
        }
    }
    
    /// <summary>
    /// int > 1,000,000 변환 string
    /// </summary>
    public static string ChangeMoneyString(int myScore)
    {
        //return string.Format("{0:n0}", myScore);
        return $"{myScore:n0}";
    }

    public static string ChangeQuizExSentence(string sentence)
    {
        string exSen = sentence;

        exSen.Replace("_", "<#c1c0bd><u><#1389cb00>aaaaaa</color></u></color>");
        
        return exSen;
    }

    public static string ChangeAnswerExSentence(string sentence, string word)
    {
        string answer = sentence;

        answer.Replace("_", $"<#c1c0bd><u><#1389cbff>{word}</color></u></color>");
        
        return answer;
    }

    /// <summary>
    /// 이름으로 sprite찾아서 가져오기
    /// </summary>
    public static Sprite GetItemSprite(Sprite[] sprites, string name)
    {
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }

        return null;
    }

    public static void SetScrollRect(ScrollRect scrollRect)
    {
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.elasticity = 0.05f;
        scrollRect.scrollSensitivity = 10f;
    }
    /// <summary>
    ///  radius값 원안에 랜덤 point값 가져오기
    /// </summary>
    public static Vector2 RandomSphereInPoint(float radius)
    {
        Vector2 getPoint = Random.onUnitSphere;

        float r = Random.Range(0.0f, radius);

        return (getPoint * r);
    }
    
    public static bool TryGetArg(string argName, out string argValue) {

        var args = Environment.GetCommandLineArgs();
        argValue = null;

        for (int i = 0; i < args.Length; i++) {
            if (args[i].Equals(argName) && args.Length > i + 1) {

                argValue = args[i + 1];
                return true;
            }
        }

        return false;
    }
    
    public static bool HasCommandLineArgument(string name)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; ++i)
        {
            if (arguments[i] == name)
                return true;
        }

        return false;
    }

    public static bool GetCommandLineArgument(string name, out string argument)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; ++i)
        {
            if (arguments[i] == name && arguments.Length > (i + 1))
            {
                argument = arguments[i + 1];
                return true;
            }
        }

        argument = default;
        return false;
    }

    public static bool GetCommandLineArgument(string name, out int argument)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; ++i)
        {
            if (arguments[i] == name && arguments.Length > (i + 1) && int.TryParse(arguments[i + 1], out int parsedArgument))
            {
                argument = parsedArgument;
                return true;
            }
        }

        argument = default;
        return false;
    }
    /// <summary>
    /// 초 -> 시,분,초
    /// </summary>

    public static string SecondToHHMMSS(int second)
    {
        var sec = TimeSpan.FromSeconds(second);
        var time = sec.ToString(@"hh\:mm\:ss");
        return time;
    }
    /// <summary>
    /// 초 -> 분,초
    /// </summary>
    public static string SecondToMMSS(int second)
    {
        var sec = TimeSpan.FromSeconds(second);
        var time = sec.ToString(@"mm\:ss");
        return time;
    }
    
    
    


}
