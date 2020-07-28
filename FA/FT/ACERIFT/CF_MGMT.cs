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
	/// Summary description for CF_MGMT.
	/// </summary>
	public class CF_MGMT: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.IS_Calcs(RG);
			Calcs.RatioCalcs(RG);
			Calcs.CashFlowMgmt_Calcs(RG);

			///***CPF 11/6/02 Load the resource manager.
			ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);

			///***CPF 3/11/02 This instantiates the Utility object.
			PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();

			FormatCommands.LoadFormatDefaults(RG);

			///This is where we load the standard column headers.
			Utility.LoadColumnHeadingDefaults(RG);

			///CPF 08/14/06 Log 1757:  Replaced Months Covered with Periods and added Reconcile To.
			Utility.arrColHead.RemoveRange(1,1);
            ///CPF 05/22/07 SCR 7057:  When the periods header was being re-added, it was not calling the "NoAdjust" variation of
            ///printordercalc.  This caused the period value to be incorrect for BTA and CustComp.
            ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalcNoAdjust(RG.STMT_PERIODS()), "", "");
			Utility.arrColHead.Insert(1, chead1);

			ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
			Utility.arrColHead.Add(chead2);

			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_LABEL, "2.25");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_COLUMN_1, "1");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");

			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			///CPF 6/3/04 Log 713 Make zeros print - instead of 0.
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");

			///***CPF 3/11/02 This creates the standard page header for the report.  If
			///this as new report, make sure the NewReport parm is "True"
			Utility.CreatePageHeader(RG);

			///CPF 08/31/06 Log 1777:  Set cross foot on before stmt constants so that the
			///extra column is attached to the table.
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");

			///***CPF 3/11/02 This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);

			//Print Source and Target Currency
			Utility.PrintSourceTargetCurr(RG);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);

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
				Utility.PrintSummary(RG, rm.GetString("exchRatePE"), RG.GetPrintOrderCalc(RG.CONV_RATE_BS()));
				Utility.PrintSummary(RG, rm.GetString("exchRateAvg"), RG.GetPrintOrderCalc(RG.CONV_RATE_IS()));
			}
			else
				Utility.PrintSummary(RG, rm.GetString("exchRate"), RG.GetPrintOrderCalc(RG.CONV_RATE_BS()));
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			Utility.Skip(RG, 1);

			///CASH FLOW DRIVERS
			Utility.UnderlinePage(RG, 1);
			Utility.PrintCenter(RG, rm.GetString("cfmCashFlowDrivers"));
			Utility.UnderlinePage(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");

			Utility.PrintLabel(RG, rm.GetString("cfmCashFlowDrivers"));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("cfmNetSalesGwth"), RG.GetPrintOrderCalc(RG.GetCalc("salesRevGrwth")));

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");

			Utility.PrintSummary(RG, rm.GetString("cfmGrsMgnPlsDepr"), RG.GetPrintOrderCalc(RG.GetCalc("cshGrssPrftMargin")));
			Utility.PrintSummary(RG, rm.GetString("cfmOpExpExclDepr"), RG.GetPrintOrderCalc(RG.GetCalc("cfmOperExpenses")));

			Utility.PrintSummary(RG, rm.GetString("netTrdRecvDays"), RG.GetPrintOrderCalc(RG.GetCalc("netTrdRecvDays")));
			Utility.PrintSummary(RG, rm.GetString("invDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("invDaysExclCOS")));
			Utility.PrintSummary(RG, rm.GetString("trdPayDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("trdPayDaysExclCOS")));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintLabel(RG, rm.GetString("cfmOthFact"));
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");

			Utility.PrintSummary(RG, rm.GetString("cfmOthOpAsts"), RG.GetPrintOrderCalc(RG.GetCalc("cfmOthOpAsts")));
			Utility.PrintSummary(RG, rm.GetString("cfmAccruals"), RG.GetPrintOrderCalc(RG.GetCalc("cfmAccruals")));

			Utility.UnderlinePage(RG, 1);

			///CASH MARGIN MANAGEMENT SUMMARY
			Utility.PrintCenter(RG, rm.GetString("cfmCashMgnMgmtSum"));
			Utility.UnderlinePage(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintLabel(RG, rm.GetString("cfmCashGrsPft"));

			Utility.PrintSummary(RG, rm.GetString("cfmBegCashGrsPft"), RG.GetPrintOrderCalc(RG.GetCalc("cfmBegCashGrsPft")));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("cfmCashGrsPftMgmt"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCashGrsPftMgmt")));
			Utility.PrintSummary(RG, rm.GetString("cfmCashGrsPftGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCashGrsPftGrwth")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

			Utility.PrintSummary(RG, rm.GetString("cfmEndCashGrsPft"), RG.GetPrintOrderCalc(RG.GetCalc("cshGrossPrft")));

			Utility.Skip(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintLabel(RG, rm.GetString("cfmOperatingExpenses"));

			Utility.PrintSummary(RG, rm.GetString("cfmBegOpExp"), -1 * RG.GetPrintOrderCalc(RG.GetCalc("cfmBegOpExp")));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("cfmOpExpMgmt"), -1 * RG.GetPrintOrderCalc(RG.GetCalc("cfmOpExpMgmt")));
			Utility.PrintSummary(RG, rm.GetString("cfmOpExpGwth"), -1 * RG.GetPrintOrderCalc(RG.GetCalc("cfmOpExpGwth")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");

			Utility.PrintSummary(RG, rm.GetString("cfmEndOpExp"), -1 * RG.GetPrintOrderCalc(RG.GetCalc("OperExpExclDeprAmrt")));

			Utility.Skip(RG, 1);

			Utility.UnderlineColumn(RG, 1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfmCashOpProfit"), RG.GetPrintOrderCalc(RG.GetCalc("cshGrossPrft")) + (-1 * RG.GetPrintOrderCalc(RG.GetCalc("OperExpExclDeprAmrt"))));

			///TRADING ACCOUNT MANAGEMENT SUMMARY
			Utility.UnderlinePage(RG, 1);
			Utility.PrintCenter(RG, rm.GetString("cfmTrdAcctMgmtSum"));
			Utility.UnderlinePage(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			///CASH IMPACT OF MGMT
			Utility.PrintLabel(RG, rm.GetString("cfmCashImpMgmt"));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("netTrdRecv"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIMNetTrdRecv")));
			Utility.PrintSummary(RG, rm.GetString("civInvent"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIMInventories")));
			Utility.PrintSummary(RG, rm.GetString("civTrade_Pay"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIMTrdPayCP")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
			Utility.PrintSummary(RG, rm.GetString("cfmTotMgmtTrdAcct"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotMgmtTrdAccts")));

			Utility.Skip(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("cfmOthOpAsts"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIMOthOpAssets")));
			Utility.PrintSummary(RG, rm.GetString("cfmAccruals"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIMAccruals")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
			Utility.PrintSummary(RG, rm.GetString("cfmTotMgmtOthFact"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotMgmtOthFactors")));

			Utility.Skip(RG, 1);

			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("cfmTotCashImpMgmt"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotCashImpMgmt")));

			Utility.Skip(RG, 1);

			///CASH IMPACT OF GROWTH
			Utility.PrintLabel(RG, rm.GetString("cfmCashImpGwth"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

			Utility.PrintSummary(RG, rm.GetString("netTrdRecv"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIGNetTrdRecv")));
			Utility.PrintSummary(RG, rm.GetString("civInvent"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIGInventories")));
			Utility.PrintSummary(RG, rm.GetString("civTrade_Pay"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIGTrdPayCP")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
			Utility.PrintSummary(RG, rm.GetString("cfmTotGwthTrdAcct"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotGwthTrdAccts")));

			Utility.Skip(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("cfmOthOpAsts"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIGOthOpAssets")));
			Utility.PrintSummary(RG, rm.GetString("cfmAccruals"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCIGAccruals")));
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
			Utility.PrintSummary(RG, rm.GetString("cfmTotGwthOthFact"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotGwthOthFactors")));

			Utility.Skip(RG, 1);

			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("cfmTotCashImpSalesGwth"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotCashImpSalesGwth")));

			Utility.Skip(RG, 1);

			Utility.UnderlineColumn(RG, 1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfmTotTrdAcct"), RG.GetPrintOrderCalc(RG.GetCalc("cfmTotTrdActOthFactChg")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintLabel(RG, rm.GetString("cfmOthFactChg"));

			Utility.Skip(RG, 1);

			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("ucaCashAftOp"), RG.GetPrintOrderCalc(RG.GetCalc("cfmCashAftOps")));

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			//End: Keep together for Full Report
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);
		}
	}
}
