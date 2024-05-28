using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace EnhancedUI.EnhancedScroller
{
    public sealed class NullContext : IEnhancedScrollerContext { }
    public abstract class EnhancedGridScrollerDelegate<TCellData, TCellView>
    : EnhancedGridScrollerDelegate<TCellData, TCellView, NullContext>
    where TCellData : class, IEnhancedScrollerCellData
    where TCellView : EnhancedScrollerCellView<TCellData>
    { }
    public abstract class EnhancedGridScrollerDelegate<TCellData, TCellView, TContext> : MonoBehaviour, IEnhancedScrollerDelegate
        where TCellData : class, IEnhancedScrollerCellData
        where TCellView : EnhancedScrollerCellView<TCellData, TContext>
        where TContext : class, IEnhancedScrollerContext, new()
    {
        [SerializeField] protected int gridCellCount;
        [SerializeField] protected float cellSpace;
        [SerializeField] protected EnhancedScroller scroller;
        protected class DefaultCellViewGroup : EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext> { }
        public TContext Context { get; } = new TContext();
        private Transform groupPrefabParent;


        protected List<TCellData> cellDatas = new List<TCellData>();
        private Dictionary<string, EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext>> groupCellMap = new Dictionary<string, EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext>>();
        private void Start()
        {
            scroller.Delegate = this;
            Initialize();
        }
        protected virtual void Initialize() { }
        private Transform GetGroupParent()
        {
            groupPrefabParent = new GameObject("GroupPrefabParent").transform;
            groupPrefabParent.gameObject.SetActive(false);
            return groupPrefabParent;
        }
        protected virtual TGroup GetCellViewGroupPrefab<TGroup>(TCellView cellPrefab) where TGroup : EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext>
        {
            if (!groupCellMap.ContainsKey(cellPrefab.cellIdentifier))
            {
                var go = new GameObject($"{cellPrefab.cellIdentifier}_Group");
                go.transform.SetParent(GetGroupParent());
                var cellViewGroup = go.AddComponent<TGroup>();
                cellViewGroup.cellIdentifier = $"{cellPrefab.cellIdentifier}_Group";
                var cellViews = new TCellView[gridCellCount];
                for (int i = 0; i < gridCellCount; ++i)
                {
                    var cell = Instantiate(cellPrefab, go.transform);
                    var cellReact = cell.GetComponent<RectTransform>();
                    if (scroller.scrollDirection == EnhancedScroller.ScrollDirectionEnum.Vertical)
                        cellReact.anchoredPosition = new Vector2(i * (cellReact.sizeDelta.x + cellSpace), 0);
                    else
                        cellReact.anchoredPosition = new Vector2(0, -i * (cellReact.sizeDelta.y + cellSpace));
                    cell.name = $"{cellPrefab.cellIdentifier}_{i}";
                    cellViews[i] = cell;
                }
                cellViewGroup.SetCellViews(cellViews);
                groupCellMap[cellPrefab.cellIdentifier] = cellViewGroup;
            }
            return groupCellMap[cellPrefab.cellIdentifier] as TGroup;
        }
        protected EnhancedScrollerCellView GetCellViewGroup<TGroup>(TCellView cellPrefab) where TGroup : EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext>
        {
            var groupPrefab = GetCellViewGroupPrefab<TGroup>(cellPrefab);
            var cellView = scroller.GetCellView(groupPrefab);
            var groupCellView = cellView as TGroup;
            groupCellView.SetContext(Context);
            return cellView;
        }

        public virtual void SetCellDatas(List<TCellData> datas)
        {
            cellDatas = datas;
            scroller.ReloadData();
        }


        /// <summary>
        /// 각 데이터에 맞는 cellView를 반환.
        /// </summary>
        /// <param name="scroller"></param>
        /// <param name="dataIndex"></param>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
        public abstract float GetCellViewSize(EnhancedScroller scroller, int dataIndex);

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)cellDatas.Count / gridCellCount);
        }
    }

}
