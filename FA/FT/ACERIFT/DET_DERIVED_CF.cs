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
	/// Summary description for DET_DERIVED_CF.
	/// </summary>
	public class DET_DERIVED_CF: FinancialAnalyst.IReport
	{
		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.DetDervCF_Calcs(RG);
			Calcs.IS_Calcs(RG);

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
			RG.SetAuthorSetting(FORMATCOMMANDS.OMIT, "First");
            //amit: log# 1997
            RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");

			Utility.Skip(RG, 1);

			if (RG.GetCalc("cfFrmOpAct").NonZero)
				Utility.PrintLabel(RG, rm.GetString("operAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.PrintSummary(RG, rm.GetString("netPrftLoss"), RG.GetPrintOrderCalc(RG.GetCalc("NetPrftLoss")));
			if (RG.GetCalc("totAdjs").NonZero)
				Utility.PrintLabel(RG, rm.GetString("reconItems"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			///CPF 07/31/06 Log 1579:  added back type -244
			printDetails( new int[]{-263, -268, -269, 258, 259, 260, 208, 219, 220, 221, 225, -245, -246, -247, -248, -235, 232, 234, 233, -243, -244, -249, -251, -250,
									   -252, -253});

			if (RG.GetCalc("NetTrdRecv").NonZero)
				Utility.PrintSummary(RG, rm.GetString("chgInNetRecv"), RG.GetPrintOrderCalc(RG.GetCalc("chgNetRecv")));

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{52, 61});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			if (RG.GetCalc("chgOthCurrOperAssts").NonZero)
				Utility.PrintSummary(RG, rm.GetString("chgOthCurOpAssts"), RG.GetPrintOrderCalc(RG.GetCalc("chgOthCurrOperAssts")));
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{37});

			if (RG.GetCalc("chgOthNCOpAssts").NonZero)
				Utility.PrintSummary(RG, rm.GetString("chgOthNCOpAssts"), RG.GetPrintOrderCalc(RG.GetCalc("chgOthNCOpAssts")));

			printChgIn(0, new int[]{161, 165, 153, 159, 166, 167, 168, 169, 171, 170});
			if (RG.GetCalc("chgOthCurOpLiab").NonZero)
				Utility.PrintSummary(RG, rm.GetString("chgOthCurOpLiab"), RG.GetPrintOrderCalc(RG.GetCalc("chgOthCurOpLiab")));
			printChgIn(0, new int[]{118, 129, 122, 123, 124, 125, 126, 128, 127});

			if (RG.GetCalc("chgOthNCOpLiab").NonZero)
				Utility.PrintSummary(RG, rm.GetString("chgOthNCOpLiab"), RG.GetPrintOrderCalc(RG.GetCalc("chgOthNCOpLiab")));

			if (RG.GetCalc("incTaxPaid").NonZero)
				Utility.PrintSummary(RG, rm.GetString("incTaxPaid"), RG.GetPrintOrderCalc(RG.GetCalc("incTaxPaid")));

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (RG.GetCalc("totAdjs").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
				Utility.PrintSummary(RG, rm.GetString("totAdjs"), RG.GetPrintOrderCalc(RG.GetCalc("totAdjs")));

			}
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("cshFrmOperAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmOpAct")));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			//Utility.Skip(RG, 1);
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (RG.GetCalc("cfFrmInvstAct").NonZero)
				Utility.PrintLabel(RG, rm.GetString("invstAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printChgIn(1, new int[]{5, 6, 7, 8, 9, 10, 12, 13});
			printChgIn(0, new int[]{14});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-208, -219, 245});

			if (RG.GetCalc("chgNetFixAssts").NonZero)
			{
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
				Utility.UnderlineColumn(RG,1, 1);
				Utility.PrintSummary(RG, rm.GetString("chgNetFixAssts"), RG.GetPrintOrderCalc(RG.GetCalc("chgNetFixAssts")));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			}

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{15});
			printChgIn(0, new int[]{16});
			printChgIn(1, new int[]{19});
			printChgIn(1, new int[]{20});
			printChgIn(1, new int[]{21});
			printChgIn(1, new int[]{22});
			printChgIn(1, new int[]{17});
			printChgIn(0, new int[]{18});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-220, -221});

			if (RG.GetCalc("chgNetIntg").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
				Utility.PrintSummary(RG, rm.GetString("chgNetIntg"), RG.GetPrintOrderCalc(RG.GetCalc("chgNetIntg")));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			}

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{27, 28, 30, 41});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{246, 244,  247, 248, -225, 235});

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.UnderlineColumn(RG,1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("cfFrmInvstAct"), RG.GetPrintOrderCalc(RG.GetCalc("cfFrmInvstAct")));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			//Utility.Skip(RG, 1);
			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (RG.GetCalc("DrvcfFrmFinAct").NonZero)
				Utility.PrintLabel(RG, rm.GetString("finAct"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(0, new int[]{110, 111, 112, 113, 114, 140, 141, 142, 143, 144, 145, 147, 148});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");

			if (RG.GetCalc("intPaid").NonZero)
				Utility.PrintSummary(RG, rm.GetString("intPaid"), RG.GetPrintOrderCalc(RG.GetCalc("intPaid")));

			if (RG.GetCalc("divPaid").NonZero)
				Utility.PrintSummary(RG, rm.GetString("divPaid"), RG.GetPrintOrderCalc(RG.GetCalc("divPaid")));

			printDetails( new int[]{243, 249, 250});
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("cfFrmFinAct"), RG.GetPrintOrderCalc(RG.GetCalc("DrvcfFrmFinAct")));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

			Utility.UnderlinePage(RG, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			Utility.mT.AddStartRow(Utility.nRow + 1);

			if (RG.GetCalc("othAct").NonZero)
			{
				Utility.PrintLabel(RG, rm.GetString("other"));
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
				RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			}
			printChgIn(0, new int[]{80, 81, 82, 87, 83, 84,85, 86});
			printChgIn(1, new int[]{90});
			printChgIn(0, new int[]{91, 93, 94, 95, 96});

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{291, 292});

			Utility.PrintSummary(RG, rm.GetString("adjDueExchRate"), RG.GetPrintOrderCalc(RG.GetCalc("adjChgInExchRate")));
			Utility.PrintSummary(RG, rm.GetString("conAdjNetPrft"), RG.GetPrintOrderCalc(RG.GetCalc("convAdjNetPrft")));

			printDetails( new int[]{-290});

			Utility.PrintSummary(RG, rm.GetString("unexpAdjRetPrfts"), RG.GetPrintOrderCalc(RG.GetCalc("unexpAdjRetPrfts")));

			printDetails( new int[]{251, 252, 253, 263, 268, 269});
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.UnderlineColumn(RG,1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("totMoveInCash"), RG.GetPrintOrderCalc(RG.GetCalc("dcfTotMoveInCash")));
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");
			Utility.PrintSummary(RG, rm.GetString("begPerCshEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("dcfBegofPerCashEquiv")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("endOfPerCshEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("dcfEndofPerCashEquiv")));
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
				int id = Convert.ToInt32(ids[i].ToString());
				if (id < 0)
				{
					id = Math.Abs(id);
					label = "F_" + id;
					//Utility.PrintDetail(RG, (-1) * RG.GetDetailCalcs(label)* RG.CONV_RATE_IS());
					Utility.PrintDetail(RG, (-1) * RG.GetDetailCalcs(label));
				}
				else
				{
					label = "F_" + id;
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(label) * RG.CONV_RATE_IS());
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
					Utility.PrintDetail(RG, RG.GetDetailCalcs(label) - RG.GetDetailCalcs(laglabel));
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(label) * RG.CONV_RATE_BS() - RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG));
				else if (flow == 1)
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG) -RG.GetDetailCalcs(label) * RG.CONV_RATE_BS());
					Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel)- RG.GetDetailCalcs(label));

			}

		}


	}
}
