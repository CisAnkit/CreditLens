using System.Collections;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;
using System;

namespace ACERIFT
{
	/// <summary>
	/// Summary description for CREDIT_COMP.
	/// </summary>
	public class CREDIT_COMP: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		ResourceManager rm = null;
		
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.CredCompCalcs(RG);

			///Load the resource manager.
			rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);
			
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

			///CPF 08/15/06 Log 1761:  Removed "Reconcile To" to match design doc.
//			ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
//			Utility.arrColHead.Add(chead2);
			
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_LABEL, "2.75");
			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SPACING_COLUMNS, "0");
						
			///This creates the standard page header for the report.  
			Utility.CreatePageHeader(RG);

			///This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);

			Utility.mT.AddStartRow(Utility.nRow + 1);

			Calc line994 = RG.ACCOUNT(7500)+RG.ACCOUNT(7520)+RG.ACCOUNT(7540);
			if (line994.NonZero)
				Utility.PrintLabel(RG, rm.GetString("liquidity"));

			
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			processAccts(7500, rm.GetString("workingCap") ,false);

			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			processAccts(7520, rm.GetString("currRatio") ,false);
			processAccts(7540, rm.GetString("quickRatio") ,false );
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			Calc line1051 = RG.ACCOUNT(7560)+RG.ACCOUNT(7580)+RG.ACCOUNT(7590)+RG.ACCOUNT(7600)+
				           RG.ACCOUNT(7620)+RG.ACCOUNT(7640);
			if (line1051.NonZero)
				Utility.PrintLabel(RG, rm.GetString("levCovrg"));
			processAccts(7560, rm.GetString("ccdbtToTNW") , true);
			processAccts(7580, rm.GetString("intCovrg"), false);
			processAccts(7590, rm.GetString("earnCovrg"), false);
			//** THIS NEEDS TESTING
			if ((RG.GetCalc("icfCFOpAct")[0] == 0) && (RG.GetCalc("dcfCFOpAct")[0] == 0))
				RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			processAccts(7600, rm.GetString("cfCovrg"), false);
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
            if (RG.IsBudgetToActual == false)
            {
                processAccts(7620, rm.GetString("capExpnd"), true);
            }
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			processAccts(7640, rm.GetString("offBSLevrg"), true);


			Calc line1121 = RG.ACCOUNT(7660) + RG.ACCOUNT(7680) + RG.ACCOUNT(7700);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);
			if (line1121.NonZero)
			{
				Utility.PrintLabel(RG, rm.GetString("profitability"));
			}
			processAccts(7660, rm.GetString("netPrftMargin"), false);
			processAccts(7680, rm.GetString("ROA"), false);
			processAccts(7700, rm.GetString("ROE"), false);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			Calc line1181 = RG.ACCOUNT(7720) + RG.ACCOUNT(7740) + RG.ACCOUNT(7760);
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);
			if (line1181.NonZero)
			{
				Utility.PrintLabel(RG, rm.GetString("activity"));
			}
			processAccts(7720, rm.GetString("netTrdRecvDays"), true);
			processAccts(7740, rm.GetString("invDays"), true);
			processAccts(7760, rm.GetString("trdPayDays"), true);

			Calc line1307 = RG.ACCOUNT(7780) + RG.ACCOUNT(7800) + RG.ACCOUNT(7820) + RG.ACCOUNT(7840) + RG.ACCOUNT(7860);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (line1307.NonZero)
			{
				Utility.PrintLabel(RG, rm.GetString("othRelns"));
			}
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
		    RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");
            if (RG.IsBudgetToActual == false)
            {
                processAccts(7780, rm.GetString("salesGrwthper"), true);
            }
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "None");
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			processAccts(7800, rm.GetString("TNW"), false);
			processAccts(7820, rm.GetString("subOrdDebt"), true);
			processAccts(7840, rm.GetString("cashDivd"), true);
			processAccts(7860, rm.GetString("cashBal"), false);

            Utility.PrintUDACompliance(RG);

			///CPF 02/01/06 Log 1603:  This warning is no longer necessary.
			//ExchRateWarnMsg.printWarning(RG, Utility, rm);
			Utility.UnderlinePage(RG, 2);
			
			Utility.mT.AddEndRow(Utility.nRow);
			//End: Keep together for Full Report
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.CloseReport(RG);
		}

		private void processAccts(int actId,  string Label, bool isMax)
		{
			
			if (RG.ACCOUNT(actId).NonZero)
			{
				RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "0");

				Utility.PrintLabel(RG, Label);
				//Utility.PrintSummary(RG, Label + " - " + rm.GetString("ccActual"), RG.GetPrintOrderCalc(RG.GetCalc(actId+"act")));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
				Utility.PrintSummary(RG, rm.GetString("ccActual"), RG.GetPrintOrderCalc(RG.GetCalc(actId+"act")));
				//RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "12");
				string label = "ccCovMin";
				if (isMax)
					label = "ccCovMax";
				Utility.PrintSummary(RG, rm.GetString(label), RG.GetPrintOrderCalc(RG.GetCalc(actId+"cov")));
				Utility.PrintSummary(RG, rm.GetString("ccVariance"), RG.GetPrintOrderCalc(RG.GetCalc(actId+"var")));
			
				Calc defCheck = new Calc();
				for (int i = 0; i < RG.GetCalc(actId+"var").Count; i++) 
					if (RG.GetCalc(actId+"var")[i] < 0)
						defCheck.Add(double.NaN);
					else
						defCheck.Add(0);

				Calc def = new Calc();
				for (int i = 0; i < RG.ACCOUNT(actId).Count; i++) 
					if (RG.ACCOUNT(actId)[i] != 0)
						def.Add(defCheck[i]);
					else
						def.Add(0);

				RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "");
				RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, rm.GetString("ccDefault"));
				//RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "**********");
				if (def.Contains(double.NaN))
					RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
				else
					RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

				Utility.PrintSummary(RG, rm.GetString("ccResult"), RG.GetPrintOrderCalc(def));
				//Utility.PrintSummary(RG, rm.GetString("ccDefault"), RG.GetPrintOrderCalc(def));
				RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
				RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "N/A");
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
				Utility.Skip(RG, 1);
								
			}
		}
	}
}
