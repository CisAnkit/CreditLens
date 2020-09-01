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
	/// Summary description for DET_INPUT_CF.
	/// </summary>
	public class DET_INPUT_CF: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
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

			int line406 = FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_470")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_493")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_475")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_494")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_471")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_472")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_408")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_473")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_474")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_405")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_406")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_407")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_476")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_477")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_478")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_479")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_480")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_481")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_482")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_483")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_484")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_485")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_486")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_487")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_488")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_489")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_490")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_491")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_409")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_492")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_410"));

			int line407 =	FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_414")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_415")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_416")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_417")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_418")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_419")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_420")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_421")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_424")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_425")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_426")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_427")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_428")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_429")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_430")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_431")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_432")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_433"));

			int line408 =	FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_437")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_438")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_439")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_440")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_441")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_442")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_443")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_444")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_445")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_446")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_447")) + FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_448")) +
							FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_449"));
			int line409 =	FormatCommands.DetailCount(RG, RG.GetDetailCalcs("F_453"));

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
			if (line406 > 0)
			{
				Utility.PrintLabel(RG, rm.GetString("operAct"));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			}
			printDetails( new int[]{470, 493, 475, 494, 471, 472, 408, 473, 474, 405, 476, 477, 480, 481, 482,
									  483, 487, 484, 485, 478, 479, 486, 488, 489, 490, 491, 406, 410, 409, 407, 492});
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (line406 > 0)
				Utility.UnderlineColumn(RG,1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("cshFrmOperAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmOprAct")));
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);


			if (line407 > 0)
				Utility.PrintLabel(RG, rm.GetString("invstAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

			printDetails( new int[]{414, 415, 416, 417, 418, 419, 420, 421, 422, 423, 424, 425, 426, 427, 428, 429,
									  430, 431, 432, 433});

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (line407 > 0)
				Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfFrmInvstAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmInvestAct")));
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (line408 > 0)
				Utility.PrintLabel(RG, rm.GetString("finAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printDetails( new int[]{437, 438, 439, 440, 441, 442, 443, 444, 445, 446, 447, 448, 449});
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			///CPF 07/27/06 Log 1691:  This line408 check was previously checking agains the wrong section (line407).
			if (line408 > 0)
				Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfFrmFinAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmFinAct")));
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			if (line409 > 0)
				Utility.PrintLabel(RG, rm.GetString("other"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintDetail(RG, RG.GetDetailCalcs("F_453"));
			//Utility.PrintLabel(RG, rm.GetString("other"));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totMoveInCash"), RG.GetPrintOrderCalc(RG.GetCalc("totMoveInCash")));

			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			Utility.PrintSummary(RG, rm.GetString("begPerCshEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("begPerCashEquiv")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.PrintSummary(RG, rm.GetString("impChgInExchRate"), RG.GetPrintOrderCalc(RG.GetCalc("ImpactOfChgInExchRates")));

			Utility.PrintSummary(RG, rm.GetString("unexpAdjCash"), RG.GetPrintOrderCalc(RG.GetCalc("unExplAdjToCash")));
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
