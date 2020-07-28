using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.Reflection;
using System.Resources;


namespace ACERIFT
{
	/// <summary>
	/// Summary description for USER_DEF_CALCS.
	/// </summary>
	public class USER_DEF_CALCS: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			///Load the resource manager.
			ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);
			
			///This instantiates the Utility object.
			//PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();
			
			
			FormatCommands.LoadFormatDefaults(RG);

			///This is where we load the standard column headers.

			Utility.LoadColumnHeadingDefaults(RG);
			Utility.arrColHead.RemoveRange(1,1);
            ///CPF 05/22/07 SCR 7057:  When the periods header was being re-added, it was not calling the "NoAdjust" variation of 
            ///printordercalc.  This caused the period value to be incorrect for BTA and CustComp.
            ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalcNoAdjust(RG.STMT_PERIODS()), "", "");
			Utility.arrColHead.Insert(1, chead1);

			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "N/A");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
					
			///This creates the standard page header for the report.  
			Utility.CreatePageHeader(RG);

			///This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);
			bool hasItems = false;
			//ASK KZ if we need to check for the Print check ????
			foreach (Analysis a in RG.Context.Customer.Analyses)
			{
				if (a.Print == true)
				{
					hasItems = true;
					break;
				}
			}
			if(hasItems)
				Utility.PrintUDAs(RG);
			else
				Utility.PrintLabel(RG, rm.GetString("udaMsg"));
			Utility.UnderlinePage(RG, 2);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);
		}
	}
}
