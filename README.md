THIS REPOSITORY IS NOT AVAILABLE FOR COMMERCIAL USE

This project adds new fields and calculations to Acumatica Field Service for Service Orders. It helps you:

Calculate Budgeted and Actual costs for Materials and Labor
Hide or rename certain built-in fields (like “Ext. Price Total”)
Make some fields read-only, so you can’t edit them by mistake
Files in This Project
FSSODetExt.cs

Adds a “Budgeted Cost” field to each Service Order line.
FSServiceOrderExt.cs

Hides fields you don’t need (e.g., “Ext. Price Total”)
Renames “Cost Total” to “Labour and Material Cost”
Adds and locks down fields like “Estimated Material Cost” or “Total Labour Cost”
ServiceOrderEntry_Extension.cs

Actually does the math:
Sums up budgeted material costs
Finds labor logs linked to each Service Order and calculates actual labor cost and hours
Stores the final totals in the fields we added in FSServiceOrderExt.cs
Why Use This?
Stop messing up data: any cost fields are read-only.
See real labor and material costs in one place.
Hide fields you don’t use.
How to Add This to Acumatica
Download or Clone this project from GitHub.
Log into Acumatica and go to Customization → Customization Projects.
Create a new project (or open an existing one).
Add each .cs file:
FSSODetExt.cs
FSServiceOrderExt.cs
ServiceOrderEntry_Extension.cs
Check that each file is in namespace PX.Objects.FS.
Publish the project.
Using It
Go to Field Service → Service Orders (screen FS300100).
Open any Service Order.
Check the Totals section (or your custom panel) to see:
Estimated Material Cost (sum of BudgetedCost)
Total Material Cost (actual cost from lines)
Total Labour Cost (actual cost from appointment logs)
Margin (if you want it)
Fields like Ext. Price Total or Estimated Total should be hidden.
Renamed “Cost Total” → “Labour and Material Cost.”
Common Questions
I don’t see my labor cost
Make sure each labor line in FSAppointmentLog uses an InventoryItem whose code starts with **SER **.
I still see “Ext. Price Total”
Ensure you have merged attributes in FSServiceOrderExt.cs and set Visible = false.
Want to Change More?
Hide or rename additional fields by editing FSServiceOrderExt.cs.
Change the logic for labor items in ServiceOrderEntry_Extension.cs (e.g., use a different InventoryCD prefix).
