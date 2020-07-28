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
	/// Summary description for RECON.
	/// </summary>
	public class RECON: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.ReconCalcs(RG);
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

			///CPF 08/14/06 Log 1759:  Removed "Reconcile To" to match design.
//			ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
//			Utility.arrColHead.Add(chead2);

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_LABEL, "2.75");
			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			//RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");

			RG.SetAuthorSetting(FORMATCOMMANDS.SPACING_COLUMNS, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "");
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
			///This creates the standard page header for the report.
			Utility.CreatePageHeader(RG);

			///This prints the statement constant rows
			Utility.PrintStmtConstRows(RG, 1);
			//Print Source and Target Currency
			Utility.PrintSourceTargetCurr(RG);

			//  Start of the outer Group (Full Report)
			Utility.mT.AddStartRow(Utility.nRow + 1);

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

			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");


			Utility.UnderlinePage(RG, 2);
			Utility.PrintCenter(RG, rm.GetString("reconRetPrfts"));
			Utility.UnderlinePage(RG, 1);
			int line413 = 0;
			Calc temp = RG.DETAILTYPE(291).GetTotals(RG) + RG.DETAILTYPE(291).GetTotals(RG);
			if (temp.NonZero) line413 = 1;
			int line412 = 0;
			if (RG.GetCalc("chgInExhRatePerEnd").NonZero) line412 = 1;
			int line414 = 0;
			if (line413 > 0) line414 = 1;
			int line415 = line412 + line414;

			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");

			if (line415 != 0)
			{
				Utility.PrintLabel(RG, rm.GetString("begRetPrftPrevRep"));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
				Utility.PrintSummary(RG, rm.GetString("asPrevRprted"), RG.GetPrintOrderCalc(RG.GetCalc("begRetPrftPrevRep")));//416
			}
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");

			if (line413 > 0)
			{
				 printDetails( new int[]{291, 292}); //417, 418
			}
			if (RG.GetCalc("chgInExhRatePerEnd").NonZero)
				Utility.PrintSummary(RG, rm.GetString("adjDueExchRate"), RG.GetPrintOrderCalc(RG.GetCalc("adjChgInExchRate")));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (line415 != 0)
			{
				Utility.UnderlineColumn(RG,1, 1);
				Utility.PrintSummary(RG, rm.GetString("begRetPrftRest"), RG.GetPrintOrderCalc(RG.GetCalc("begRetPrftRest")));
			}
			Utility.PrintSummary(RG, rm.GetString("begRetEarn"), RG.GetPrintOrderCalc(RG.GetCalc("begRetEarn")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("netPrftLoss"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftLoss")));
			Utility.PrintSummary(RG, rm.GetString("conAdjNetPrft"), RG.GetPrintOrderCalc(RG.GetCalc("convAdjNetPrft")));
			printDetails( new int[]{-287, -288, -289, -290});
			Utility.PrintSummary(RG, rm.GetString("unexpAdjRetPrfts"), RG.GetPrintOrderCalc(RG.GetCalc("unexpAdjRetPrfts")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (RG.GetCalc("endRetPrfts").NonZero)
				Utility.UnderlineColumn(RG, 1, 1);
			Utility.PrintSummary(RG, rm.GetString("endRetPrfts"), RG.GetPrintOrderCalc(RG.GetCalc("endRetPrfts")));
			Utility.Skip(RG, 1);
			Utility.UnderlinePage(RG, 1);
			//if (line415 != 0)
				//Utility.UnderlinePage(RG, 2);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddStartRow(Utility.nRow + 1);

			Utility.PrintCenter(RG, rm.GetString("reconEqtyRsrvs"));
			Utility.UnderlinePage(RG, 1);
			Utility.PrintSummary(RG, rm.GetString("begEqtyRsrvs"), RG.GetPrintOrderCalc(RG.GetCalc("begEqAndRsrvs")));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			Utility.PrintSummary(RG, rm.GetString("netPrftLoss"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftLoss")));
			Utility.PrintSummary(RG, rm.GetString("cshStockDivd"), RG.GetPrintOrderCalc(RG.GetCalc("cshStkDivd")));
			Utility.PrintSummary(RG, rm.GetString("adjRetPrftAbove"), RG.GetPrintOrderCalc(RG.GetCalc("adjToRetPrft")));

			int[] a = new int[]{80, 81, 82, 83, 84, 85, 86, 91, 93, 94, 95, 96};

			Calc aCalc = new Calc(0, RG.Statements.Count);
			for (int i=0; i < a.Length; i++)
			{
				int  flowid = Convert.ToInt32(a[i].ToString());

				aCalc = aCalc + (RG.FLOW(flowid) * RG.CONV_RATE_BS() - RG.FLOW(flowid, RG.LAG) * RG.CONV_RATE_BS(RG.LAG));
			}


			Calc cal430 = aCalc + (RG.FLOW(90) * RG.CONV_RATE_BS() - RG.FLOW(90, RG.LAG) * RG.CONV_RATE_BS(RG.LAG));


			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (cal430.NonZero)
				Utility.PrintLabel(RG, rm.GetString("incDecrIn"));

			///CPF 08/15/06 Log 1759:  Added type 87
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printChgIn(0, new int[]{80, 81, 82, 87, 83, 84, 85, 86});
			printChgIn(1, new int[]{90});
			printChgIn(0, new int[]{91, 93, 94, 95, 96});

			if (cal430.NonZero)
				Utility.UnderlineColumn(RG,1, 1);

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.PrintSummary(RG, rm.GetString("actEndEqtyRsrvs"), RG.GetPrintOrderCalc(RG.GetCalc("actEndEqtyRsrvs")));
			if (RG.GetCalc("incDecEqtyRsrvs").NonZero)
				Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("incDecrEqtyRsrvs"), RG.GetPrintOrderCalc(RG.GetCalc("incDecEqtyRsrvs")));
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
				int id = Convert.ToInt32(ids[i].ToString());
				if (id < 0)
				{
					id = Math.Abs(id);
					label = "T_" + id;
					Utility.PrintDetail(RG, (-1) * RG.GetDetailCalcs(label));
				}
				else
				{
					label = "T_" + id;
					Utility.PrintDetail(RG, RG.GetDetailCalcs(label));
				}
			}
		}

		private void printChgIn(int flow, int[] ids)
		{
			for (int i=0; i< ids.Length; i++)
			{
				string label = "F_" + ids[i].ToString();
				string laglabel = "FL_"+ ids[i].ToString();
				if (flow == 0)
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(label) * RG.CONV_RATE_BS() - RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG));
					Utility.PrintDetail(RG, RG.GetDetailCalcs(label)- RG.GetDetailCalcs(laglabel));
				else if (flow == 1)
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG) -RG.GetDetailCalcs(label) * RG.CONV_RATE_BS());
					Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel) - RG.GetDetailCalcs(label));

			}

		}
	}
}
