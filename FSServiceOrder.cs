using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.FS;
using PX.Data;

namespace PX.Objects.FS
{
    /// <summary>
    /// DAC Extension for FSServiceOrder.
    /// - Hides Ext. Price Total and Estimated Total
    /// - Renames Cost Total to Labour and Material Cost
    /// - Hides Budgeted Material Cost
    /// - Makes certain fields read-only
    /// </summary>
    public class FSServiceOrderExt : PXCacheExtension<FSServiceOrder>
    {
        // ------------------------------------------------------
        // Hide or rename standard fields
        // ------------------------------------------------------

        #region CuryExtPriceTotal
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = "Ext. Price Total", Visible = false)]
        public decimal? CuryEstimatedOrderTotal { get; set; }
        public abstract class curyExtPriceTotal : PX.Data.BQL.BqlDecimal.Field<curyExtPriceTotal> { }
        #endregion

        #region CuryEstimationTotal
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = "Estimated Total", Visible = false)]
        public virtual decimal? CuryDocTotal { get; set; }
        public abstract class curyEstimationTotal : PX.Data.BQL.BqlDecimal.Field<curyEstimationTotal> { }
        #endregion

        #region CuryCostTotal
        [PXMergeAttributes(Method = MergeMethod.Merge)]
        [PXUIField(DisplayName = "Labour and Material Cost", Enabled = false)]
        public virtual decimal? CuryCostTotal { get; set; }
        public abstract class curyCostTotal : PX.Data.BQL.BqlDecimal.Field<curyCostTotal> { }
        #endregion

        // ------------------------------------------------------
        // Your custom fields
        // ------------------------------------------------------

        #region UsrEstimatedMaterialCost
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Estimated Material Cost", Enabled = false)]
        public virtual decimal? UsrEstimatedMaterialCost { get; set; }
        public abstract class usrEstimatedMaterialCost : PX.Data.BQL.BqlDecimal.Field<usrEstimatedMaterialCost> { }
        #endregion

        #region UsrBudgetedMaterialCost
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Budgeted Material Cost", Visible = false)]
        public virtual decimal? UsrBudgetedMaterialCost { get; set; }
        public abstract class usrBudgetedMaterialCost : PX.Data.BQL.BqlDecimal.Field<usrBudgetedMaterialCost> { }
        #endregion

        #region UsrTotalMaterialCost
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Material Cost", Enabled = false)]
        public virtual decimal? UsrTotalMaterialCost { get; set; }
        public abstract class usrTotalMaterialCost : PX.Data.BQL.BqlDecimal.Field<usrTotalMaterialCost> { }
        #endregion

        #region UsrBudgetedLabourCost
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Budgeted Labour Cost", Enabled = false)]
        public virtual decimal? UsrBudgetedLabourCost { get; set; }
        public abstract class usrBudgetedLabourCost : PX.Data.BQL.BqlDecimal.Field<usrBudgetedLabourCost> { }
        #endregion

        #region UsrBudgetedLabourHours
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Budgeted Labour Hours", Enabled = false)]
        public virtual decimal? UsrBudgetedLabourHours { get; set; }
        public abstract class usrBudgetedLabourHours : PX.Data.BQL.BqlDecimal.Field<usrBudgetedLabourHours> { }
        #endregion

        #region UsrTotalLabourCost
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Labour Cost", Enabled = false)]
        public virtual decimal? UsrTotalLabourCost { get; set; }
        public abstract class usrTotalLabourCost : PX.Data.BQL.BqlDecimal.Field<usrTotalLabourCost> { }
        #endregion

        #region UsrTotalLabourHours
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Labour Hours", Enabled = false)]
        public virtual decimal? UsrTotalLabourHours { get; set; }
        public abstract class usrTotalLabourHours : PX.Data.BQL.BqlDecimal.Field<usrTotalLabourHours> { }
        #endregion

        #region UsrWorkOrderMargin
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Work Order Margin", Visible = false)]
        public virtual decimal? UsrWorkOrderMargin { get; set; }
        public abstract class usrWorkOrderMargin : PX.Data.BQL.BqlDecimal.Field<usrWorkOrderMargin> { }
        #endregion

        #region UsrToInvoice
        [PXDecimal(2)]
        [PXUIField(DisplayName = "To Invoice", Enabled = false)]
        public virtual decimal? UsrToInvoice { get; set; }
        public abstract class usrToInvoice : PX.Data.BQL.BqlDecimal.Field<usrToInvoice> { }
        #endregion
    }
}
