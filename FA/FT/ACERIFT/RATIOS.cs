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
	/// Summary description for RATIOS.
	/// </summary>
	public class RATIOS
	{
		static public void ratios_report(ReportGenerator RG, bool isPeer)
		{
			CALCULATIONS Calcs = new CALCULATIONS(RG);


			Calcs.RatioCalcs(RG);

			///Load the resource manager.
			ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);

			///This instantiates the Utility object.
			PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();

			FormatCommands.LoadFormatDefaults(RG);

			///This is where we load the standard column headers.
			Utility.LoadColumnHeadingDefaults(RG);

			Utility.arrColHead.RemoveRange(1,1);
            ///CPF 05/22/07 SCR 7057:  When the periods header was being re-added, it was not calling the "NoAdjust" variation of
            ///printordercalc.  This caused the period value to be incorrect for BTA and CustComp.
            ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalcNoAdjust(RG.STMT_PERIODS()), "", "");
			Utility.arrColHead.Insert(1, chead1);

            ///CPF 06/11/07 SCR 1968:  If BTA do not show Reconcile To.
            if (RG.IsBudgetToActual == false)
            {
                ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
                Utility.arrColHead.Add(chead2);
            }

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");

			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");

			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "N/A");

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");

			///This creates the standard page header for the report.
			Utility.CreatePageHeader(RG);

			///This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);
			//Print Source and Target Currency
			Utility.PrintSourceTargetCurr(RG);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);
			//  Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

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
			Utility.Skip(RG, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("liquidity"));
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");

			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			Utility.PrintSummary(RG, rm.GetString("workingCap"), RG.GetPrintOrderCalc(RG.GetCalc("workingCap")));
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("currRatio"), RG.GetPrintOrderCalc(RG.GetCalc("currRatio")));
			Utility.PrintSummary(RG, rm.GetString("quickRatio"), RG.GetPrintOrderCalc(RG.GetCalc("quickRatio")));
			Utility.PrintSummary(RG, rm.GetString("slsRevToWC"), RG.GetPrintOrderCalc(RG.GetCalc("slsRevToWC")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("cashRatio"), RG.GetPrintOrderCalc(RG.GetCalc("cashRatio")));
            Utility.Skip(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("levarage"));
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			Utility.PrintSummary(RG, rm.GetString("TNW"), RG.GetPrintOrderCalc(RG.GetCalc("TNW")));
			Utility.PrintSummary(RG, rm.GetString("effTNW"), RG.GetPrintOrderCalc(RG.GetCalc("effTNW")));
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("dbtToNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToNW")));
			Utility.PrintSummary(RG, rm.GetString("dbtToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToTNW")));
			Utility.PrintSummary(RG, rm.GetString("dbttLessSubDbtEffTNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbttLessSubDbtEffTNW")));
			Utility.PrintSummary(RG, rm.GetString("brwFundToTotLiab"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundToTotLiab")));
			Utility.PrintSummary(RG, rm.GetString("brwFundtoEffTNW"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundtoEffTNW")));
			Utility.PrintSummary(RG, rm.GetString("brwFundToEBITDA"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundToEBITDA")));
			Utility.PrintSummary(RG, rm.GetString("TotLiabToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("TotLiabToTotAssts")));
			Utility.PrintSummary(RG, rm.GetString("offBSLevrg"), RG.GetPrintOrderCalc(RG.GetCalc("offBSLevrg")));
            //hongzhou 2010-5-10, add Debt To Book Capital
            Utility.PrintSummary(RG, rm.GetString("DebtToBookCapital"), RG.GetPrintOrderCalc(RG.GetCalc("DebtToBookCapital")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("dbtToAssts"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToAssts")));
            Utility.PrintSummary(RG, rm.GetString("STDbtToTotDbt"), RG.GetPrintOrderCalc(RG.GetCalc("STDbtToTotDbt")));
            Utility.PrintSummary(RG, rm.GetString("STDbtToWC"), RG.GetPrintOrderCalc(RG.GetCalc("STDbtToWC")));
            Utility.PrintSummary(RG, rm.GetString("netDbtToEBIT"), RG.GetPrintOrderCalc(RG.GetCalc("netDbtToEBIT")));
            Utility.Skip(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			Utility.PrintLabel(RG, rm.GetString("coverage"));
			Utility.PrintSummary(RG, rm.GetString("intCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("intCovrg")));

			Utility.PrintSummary(RG, rm.GetString("earnCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("earnCovrg")));

			//** THIS NEEDS TESTING
			if ((RG.GetCalc("icfCFOpAct")[0] == 0) && (RG.GetCalc("dcfCFOpAct")[0] == 0))
				RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			if ((RG.ISCONSOLIDATION == 0)&& (RG.IsBudgetToActual == false))
				Utility.PrintSummary(RG, rm.GetString("cfCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("cfCovrg")));

            if (RG.IsBudgetToActual == false)
            {
                ///CPF 06/11/07 SCR 1967:  Omit the first column values for the cash coverage ratios.
                RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
                Utility.PrintSummary(RG, rm.GetString("ucaCFCovCY"), RG.GetPrintOrderCalc(RG.GetCalc("UCACFCoverageCY")));
                Utility.PrintSummary(RG, rm.GetString("ucaCFCovPY"), RG.GetPrintOrderCalc(RG.GetCalc("UCACFCoveragePY")));
            }
            //hongzhou 2010-5-10, add Cash From Operations to Debt
            Utility.PrintSummary(RG, rm.GetString("CashFromOperationsToDebt"), RG.GetPrintOrderCalc(RG.GetCalc("CashFromOperationsToDebt")));
            RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");

			Utility.Skip(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("profitability"));
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			Utility.PrintSummary(RG, rm.GetString("grssPrftMargin"), RG.GetPrintOrderCalc(RG.GetCalc("grssPrftMargin")));
			Utility.PrintSummary(RG, rm.GetString("cshGrssPrftMargin"), RG.GetPrintOrderCalc(RG.GetCalc("cshGrssPrftMargin")));
			Utility.PrintSummary(RG, rm.GetString("EBTIDAMargin"), RG.GetPrintOrderCalc(RG.GetCalc("EBTIDAMargin")));
			Utility.PrintSummary(RG, rm.GetString("netOpEBITMrg"), RG.GetPrintOrderCalc(RG.GetCalc("netOpMargin")));
			Utility.PrintSummary(RG, rm.GetString("PrftB4TaxMargin"), RG.GetPrintOrderCalc(RG.GetCalc("PrftB4TaxMargin")));
			Utility.PrintSummary(RG, rm.GetString("NetPrftMarginPer"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftMargin")));
			Utility.PrintSummary(RG, rm.GetString("divdPayRate"), RG.GetPrintOrderCalc(RG.GetCalc("divdPayRate")));

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			if ((RG.ISCONSOLIDATION == 0) && (RG.IsBudgetToActual == false))
				Utility.PrintSummary(RG, rm.GetString("effTaxRate"), RG.GetPrintOrderCalc(RG.GetCalc("effTaxRate")));
			///RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");

			Utility.PrintSummary(RG, rm.GetString("PBTToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTotAssts")));
			Utility.PrintSummary(RG, rm.GetString("PBTToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTNW")));
			Utility.PrintSummary(RG, rm.GetString("PBTToTotEqtyResrvs"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTotEqtyResrvs")));
			Utility.PrintSummary(RG, rm.GetString("ROA"), RG.GetPrintOrderCalc(RG.GetCalc("ROA")));
			Utility.PrintSummary(RG, rm.GetString("ROTNW"), RG.GetPrintOrderCalc(RG.GetCalc("ROTNW")));
			Utility.PrintSummary(RG, rm.GetString("ROTotEqtyResrvs"), RG.GetPrintOrderCalc(RG.GetCalc("ROTotEqtyResrvs")));
			Utility.Skip(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			Utility.PrintLabel(RG, rm.GetString("activity"));
			//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			Utility.PrintSummary(RG, rm.GetString("netTrdRecvDays"), RG.GetPrintOrderCalc(RG.GetCalc("netTrdRecvDays")));
			Utility.PrintSummary(RG, rm.GetString("invDays"), RG.GetPrintOrderCalc(RG.GetCalc("invDays")));
			Utility.PrintSummary(RG, rm.GetString("invDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("invDaysExclCOS")));
			Utility.PrintSummary(RG, rm.GetString("trdPayDays"), RG.GetPrintOrderCalc(RG.GetCalc("trdPayDays")));
			Utility.PrintSummary(RG, rm.GetString("trdPayDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("trdPayDaysExclCOS")));
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("salesToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("salesToTotAssts")));
			Utility.PrintSummary(RG, rm.GetString("salesToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("salesToTNW")));
			Utility.PrintSummary(RG, rm.GetString("salesToNFA"), RG.GetPrintOrderCalc(RG.GetCalc("salesToNFA")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("cashConvCyc"), RG.GetPrintOrderCalc(RG.GetCalc("cashConvCyc")));
            Utility.PrintSummary(RG, rm.GetString("invToWC"), RG.GetPrintOrderCalc(RG.GetCalc("invToWC")));
            //hongzhou 2010-5-10, add Cash to Net Sales, Log 10 (Net Sales)
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "4");
            Utility.PrintSummary(RG, rm.GetString("CashToNetSales"), RG.GetPrintOrderCalc(RG.GetCalc("CashToNetSales")));
            Utility.PrintSummary(RG, rm.GetString("Log10NetSales"), RG.GetPrintOrderCalc(RG.GetCalc("Log10NetSales")));

            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");

			Utility.Skip(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);
			if ((RG.ISCONSOLIDATION == 0) && ( RG.IsBudgetToActual == false))
			{
				//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
				Utility.PrintLabel(RG, rm.GetString("growth"));
				//RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
				RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
				RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");

				Utility.PrintSummary(RG, rm.GetString("totAsstsGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("totAsstsGrwth")));
				Utility.PrintSummary(RG, rm.GetString("salesRevGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("salesRevGrwth")));
				Utility.PrintSummary(RG, rm.GetString("EBITDAGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("EBITDAGrwth")));
				Utility.PrintSummary(RG, rm.GetString("netOpPrftGrw"), RG.GetPrintOrderCalc(RG.GetCalc("netOpPrftGrw")));

				Utility.PrintSummary(RG, rm.GetString("netPrftGrwth"), RG.GetPrintOrderCalc(RG.GetCalc("netPrftGrwth")));
			}
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
			Utility.PrintSummary(RG, rm.GetString("sustainGrowth"), RG.GetPrintOrderCalc(RG.GetCalc("sustainGrowth")));
			///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
			///ExchRateWarnMsg.printWarning(RG, Utility, rm);
            Utility.Skip(RG, 1);
            Utility.PrintUDAs(RG);

			Utility.UnderlinePage(RG, 2);
			Utility.mT.AddEndRow(Utility.nRow);

			//End: Keep together for Full Report
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);
		}

        static public void ratios_entcomp_report(ReportGenerator RG, bool isPeer)
        {
            CALCULATIONS Calcs = new CALCULATIONS(RG);


            Calcs.RatioCalcs(RG);

            ///Load the resource manager.
            ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);

            ///This instantiates the Utility object.
            PRINTCOMMANDS Utility = new PRINTCOMMANDS();
            FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();

            FormatCommands.LoadFormatDefaults(RG);

            ///This is where we load the standard column headers.
            Utility.LoadColumnHeadingDefaults(RG);

            Utility.arrColHead.RemoveRange(1, 1);
            ///CPF 05/22/07 SCR 7057:  When the periods header was being re-added, it was not calling the "NoAdjust" variation of
            ///printordercalc.  This caused the period value to be incorrect for BTA and CustComp.
            ColumnHeader chead1 = new ColumnHeader(rm.GetString("periods"), RG.GetPrintOrderCalcNoAdjust(RG.STMT_PERIODS()), "", "");
            Utility.arrColHead.Insert(1, chead1);

            ///CPF 06/11/07 SCR 1968:  If BTA do not show Reconcile To.
            if (RG.IsBudgetToActual == false)
            {
                ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
                Utility.arrColHead.Add(chead2);
            }

            RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");

            RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");

            RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
            RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "N/A");

            RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");

            ///This creates the standard page header for the report.
            Utility.CreatePageHeader(RG);

            ///This prints the statement constant rows
            Utility.PrintStmtConstRows(RG, 1);
            //Print Source and Target Currency
            Utility.PrintSourceTargetCurr(RG);

            //  Start of the outer Group (Full Report)
            Utility.mT.AddStartRow(Utility.nRow + 1);
            //  Start
            Utility.mT.AddStartRow(Utility.nRow + 1);

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
            Utility.Skip(RG, 1);

            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
            Utility.PrintLabel(RG, rm.GetString("liquidity"));
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");

            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
            Utility.PrintSummary(RG, rm.GetString("workingCap"), RG.GetPrintOrderCalc(RG.GetCalc("workingCap")));
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
            Utility.PrintSummary(RG, rm.GetString("currRatio"), RG.GetPrintOrderCalc(RG.GetCalc("currRatio")));
            Utility.PrintSummary(RG, rm.GetString("quickRatio"), RG.GetPrintOrderCalc(RG.GetCalc("quickRatio")));
            Utility.PrintSummary(RG, rm.GetString("slsRevToWC"), RG.GetPrintOrderCalc(RG.GetCalc("slsRevToWC")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("cashRatio"), RG.GetPrintOrderCalc(RG.GetCalc("cashRatio")));
            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);

            Utility.mT.AddStartRow(Utility.nRow + 1);
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
            Utility.PrintLabel(RG, rm.GetString("levarage"));
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
            Utility.PrintSummary(RG, rm.GetString("TNW"), RG.GetPrintOrderCalc(RG.GetCalc("TNW")));
            Utility.PrintSummary(RG, rm.GetString("effTNW"), RG.GetPrintOrderCalc(RG.GetCalc("effTNW")));
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
            Utility.PrintSummary(RG, rm.GetString("dbtToNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToNW")));
            Utility.PrintSummary(RG, rm.GetString("dbtToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToTNW")));
            Utility.PrintSummary(RG, rm.GetString("dbttLessSubDbtEffTNW"), RG.GetPrintOrderCalc(RG.GetCalc("dbttLessSubDbtEffTNW")));
            Utility.PrintSummary(RG, rm.GetString("brwFundToTotLiab"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundToTotLiab")));
            Utility.PrintSummary(RG, rm.GetString("brwFundtoEffTNW"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundtoEffTNW")));
            Utility.PrintSummary(RG, rm.GetString("brwFundToEBITDA"), RG.GetPrintOrderCalc(RG.GetCalc("brwFundToEBITDA")));
            Utility.PrintSummary(RG, rm.GetString("TotLiabToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("TotLiabToTotAssts")));
            Utility.PrintSummary(RG, rm.GetString("offBSLevrg"), RG.GetPrintOrderCalc(RG.GetCalc("offBSLevrg")));
            //hongzhou 2010-5-10, add Debt To Book Capital
            Utility.PrintSummary(RG, rm.GetString("DebtToBookCapital"), RG.GetPrintOrderCalc(RG.GetCalc("DebtToBookCapital")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("dbtToAssts"), RG.GetPrintOrderCalc(RG.GetCalc("dbtToAssts")));
            Utility.PrintSummary(RG, rm.GetString("STDbtToTotDbt"), RG.GetPrintOrderCalc(RG.GetCalc("STDbtToTotDbt")));
            Utility.PrintSummary(RG, rm.GetString("STDbtToWC"), RG.GetPrintOrderCalc(RG.GetCalc("STDbtToWC")));
            Utility.PrintSummary(RG, rm.GetString("netDbtToEBIT"), RG.GetPrintOrderCalc(RG.GetCalc("netDbtToEBIT")));
            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);

            Utility.mT.AddStartRow(Utility.nRow + 1);
            Utility.PrintLabel(RG, rm.GetString("coverage"));
            Utility.PrintSummary(RG, rm.GetString("intCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("intCovrg")));
            Utility.PrintSummary(RG, rm.GetString("earnCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("earnCovrg")));

            //** THIS NEEDS TESTING
            if (!((RG.GetCalc("icfCFOpAct")[0] == 0) && (RG.GetCalc("dcfCFOpAct")[0] == 0)))
            {
                if ((RG.ISCONSOLIDATION == 0) && (RG.IsBudgetToActual == false))
                    Utility.PrintSummary(RG, rm.GetString("cfCovrg"), RG.GetPrintOrderCalc(RG.GetCalc("cfCovrg")));
            }

            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");

            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);

            Utility.mT.AddStartRow(Utility.nRow + 1);
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
            Utility.PrintLabel(RG, rm.GetString("profitability"));
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
            Utility.PrintSummary(RG, rm.GetString("grssPrftMargin"), RG.GetPrintOrderCalc(RG.GetCalc("grssPrftMargin")));
            Utility.PrintSummary(RG, rm.GetString("cshGrssPrftMargin"), RG.GetPrintOrderCalc(RG.GetCalc("cshGrssPrftMargin")));
            Utility.PrintSummary(RG, rm.GetString("EBTIDAMargin"), RG.GetPrintOrderCalc(RG.GetCalc("EBTIDAMargin")));
            Utility.PrintSummary(RG, rm.GetString("netOpEBITMrg"), RG.GetPrintOrderCalc(RG.GetCalc("netOpMargin")));
            Utility.PrintSummary(RG, rm.GetString("PrftB4TaxMargin"), RG.GetPrintOrderCalc(RG.GetCalc("PrftB4TaxMargin")));
            Utility.PrintSummary(RG, rm.GetString("NetPrftMarginPer"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftMargin")));
            Utility.PrintSummary(RG, rm.GetString("divdPayRate"), RG.GetPrintOrderCalc(RG.GetCalc("divdPayRate")));

            Utility.PrintSummary(RG, rm.GetString("PBTToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTotAssts")));
            Utility.PrintSummary(RG, rm.GetString("PBTToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTNW")));
            Utility.PrintSummary(RG, rm.GetString("PBTToTotEqtyResrvs"), RG.GetPrintOrderCalc(RG.GetCalc("PBTToTotEqtyResrvs")));
            Utility.PrintSummary(RG, rm.GetString("ROA"), RG.GetPrintOrderCalc(RG.GetCalc("ROA")));
            Utility.PrintSummary(RG, rm.GetString("ROTNW"), RG.GetPrintOrderCalc(RG.GetCalc("ROTNW")));
            Utility.PrintSummary(RG, rm.GetString("ROTotEqtyResrvs"), RG.GetPrintOrderCalc(RG.GetCalc("ROTotEqtyResrvs")));
            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);

            Utility.mT.AddStartRow(Utility.nRow + 1);
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
            Utility.PrintLabel(RG, rm.GetString("activity"));
            //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            Utility.PrintSummary(RG, rm.GetString("netTrdRecvDays"), RG.GetPrintOrderCalc(RG.GetCalc("netTrdRecvDays")));
            Utility.PrintSummary(RG, rm.GetString("invDays"), RG.GetPrintOrderCalc(RG.GetCalc("invDays")));
            Utility.PrintSummary(RG, rm.GetString("invDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("invDaysExclCOS")));
            Utility.PrintSummary(RG, rm.GetString("trdPayDays"), RG.GetPrintOrderCalc(RG.GetCalc("trdPayDays")));
            Utility.PrintSummary(RG, rm.GetString("trdPayDaysExclCOS"), RG.GetPrintOrderCalc(RG.GetCalc("trdPayDaysExclCOS")));
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
            Utility.PrintSummary(RG, rm.GetString("salesToTotAssts"), RG.GetPrintOrderCalc(RG.GetCalc("salesToTotAssts")));
            Utility.PrintSummary(RG, rm.GetString("salesToTNW"), RG.GetPrintOrderCalc(RG.GetCalc("salesToTNW")));
            Utility.PrintSummary(RG, rm.GetString("salesToNFA"), RG.GetPrintOrderCalc(RG.GetCalc("salesToNFA")));
            //aveys 2020-5-27, add ACERIFT custom macros
            Utility.PrintSummary(RG, rm.GetString("cashConvCyc"), RG.GetPrintOrderCalc(RG.GetCalc("cashConvCyc")));
            Utility.PrintSummary(RG, rm.GetString("invToWC"), RG.GetPrintOrderCalc(RG.GetCalc("invToWC")));
            //hongzhou 2010-5-10, add Cash to Net Sales, Log 10 (Net Sales)
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "4");
            Utility.PrintSummary(RG, rm.GetString("CashToNetSales"), RG.GetPrintOrderCalc(RG.GetCalc("CashToNetSales")));
            Utility.PrintSummary(RG, rm.GetString("Log10NetSales"), RG.GetPrintOrderCalc(RG.GetCalc("Log10NetSales")));

            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");

            Utility.Skip(RG, 1);
            Utility.mT.AddEndRow(Utility.nRow);

            Utility.mT.AddStartRow(Utility.nRow + 1);
            if ((RG.ISCONSOLIDATION == 0) && (RG.IsBudgetToActual == false))
            {
                //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
                Utility.PrintLabel(RG, rm.GetString("growth"));
                //RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
            }
            RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "2");
            Utility.PrintSummary(RG, rm.GetString("sustainGrowth"), RG.GetPrintOrderCalc(RG.GetCalc("sustainGrowth")));
            ///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
            ///ExchRateWarnMsg.printWarning(RG, Utility, rm);
            Utility.Skip(RG, 1);
            Utility.PrintUDAs(RG);

            Utility.UnderlinePage(RG, 2);
            Utility.mT.AddEndRow(Utility.nRow);

            //End: Keep together for Full Report
            Utility.mT.AddEndRow(Utility.nRow);
            Utility.CloseReport(RG);
        }
    }
}
