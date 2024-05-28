using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace EnhancedUI.EnhancedScroller
{
    public abstract class EnhancedScrollerDelegate<TCellData> : MonoBehaviour, IEnhancedScrollerDelegate
    where TCellData : IEnhancedScrollerCellData
    {
        [SerializeField] protected EnhancedScroller scroller;
        protected List<TCellData> cellDatas = new List<TCellData>();
        private void Awake()
        {
            scroller.Delegate = this;
        }
        public virtual void SetCellDatas(List<TCellData> cellDatas)
        {
            this.cellDatas = cellDatas;
            scroller.ReloadData();
        }
        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);

        public abstract float GetCellViewSize(EnhancedScroller scroller, int dataIndex);

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return cellDatas.Count;
        }
    }
    public abstract class EnhancedScrollerDelegate<TCellData, TCellView> : EnhancedScrollerDelegate<TCellData>
     where TCellData : IEnhancedScrollerCellData
     where TCellView : EnhancedScrollerCellView<TCellData>
    {

        [SerializeField] protected TCellView defaultCellPrefab;

        public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(defaultCellPrefab) as TCellView;
            cellView.SetCellData(cellDatas[dataIndex]);
            return cellView;
        }
        public override float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
#if UNITY_EDITOR
            if (defaultCellPrefab != null && defaultCellPrefab.CellSize == 0)
                Debug.LogWarning($"EnhancedScrollerDelegate:GetCellViewSize - CellSize is 0, please set the CellSize in the prefab. cell view type : {typeof(TCellView).Name}");
            else if (defaultCellPrefab == null)
                Debug.LogWarning($"EnhancedScrollerDelegate:GetCellViewSize - default cell prefab is null. cell view type : {typeof(TCellView).Name}");
#endif

            return defaultCellPrefab?.CellSize ?? 0;
        }
    }
}
