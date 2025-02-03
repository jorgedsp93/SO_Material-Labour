using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.FS;
using PX.Objects.IN;  // for InventoryItem
using System;

namespace PX.Objects.FS
{
    public class ServiceOrderEntry_Extension : PXGraphExtension<ServiceOrderEntry>
    {
        protected virtual void FSServiceOrder_RowSelected(
            PXCache cache,
            PXRowSelectedEventArgs e,
            PXRowSelected baseHandler)
        {
            // Let the base event run first
            baseHandler?.Invoke(cache, e);

            FSServiceOrder row = (FSServiceOrder)e.Row;
            if (row == null) return;

            FSServiceOrderExt rowExt = row.GetExtension<FSServiceOrderExt>();
            if (rowExt == null) return;

            //-------------------------------------------------------------
            // 1) Summation for "Estimated Material Cost" => sum of ALL
            //    FSSODet.UsrBudgetedCost lines (not just FLAT_RATE).
            //-------------------------------------------------------------
            decimal totalMaterialCostForEstimate = 0m;
            foreach (FSSODet det in Base.ServiceOrderDetails.Select())
            {
                FSSODetExt detExt = det.GetExtension<FSSODetExt>();
                decimal lineCost  = detExt?.UsrBudgetedCost ?? 0m;
                totalMaterialCostForEstimate += lineCost;
            }
            rowExt.UsrEstimatedMaterialCost = totalMaterialCostForEstimate;

            //-------------------------------------------------------------
            // 2) Summation of budgeted vs. actual for material lines & labour lines
            //    from FSSODet
            //-------------------------------------------------------------
            decimal totalMaterialCost    = 0m;
            decimal budgetedMaterialCost = 0m;
            decimal budgetedLabourCost   = 0m;
            decimal budgetedLabourHours  = 0m;
            decimal toInvoice            = 0m;

            foreach (FSSODet det in Base.ServiceOrderDetails.Select())
            {
                FSSODetExt detExt     = det.GetExtension<FSSODetExt>();
                decimal budCost       = detExt?.UsrBudgetedCost   ?? 0m;
                decimal actualCost    = det.CuryExtCost           ?? 0m;
                decimal lineBillable  = det.CuryBillableExtPrice  ?? 0m;

                bool isMaterial = (det.BillingRule == ID.BillingRule.NONE
                                || det.BillingRule == ID.BillingRule.FLAT_RATE);
                bool isLabour   = (det.BillingRule == ID.BillingRule.TIME);

                decimal estHours = det.EstimatedQty ?? 0m;

                if (isMaterial)
                {
                    totalMaterialCost    += actualCost;
                    budgetedMaterialCost += budCost;
                }
                else if (isLabour)
                {
                    budgetedLabourCost  += budCost;
                    budgetedLabourHours += estHours;
                }

                toInvoice += lineBillable;
            }

            // Store partial results from FSSODet
            rowExt.UsrTotalMaterialCost    = totalMaterialCost;
            rowExt.UsrBudgetedMaterialCost = budgetedMaterialCost;
            rowExt.UsrBudgetedLabourCost   = budgetedLabourCost;
            rowExt.UsrBudgetedLabourHours  = budgetedLabourHours;
            rowExt.UsrToInvoice            = toInvoice;

            //-------------------------------------------------------------
            // 3) Summation of ACTUAL labor from FSAppointmentLog
            //    We'll match logs by (docType == FSAppointment.srvOrdType)
            //    & (docRefNbr == FSAppointment.refNbr).
            //    Then join to InventoryItem to see if InventoryCD starts "SER "
            //-------------------------------------------------------------
            decimal totalLabourCost  = 0m;
            decimal totalLabourHours = 0m;

            var appts = SelectFrom<FSAppointment>
                .Where<FSAppointment.srvOrdType.IsEqual<FSServiceOrder.srvOrdType.FromCurrent>
                  .And<FSAppointment.soRefNbr.IsEqual<FSServiceOrder.refNbr.FromCurrent>>>
                .View
                .Select(Base);

            foreach (FSAppointment appt in appts)
            {
                var logs = SelectFrom<FSAppointmentLog>
                    .Where<FSAppointmentLog.docType.IsEqual<FSAppointment.srvOrdType.FromCurrent>
                      .And<FSAppointmentLog.docRefNbr.IsEqual<FSAppointment.refNbr.FromCurrent>>>
                    .View
                    .Select(Base);

                foreach (FSAppointmentLog logRow in logs)
                {
                    if (logRow.LaborItemID != null)
                    {
                        // Look up the InventoryItem
                        InventoryItem item = SelectFrom<InventoryItem>
                            .Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>
                            .View
                            .Select(Base, logRow.LaborItemID);

                        if (item?.InventoryCD != null &&
                            item.InventoryCD.StartsWith("SER ", StringComparison.OrdinalIgnoreCase))
                        {
                            decimal extCost  = logRow.CuryExtCost  ?? 0m;
                            decimal minutes  = logRow.TimeDuration ?? 0m;
                            decimal hours    = minutes / 60m;

                            totalLabourCost  += extCost;
                            totalLabourHours += hours;
                        }
                    }
                }
            }

            //-------------------------------------------------------------
            // 4) Calculate margin => (Revenue - Cost) / Revenue
            //-------------------------------------------------------------
            decimal totalCost       = totalMaterialCost + totalLabourCost;
            decimal workOrderMargin = 0m;
            if (toInvoice != 0m)
            {
                workOrderMargin = (toInvoice - totalCost) / toInvoice;
            }

            //-------------------------------------------------------------
            // 5) Store final results in FSServiceOrderExt
            //-------------------------------------------------------------
            rowExt.UsrTotalLabourCost  = totalLabourCost;
            rowExt.UsrTotalLabourHours = totalLabourHours;
            rowExt.UsrWorkOrderMargin  = workOrderMargin;
        }
    }
}
