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
        public float CellSize;
        public virtual void SetCellData(TCellData cellData)
        {
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
