using CRLocation = PX.Objects.CR.Standalone.Location;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.CM.Extensions;
using PX.Objects.Common.Discount.Attributes;
using PX.Objects.Common.Discount;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.EP;
using PX.Objects.FS;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PM;
using PX.Objects.PO;
using PX.Objects.SO;
using PX.Objects.TX;
using PX.Objects;
using System.Collections.Generic;
using System;

namespace PX.Objects.FS
{
    public class FSSODetExt : PXCacheExtension<PX.Objects.FS.FSSODet>
    {
        #region UsrCustomField
        [PXString]
        [PXUIField(DisplayName="Custom Field")]
        public virtual string UsrCustomField { get; set; }
        public abstract class usrCustomField : PX.Data.BQL.BqlString.Field<usrCustomField> { }
        #endregion

        #region UsrBudgetedCost
        [PXDBDecimal(2)]
        [PXUIField(DisplayName="Budgeted Cost")]
        public virtual decimal? UsrBudgetedCost { get; set; }
        public abstract class usrBudgetedCost : PX.Data.BQL.BqlDecimal.Field<usrBudgetedCost> { }
        #endregion
    }
}
