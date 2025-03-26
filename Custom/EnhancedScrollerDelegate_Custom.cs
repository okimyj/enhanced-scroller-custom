using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedUI.EnhancedScroller
{
    public abstract class EnhancedScrollerDelegate<TCellData> : MonoBehaviour, IEnhancedScrollerDelegate
    where TCellData : IEnhancedScrollerCellData
    {
        [SerializeField] protected EnhancedScroller scroller;
        public EnhancedScroller Scroller => scroller;
        protected List<TCellData> cellDatas = new List<TCellData>();
        public bool ScrollRectEnabled
        {
            get { return scroller.ScrollRect.enabled; }
            set { scroller.ScrollRect.enabled = value; }
        }
        private void Start()
        {
            scroller.Delegate = this;

            Initialize();
        }
        protected virtual void Initialize() { }
        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
        public virtual void SetCellDatas(List<TCellData> cellDatas, float scrollPositionFactor = 0f)
        {
            this.cellDatas = cellDatas;
            scroller.ReloadData(scrollPositionFactor);
        }
        public virtual bool SetCellDataAtDataIndex(TCellData cellData, int dataIndex)
        {
            if(this.cellDatas.Count > 0 && this.cellDatas.Count > dataIndex)
            {
                this.cellDatas[dataIndex] = cellData;
                return true;
            }
            return false;
        }


        public abstract float GetCellViewSize(EnhancedScroller scroller, int dataIndex);

        public virtual int GetNumberOfCells(EnhancedScroller scroller)
        {
            return cellDatas.Count;
        }
        public EnhancedScrollerCellView GetCellViewAtDataIndex(int index)
        {
            return scroller.GetCellViewAtDataIndex(index);
        }

    }
    public abstract class EnhancedScrollerDelegate<TCellData, TCellView, TContext> : EnhancedScrollerDelegate<TCellData>
    where TCellData : IEnhancedScrollerCellData
    where TCellView : EnhancedScrollerCellView<TCellData, TContext>
    where TContext : class, IEnhancedScrollerContext, new()
    {

        public TContext Context { get; } = new TContext();
        [SerializeField] protected TCellView defaultCellPrefab;

        public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            var cellView = scroller.GetCellView(defaultCellPrefab) as TCellView;
            cellView.SetContext(Context);
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
        public new virtual TCellView GetCellViewAtDataIndex(int index)
        {
            return scroller.GetCellViewAtDataIndex(index) as TCellView;
        }
        public override bool SetCellDataAtDataIndex(TCellData cellData, int dataIndex)
        {
            if(base.SetCellDataAtDataIndex(cellData, dataIndex))
            {
                var cellView = GetCellViewAtDataIndex(dataIndex);
                cellView.SetCellData(cellData);
                return true;
            }
            return false;
        }

    }

    public abstract class EnhancedScrollerDelegate<TCellData, TCellView> : EnhancedScrollerDelegate<TCellData, TCellView, NullContext>
     where TCellData : IEnhancedScrollerCellData
     where TCellView : EnhancedScrollerCellView<TCellData, NullContext>
    {


    }


}
