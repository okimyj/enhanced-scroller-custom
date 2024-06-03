using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Custom
namespace EnhancedUI.EnhancedScroller
{
    public class EnhancedScrollerCellView<TCellData, TContext> : EnhancedScrollerCellView
    where TCellData : IEnhancedScrollerCellData
    where TContext : IEnhancedScrollerContext
    {
        [SerializeField] public TContext Context { get; private set; }
        protected TCellData cellData;
        public float CellSize;
        public virtual void SetCellData(TCellData cellData)
        {
            this.cellData = cellData;
        }
        public override void RefreshCellView()
        {
            SetCellData(cellData);
        }
        public virtual void SetContext(TContext context)
        {
            Context = context;
        }
    }
    public class EnhancedScrollerCellView<TCellData> : EnhancedScrollerCellView<TCellData, NullContext>
    where TCellData : IEnhancedScrollerCellData
    {

    }


}
