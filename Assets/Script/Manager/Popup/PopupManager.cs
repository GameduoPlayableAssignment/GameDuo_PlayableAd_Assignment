using Base;
using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Manager.Popup
{
    
    public class PopupManager : Singleton<PopupManager>
    {
        public ReactiveCollection<PopupBase> popupSystem { get; set; } = new();
        public ReactiveCollection<PopupBase> popupUI { get; set; } = new ();
        public ReactiveCollection<PopupBase> popupIgnore { get; set; } = new ();
        public ReactiveCollection<PopupBase> popupQueue { get; set; } = new();
        private PopupBase LastPopup { get; set; }
        public bool IsOpenPopup => popupSystem.Count == 0 && popupUI.Count == 0 && popupIgnore.Count == 0 && popupQueue.Count == 0;
        public IObservable<int> ObservableList => _observableList.Merge();
        private readonly List<IObservable<int>> _observableList = new();
        
        private void Awake()
        {
            //QueueCheck
            popupQueue.ObserveRemove().TakeUntilDestroy(this).Subscribe(_ =>
            {
                if (popupQueue.Count > 0)
                {
                    popupQueue[0].Show();
                }
            });
            _observableList.Add(popupSystem.ObserveCountChanged());
            _observableList.Add(popupUI.ObserveCountChanged());
            _observableList.Add(popupIgnore.ObserveCountChanged());
            _observableList.Add(popupQueue.ObserveCountChanged());
        }


        /// <summary>
        /// 팝업 생성
        /// 팝업을 생성하기위한 가장 기본적인 함수
        /// </summary>
        public PopupBase Create(PopupType popupType, bool isShow = true, bool isQueue = false)
        {
            PopupBase p = GetPopup(popupType); //팝업 게임오브젝트 생성해서 가져온다.
            p.popupType = popupType; //생성 이후 팝업을 구분하기위한 enum값 설정
            LastPopup = p; //마지막 팝업 설정

            CreatePopup(p, isShow, isQueue);
            return p;
        }
       
        /// <summary>
        /// 팝업오브젝트 생성 가져오기
        /// </summary>
        public PopupBase GetPopup(PopupType popupType)
        {
            PopupBase p = Instantiate(Resources.Load($"{Constant.PathUIPopup}{popupType}")).GetComponent<PopupBase>();
            p.transform.SetParent(transform, false);
            p.transform.SetAsFirstSibling();
            return p;
        }
        
        
        private PopupBase CreatePopup(PopupBase popup, bool isShow, bool isQueue)
        {
            if (isShow)
            {
                if (isQueue)
                {
                    if (popupQueue.Count <= 0)
                    {
                        popup.Show();
                    }
                }
                else
                {
                    popup.Show();
                }
            }
            
            if (isQueue)
            {
                //큐에 넣어야하는경우
                popupQueue.Add(popup);
            }
            else
            {
                //일반
                switch (popup.popupSameType)
                {
                    case PopupSameType.System:
                        popupSystem.Add(popup);
                        break;
                    case PopupSameType.UI:
                        popupUI.Add(popup);
                        break;
                    case PopupSameType.Ignore:
                        popupIgnore.Add(popup);
                        break;
                }
            }

            
            return popup;
        }


        
        
        
        
        
        

        public void CheckClosePopup()
        {
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i].isMoveToClose)
                {
                    popupUI[i].Close();
                }
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i].isMoveToClose)
                {
                    popupSystem[i].Close();
                }
            }
        }
    
        /// <summary>
        /// 지정한 타입의 모든패널을 삭제시킨다.
        /// </summary>
        public void CloseAll(PopupSameType popupSameType)
        {
            switch (popupSameType)
            {
                case PopupSameType.UI:
                    for (int i = popupUI.Count - 1; i >= 0 ; i--) popupUI[i].Close();
                    break;
                case PopupSameType.System:
                    for (int i = popupSystem.Count - 1; i >= 0 ; i--) popupSystem[i].Close();
                    break;
                case PopupSameType.All:
                    for (int i = popupUI.Count - 1; i >= 0 ; i--) popupUI[i].Close();
                    for (int i = popupSystem.Count - 1; i >= 0 ; i--) popupSystem[i].Close();
                    break;
                case PopupSameType.Queue:
                    for (int i = popupQueue.Count - 1; i >= 0 ; i--) popupQueue[i].Close();   
                    break;
            }

            LastPopup = null;
        }

    
    
        /// <summary>
        /// 동일한 패널타입이 리스트에 존재하는지 확인한다.
        /// </summary>
        public void GetActivePopup(PopupType popupType, out PopupBase popup)
        {
            popup = null;
        
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i].popupType == popupType)
                {
                    popup = popupUI[i];
                }
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i].popupType == popupType)
                {
                    popup = popupSystem[i];
                }
            }
        }
    
        /// <summary>
        /// 동일한 패널타입이 리스트에 존재하는지 확인한다.
        /// </summary>
        public void IsActivePopup(PopupType popupType, out PopupBase popup)
        {
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i].popupType == popupType)
                {
                    popup = popupUI[i];
                    return;
                }
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i].popupType == popupType)
                {
                    popup = popupSystem[i];
                    return;
                }
            }
            
            for (int i = 0; i < popupIgnore.Count; i++)
            {
                if (popupIgnore[i].popupType == popupType)
                {
                    popup = popupIgnore[i];
                    return;
                }
            }

            popup = null;
        }
    
        /// <summary>
        /// 지정한 타입과 동일한 패널을 가져온다.
        /// </summary>
        public PopupBase CurrentPopupByType(PopupType popupType)
        {
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i].popupType == popupType) return popupUI[i];
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i].popupType == popupType) return popupSystem[i];
            }
            
            for (int i = 0; i < popupIgnore.Count; i++)
            {
                if (popupIgnore[i].popupType == popupType) return popupIgnore[i];
            }
            return null;
        }

        public void CloseLastPopup(out bool success)
        {
            if (popupSystem.Count > 0)
            {
                popupSystem[^1].Close();
                success = true;
                return;
            }

            if (popupUI.Count > 0)
            {
                popupUI[^1].Close();
                success = true;
                return;
            }

            success = false;
            
#if UNITY_EDITOR
            DebugX.Log("팝업닫기 성공");
#endif
        }
    
    
        public void CloseByPopupType(PopupType popupType)
        {
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i].popupType == popupType)
                {
                    popupUI[i].Close();
                    break;
                }
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i].popupType == popupType)
                {
                    popupUI[i].Close();
                    break;
                }
            }
            
            for (int i = 0; i < popupIgnore.Count; i++)
            {
                if (popupIgnore[i].popupType == popupType)
                {
                    popupIgnore[i].Close();
                    break;
                }
            }
        }
    
        public void RemovePopup(PopupBase popupBase)
        {
            for (int i = 0; i < popupUI.Count; i++)
            {
                if (popupUI[i] == popupBase)
                {
                    popupUI.Remove(popupBase);
                    break;
                }
            }
        
            for (int i = 0; i < popupSystem.Count; i++)
            {
                if (popupSystem[i] == popupBase)
                {
                    popupSystem.Remove(popupBase);
                    break;
                }
            }
            
            for (int i = 0; i < popupIgnore.Count; i++)
            {
                if (popupIgnore[i] == popupBase)
                {
                    popupIgnore.Remove(popupBase);
                    break;

                }
            }

            for (int i = 0; i < popupQueue.Count; i++)
            {
                if (popupQueue[i] == popupBase)
                {
                    popupQueue.Remove(popupBase);
                    break;
                }
            }
        }
    }
}
