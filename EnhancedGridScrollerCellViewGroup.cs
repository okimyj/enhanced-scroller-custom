using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
namespace EnhancedUI.EnhancedScroller
{
    public class EnhancedGridScrollerCellViewGroup<TCellData, TCellView, TContext> : EnhancedScrollerCellView<TCellData, TContext>
     where TCellView : EnhancedScrollerCellView<TCellData, TContext>
     where TCellData : class, IEnhancedScrollerCellData
     where TContext : class, IEnhancedScrollerContext
    {
        [SerializeField] private TCellView[] cellViews;

        public override void SetContext(TContext context)
        {
            base.SetContext(context);
            for (int i = 0; i < cellViews.Length; i++)
            {
                cellViews[i].SetContext(context);
            }
        }
        public void SetCellViews(TCellView[] cellViews)
        {
            this.cellViews = cellViews;
        }
        public void SetCellDatas(ref List<TCellData> cellDatas, int startingIndex)
        {
            for (int i = 0; i < cellViews.Length; i++)
            {
                var dataIndex = startingIndex + i;
                if (dataIndex < cellDatas.Count)
                {
                    cellViews[i].gameObject.SetActive(true);
                    cellViews[i].dataIndex = dataIndex;
                    cellViews[i].SetCellData(cellDatas[dataIndex]);
                }
                else
                {
                    cellViews[i].gameObject.SetActive(false);
                }
            }
        }
        public override void RefreshCellView()
        {
            base.RefreshCellView();
            for (int i = 0; i < cellViews.Length; i++)
            {
                cellViews[i].RefreshCellView();
            }
        }

    }

}
