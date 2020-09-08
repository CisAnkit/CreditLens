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
	/// Summary description for EXEC_SUMMARY.
	/// This report always needs to fit on one page.
	/// To do *** add the Accounting Standard to heading
	/// </summary>
	public class EXEC_SUMMARY: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.IS_Calcs(RG);
			Calcs.BS_Calcs(RG);
			Calcs.RatioCalcs(RG);
			Calcs.ExecSummary_Calcs(RG);
	    	Calcs.ReconCalcs(RG);
			// TODO: Add constructor logic here

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

			///CPF 08/14/06 Log 1758:  Removed "Reconcile To" to match design.
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

			// Start First Group (Income Statement Metrics)
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
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "True");
            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");

			Utility.Skip(RG, 1);

			/// Print the "KEY INCOME STATEMENT METRICS" header
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("esKeyISMetrics"));
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
		   /// Print the Income Statement section.

           Utility.PrintSummary(RG, rm.GetString("salesRevs"), RG.GetPrintOrderCalc(RG.GetCalc("SalesRevs")));

		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
		   RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
		   Utility.PrintSummary(RG, rm.GetString("salesRevGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("salesRevGrwth")));
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
		   RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
		   Utility.PrintSummary(RG, rm.GetString("grssPrft"), RG.GetPrintOrderCalc(RG.GetCalc("GrossPrft")));
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
           RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
		   Utility.PrintSummary(RG, rm.GetString("grssPrftMargin"), RG.GetPrintOrderCalc(RG.GetCalc("grssPrftMargin")));
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
           RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
		   Utility.PrintSummary(RG, rm.GetString("ebitda"), RG.GetPrintOrderCalc(RG.GetCalc("EBITDA")));

           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
		   Utility.PrintSummary(RG, rm.GetString("EBTIDAMargin"), RG.GetPrintOrderCalc(RG.GetCalc("EBTIDAMargin")));
		   Utility.PrintSummary(RG, rm.GetString("earnCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("earnCovrg")));
		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
	       Utility.PrintSummary(RG, rm.GetString("netOpPrftEBIT").ToUpper(), RG.GetPrintOrderCalc(RG.GetCalc("EBIT")));
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
		   Utility.PrintSummary(RG, rm.GetString("intCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("intCovrg")));
           RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
		   RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
		   RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
		   Utility.PrintSummary(RG, rm.GetString("netIntIncExp"), RG.GetPrintOrderCalc(RG.GetCalc("NetIntIncExp")));
		   Utility.PrintSummary(RG, rm.GetString("netOthFinIncExp"), RG.GetPrintOrderCalc(RG.GetCalc("NetOthFinIncExp")));
		   Utility.PrintSummary(RG, rm.GetString("othIncExp"), RG.GetPrintOrderCalc(RG.GetCalc("OthIncExp")));
		   Utility.PrintSummary(RG, rm.GetString("prftLossB4tax"), RG.GetPrintOrderCalc(RG.GetCalc("PrftLossB4Tax")));
	   	   Utility.PrintSummary(RG, rm.GetString("totIncTax"), RG.GetPrintOrderCalc(RG.GetCalc("TotIncTax")));
		   Utility.PrintSummary(RG, rm.GetString("prftLsB4ExOrdItem"), RG.GetPrintOrderCalc(RG.GetCalc("PrftLossB4ExtOrdItems")));
		   Utility.PrintSummary(RG, rm.GetString("netPrftLoss"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftLoss")));
		   Utility.PrintSummary(RG, rm.GetString("esDividends"), RG.GetPrintOrderCalc(RG.GetCalc("Dividends")));

		   Utility.Skip(RG, 1);

			/// Print the "RATIOS" header
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("civRatios"));
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");

			Utility.PrintSummary(RG, rm.GetString("workingCap"), RG.GetPrintOrderCalc(RG.GetCalc("workingCap")));

            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");

			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("currRatio"), RG.GetPrintOrderCalc(RG.GetCalc("currRatio")));
			Utility.PrintSummary(RG, rm.GetString("quickRatio"), RG.GetPrintOrderCalc(RG.GetCalc("quickRatio")));

	        RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			Utility.PrintSummary(RG, rm.GetString("TNW"), RG.GetPrintOrderCalc(RG.GetCalc("TNW")));

            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("dbtToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToTNW")));
			Utility.PrintSummary(RG, rm.GetString("brwFundToEBITDA"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundToEBITDA")));
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			Utility.PrintSummary(RG, rm.GetString("cfCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("cfCovrg")));
			Utility.PrintSummary(RG, rm.GetString("netTrdRecvDays"), RG.GetPrintOrderCalc(RG.GetCalc("netTrdRecvDays")));
			Utility.PrintSummary(RG, rm.GetString("invDays"), RG.GetPrintOrderCalc(RG.GetCalc("invDays")));
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			Utility.Skip(RG, 1);


			/// Print the "BALANCE SHEET DATA" header
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("civBSD"));
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			Utility.PrintSummary(RG, rm.GetString("netFixAssets"), RG.GetPrintOrderCalc(RG.GetCalc("NetFixAssts")));
			Utility.PrintSummary(RG, rm.GetString("netItangibles"), RG.GetPrintOrderCalc(RG.GetCalc("NetIntang")));
			Utility.PrintSummary(RG, rm.GetString("esOthNCAssts"), RG.GetPrintOrderCalc(RG.GetCalc("AllOthNCAstsBS")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totNCAssts"), RG.GetPrintOrderCalc(RG.GetCalc("TotNCAssts")));

			Utility.Skip(RG, 1);

			Utility.PrintSummary(RG, rm.GetString("civInvent"), RG.GetPrintOrderCalc(RG.GetCalc("InventoryBS")));
			Utility.PrintSummary(RG, rm.GetString("netTrdRecv"), RG.GetPrintOrderCalc(RG.GetCalc("NetTrRecBS")));
			Utility.PrintSummary(RG, rm.GetString("civCashEq"), RG.GetPrintOrderCalc(RG.GetCalc("CashEqBS")));
			Utility.PrintSummary(RG, rm.GetString("esOthCurrAssts"), RG.GetPrintOrderCalc(RG.GetCalc("AllOtherCurAstsBS")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totCurrAssts"), RG.GetPrintOrderCalc(RG.GetCalc("TotCurrAssts")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totAssts"), RG.GetPrintOrderCalc(RG.GetCalc("TotAssts")));
			Utility.Skip(RG, 1);
			Utility.PrintSummary(RG, rm.GetString("prmntEqty"), RG.GetPrintOrderCalc(RG.GetCalc("PrmntEquity")));
			Utility.PrintSummary(RG, rm.GetString("esRetProf"), RG.GetPrintOrderCalc(RG.GetCalc("RetProfit")));
     		Utility.PrintSummary(RG, rm.GetString("esOthEquity"), RG.GetPrintOrderCalc(RG.GetCalc("OthEquity")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totEqtyRsrvs"), RG.GetPrintOrderCalc(RG.GetCalc("actEndEqtyRsrvs")));

            Utility.Skip(RG, 1);

			Utility.PrintSummary(RG, rm.GetString("civLTD"), RG.GetPrintOrderCalc(RG.GetCalc("LTDBS")));
			Utility.PrintSummary(RG, rm.GetString("esOthLTLiabs"), RG.GetPrintOrderCalc(RG.GetCalc("OthLTLiabs")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totNCLiabs"), RG.GetPrintOrderCalc(RG.GetCalc("TotNCLiabs")));

			Utility.Skip(RG, 1);
			Utility.PrintSummary(RG, rm.GetString("esCPLTDST"), RG.GetPrintOrderCalc(RG.GetCalc("CPLTDSTLoans")));
			Utility.PrintSummary(RG, rm.GetString("civTrade_Pay"), RG.GetPrintOrderCalc(RG.GetCalc("TradePayCP")));

			Utility.PrintSummary(RG, rm.GetString("esOthCurrLiabs"), RG.GetPrintOrderCalc(RG.GetCalc("OthCurrLiabs")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totCurrLiabs"), RG.GetPrintOrderCalc(RG.GetCalc("TotCurrLiab")));

			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("totEqtyRsrvLiabs"), RG.GetPrintOrderCalc(RG.GetCalc("TotEqRsrvsLiab")));
            Utility.UnderlinePage(RG, 2);
			//End: Keep together for Full Report
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);



		}
	}
}
