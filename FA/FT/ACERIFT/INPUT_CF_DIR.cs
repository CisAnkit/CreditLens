using System.Collections;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;
using System;
using FinancialAnalyst.DataManager;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for INPUT_CF_DIR.
	/// </summary>
	public class INPUT_CF_DIR: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.DtInputCFDir_Calcs(RG);
			Calcs.DtInputCF_Calcs(RG);

			///Load the resource manager.
			ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);

			///This instantiates the Utility object.
			//PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();

			FormatCommands.LoadFormatDefaults(RG);

			///This is where we load the standard column headers.

			Utility.LoadColumnHeadingDefaults(RG);

			Utility.arrColHead.RemoveRange(1,1);
			ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalc(RG.STMT_PERIODS()), "", "");
			Utility.arrColHead.Insert(1, chead1);

			///CPF 08/15/06 Log 1761:  Removed "Reconcile To" to match design doc.
//			ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
//			Utility.arrColHead.Add(chead2);

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_LABEL, "2.75");
			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			//RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
			RG.SetAuthorSetting(FORMATCOMMANDS.SPACING_COLUMNS, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");  // Must set to true before calling PrintStmtConstRows

			///This creates the standard page header for the report.
			Utility.CreatePageHeader(RG);

			///This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);
			//Print Source and Target Currency
			Utility.PrintSourceTargetCurr(RG);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);
			int OperAct = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_600")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_601")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_602")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_603")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_604"));

			int InvestAct = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_414")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_415")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_416")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_417")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_418")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_419")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_420")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_421")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_422")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_423")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_424")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_425")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_426")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_427")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_428")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_429")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_430")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_431")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_432")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_433"));

			int FinanceAct =	FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_437")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_438")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_439")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_440")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_441")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_442")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_443")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_444")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_445")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_446")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_447")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_448")) +
				FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_449"));

				int ForeignExch =FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_453"));

			Utility.mT.AddStartRow(Utility.nRow + 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "5");
            ///CPF 02/01/06 Log 1603:  Before printing specific "End" and "Average" rates, check to see if we are using acct
            ///rates.  The only circumstances under which we print the End/Avg rates are:  1.  Using grid rates 2. Using Exch DB AND both end/avg are required.
            bool useAcctRate = OrgPropertyDataManager.GetRptsUseAcctRate();
            eDualConversion dualConversion = OrgPropertyDataManager.GetDualConversion();
            if (useAcctRate || (!useAcctRate) && (dualConversion == eDualConversion.On))
            {
				//amit 07/27/06 log 1595: Only show the period average rate for this report
				//Utility.PrintSummary(RG, rm.GetString("exchRatePE"), RG.GetPrintOrderCalc(RG.CONV_RATE_BS()));
				Utility.PrintSummary(RG, rm.GetString("exchRateAvg"), RG.GetPrintOrderCalc(RG.CONV_RATE_IS()));
			}
			else
				Utility.PrintSummary(RG, rm.GetString("exchRate"), RG.GetPrintOrderCalc(RG.CONV_RATE_BS()));
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");
            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			Utility.Skip(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			if (OperAct > 0)
			{
				Utility.PrintLabel(RG, rm.GetString("operAct"));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			}
			printDetails( new int[]{600, 601, 602, 603, 604});
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (OperAct > 0)
				Utility.UnderlineColumn(RG,1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("cshFrmOperAct"),RG.GetPrintOrderCalc(RG.GetCalc("cfFrmOprActDir")));

			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (InvestAct > 0)
				Utility.PrintLabel(RG, rm.GetString("invstAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

			printDetails( new int[]{414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429,
									   430, 431, 432, 433});


			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (InvestAct > 0)
				Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfFrmInvstAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmInvestAct")));
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (FinanceAct > 0)
				Utility.PrintLabel(RG, rm.GetString("finAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printDetails( new int[]{437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449});
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (FinanceAct > 0)
				Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfFrmFinAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmFinAct")));
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			if (ForeignExch > 0)
				Utility.PrintLabel(RG, rm.GetString("other"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintDetail(RG, RG.GetDetailCalcs("F_453"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totMoveInCash"), RG.GetPrintOrderCalc(RG.GetCalc("TotMoveCashDir")));
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			Utility.PrintSummary(RG, rm.GetString("begPerCshEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("begPerCashEquiv")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.PrintSummary(RG, rm.GetString("impChgInExchRate"), RG.GetPrintOrderCalc(RG.GetCalc("ImpactOfChgInExchRatesDir")));

			Utility.PrintSummary(RG, rm.GetString("unexpAdjCash"), RG.GetPrintOrderCalc(RG.GetCalc("unExplAdjToCashDir")));
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("endOfPerCshEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("endPerCashEquiv")));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
			///ExchRateWarnMsg.printWarning(RG, Utility, rm);
			Utility.UnderlinePage(RG, 2);
			Utility.mT.AddEndRow(Utility.nRow);


			//End: Keep together for Full Report
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);
		}

			private void printDetails(int[] ids)
			{
				string label = "";

				for (int i=0; i< ids.Length; i++)
				{
					label = "F_" + ids[i].ToString();
					Utility.PrintDetail(RG, RG.GetDetailCalcs(label));
				}



			}

	}
}



