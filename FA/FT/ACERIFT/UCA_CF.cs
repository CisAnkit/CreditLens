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
	/// Summary description for UCA_CF.
	/// </summary>
	public class UCA_CF: FinancialAnalyst.IReport

	{

		PRINTCOMMANDS Utility = new PRINTCOMMANDS();
		ReportGenerator RG= null;
		public void Execute(ReportGenerator RGG)
		{
			RG = RGG;
			CALCULATIONS Calcs = new CALCULATIONS(RG);
			Calcs.UCACashFlow_Calcs(RG);
			Calcs.DetDervCF_Calcs(RG);

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

			ColumnHeader chead2 = new ColumnHeader(rm.GetString("ReconTo"), RG.GetPrintOrderObject(ExchRateWarnMsg.ReconcileDate(RG, rm)), "", "");
			Utility.arrColHead.Add(chead2);

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

			//Cash Collected from Sales section
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printDetails( new int[]{200});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{66});
            printChgIn(0, new int[]{67});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			printDetails( new int[]{-224});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{55});
            printChgIn(0, new int[]{154,153,159,118,129});
            //always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
	        RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");

			if (RG.GetCalc("ucaCshCollect").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}
			Utility.PrintSummary(RG, rm.GetString("ucaCshCollect"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshCollect")));
			Utility.Skip(RG, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

			//End of Cash Collected
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Cash Paid To Suppliers section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			//CPF added the printing of type 205, as goods and services was added back to IFT.
			printDetails( new int[]{203,204,-202,-205,-206});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{52});
			printChgIn(0, new int[]{161,165,149,120});

			//always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (RG.GetCalc("ucaCshPdSupplier").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}
			Utility.PrintSummary(RG, rm.GetString("ucaCshPdSupplier"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshPdSupplier")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaCshTrdingActiv"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshFrmTrdAct")));

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			//End of Cash Paid to Supplier
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Cash Paid for Operating Costs section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-212,-213,-214,-217,-218,-222,-223,-226,-215,-216,-228,-227});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{61,40,35});
            printChgIn(0, new int[]{151,158,166,167,168,169,171,170,121,122,123,124,126,128,127});
			//always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (RG.GetCalc("ucaCshPdOpCosts").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}

			Utility.PrintSummary(RG, rm.GetString("ucaCshPdOpCosts"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshPdOpCosts")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaCashAftOp"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshAftOp")));
			if (RG.GetCalc("ucaCshAftOp").NonZero)
				Utility.Skip(RG, 1);


			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			//End Cash Pd for Oper Costs
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Other Income(Exp) & Taxes Paid
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{209,210,211});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{50,65});
            printChgIn(0, new int[]{152});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{235,-254,-258,-259,-260});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{60,37});
			printChgIn(0, new int[]{125,156});
			//always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (RG.GetCalc("ucaOthIncExpTaxPd").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}
			Utility.PrintSummary(RG, rm.GetString("ucaOthIncExpTxPd"), RG.GetPrintOrderCalc(RG.GetCalc("ucaOthIncExpTaxPd")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaNetCshAftOp"), RG.GetPrintOrderCalc(RG.GetCalc("ucaNetCshAftOp")));
			Utility.UnderlinePage(RG, 2);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			//End Oth inc Exp Taxes Paid
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Cash Paid for Dividends & Interest section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-287,-288});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(0, new int[]{160});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-232,-234,-233});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(0, new int[]{157});
			//always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			if (RG.GetCalc("ucaCshPdDivsInt").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}
			Utility.PrintSummary(RG, rm.GetString("ucaCshPdDivsInt"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshPdDivsInt")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaNetCshInc"), RG.GetPrintOrderCalc(RG.GetCalc("ucaNetCshInc")));

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			//End Cash Paid for Divs & Int
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Current Portion LTD section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			Utility.PrintDetail(RG, RG.GetDetailCalcs("DTCPLTDBank"));
			Utility.PrintDetail(RG, RG.GetDetailCalcs("DTCPLTDOther"));
			Utility.PrintDetail(RG, RG.GetDetailCalcs("DTCPLTDConver"));
			Utility.PrintDetail(RG, RG.GetDetailCalcs("DTCPLTDSub"));
			Utility.PrintDetail(RG, RG.GetDetailCalcs("DTCPLTDFinLs"));

				//always print subtotal even if 0;only underline if non-zero
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (RG.GetCalc("ucaCshAftDbtAmort").NonZero)
			{
				Utility.UnderlineColumn(RG,1, 1);
			}
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("ucaCPLTD"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCPLTD")));
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaCshAftDbtAmort"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshAftDbtAmort")));

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");

			if (RG.GetCalc("ucaCshAftDbtAmort").NonZero)
				Utility.Skip(RG, 1);

			//End of CPLTD
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Chg in Net Fixed Assets section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{5,6,7,8,9,10,12,13});
			printChgIn(0, new int[]{14});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-208,-219,-225,245});

			//Continue to indent the Net Fixed Assets subtotal; do not print if 0
			if (RG.GetCalc("ucaChgNFA").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
				Utility.PrintSummary(RG, rm.GetString("chgNetFixAssts"), RG.GetPrintOrderCalc(RG.GetCalc("ucaChgNFA")));
			    Utility.Skip(RG, 1);
			}

			//End chg in NFA
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Chg in Intangibles section
            RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{15});
            printChgIn(0, new int[]{16});
			printChgIn(1, new int[]{19,20,21,22,17});
			printChgIn(0, new int[]{18});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-220,-221});
			//Continue to indent the Intangibles subtotal; do not print if 0
			if (RG.GetCalc("ucaChgIntang").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "4");
				Utility.PrintSummary(RG, rm.GetString("chgNetIntg"), RG.GetPrintOrderCalc(RG.GetCalc("ucaChgIntang")));
				Utility.Skip(RG, 1);
			}
			//End of chg in intangibles
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Chg in Investments section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{54,56,63,57,58,59,70,68,41,27,28,30,31,32,33,34,36,39});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{238,239,243,244,249,251,250,246,248});

			//Continue to indent the Investments subtotal; do not print if 0
			if (RG.GetCalc("ucaChgInInvest").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
				Utility.PrintSummary(RG, rm.GetString("chgInInvest"), RG.GetPrintOrderCalc(RG.GetCalc("ucaChgInInvest")));
			}

			//Always print this subtotal; only print underline if a non-zero total exists
			if (RG.GetCalc("ucaCshPdPlantInvest").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
			}

			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("ucaCshPdPlantInvest"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCshPdPlantInvest")));

			//End of chg in investments
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			//Extraordinary and Non-Cash Items section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(1, new int[]{64});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{247,252,253,268,269});

			//Continue to indent the Extraord subtotal; do not print if 0
				RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (RG.GetCalc("ucaExtraNonCash").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
				Utility.PrintSummary(RG, rm.GetString("ucaExtraNonCash"), RG.GetPrintOrderCalc(RG.GetCalc("ucaExtraNonCash")));
			}
			//End of Extraord
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.UnderlineColumn(RG,1, 1);
			Utility.PrintSummary(RG, rm.GetString("ucaFinSurp"), RG.GetPrintOrderCalc(RG.GetCalc("ucaFinSurp")));
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "True");
			Utility.Skip(RG, 1);
			//End of Fin Surp
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
		    Utility.mT.AddStartRow(Utility.nRow + 1);
			//Total External Financing section
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(0, new int[]{144,145,148,150,155});

			Utility.PrintSummary(RG, rm.GetString("ucaChginLTD"), RG.GetPrintOrderCalc(RG.GetCalc("ucaChgInLTD")));

			printChgIn(0, new int[]{115,116,117,119,80,81});

			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{-289});
            RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, rm.GetString("chgIn"));
			printChgIn(0, new int[]{82,87,83,84,85,86});
			printChgIn(1, new int[]{90});
			printChgIn(0, new int[]{91,93,94,95,96});
			RG.SetAuthorSetting(FORMATCOMMANDS.PREFIX, "");
			printDetails( new int[]{263,-290,291,292});

			Utility.PrintSummary(RG, rm.GetString("adjDueExchRate"), RG.GetPrintOrderCalc(RG.GetCalc("adjChgInExchRate")));
			Utility.PrintSummary(RG, rm.GetString("conAdjNetPrft"), RG.GetPrintOrderCalc(RG.GetCalc("convAdjNetPrft")));
			Utility.PrintSummary(RG, rm.GetString("unexpAdjRetPrfts"), RG.GetPrintOrderCalc(RG.GetCalc("unexpAdjRetPrfts")));

			//do not print if 0
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			if (RG.GetCalc("ucaTotExtFin").NonZero)
			{
				Utility.UnderlineColumn(RG, 1, 1);
				Utility.PrintSummary(RG, rm.GetString("ucaTotExtFin"), RG.GetPrintOrderCalc(RG.GetCalc("ucaTotExtFin")));
			}
			//do not print if 0
			if (RG.GetCalc("ucaCashAftFin").NonZero)
			{
				Utility.PrintSummary(RG, rm.GetString("ucaCashAftFin"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCashAftFin")));
				Utility.Skip(RG, 1);
			}
			//End Tot Ext Fin
			Utility.mT.AddEndRow(Utility.nRow);
			//Start
			Utility.mT.AddStartRow(Utility.nRow + 1);

			///HERE WE WOULD SHUT OFF ST_CROSS_FOOT FOR STP.
			RG.SetAuthorSetting(FORMATCOMMANDS.ST_CROSS_FOOT, "False");

			//almost there! ENDING CASH & EQUIVALENTS section

			//do not print if no changes in cash
			if (RG.GetCalc("ucaCAFPlusCash").NonZero)
			{
				Utility.PrintLabel(RG, rm.GetString("ucaAdd"));
				//Utility.PrintSummary(RG, rm.GetString("civCashEq"), RG.GetPrintOrderCalc(RG.GetCalc("dcfBegofPerCashEquiv")));
				Utility.PrintSummary(RG, rm.GetString("ucaCashEquiv"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCashEquiv")));
				Utility.PrintSummary(RG, rm.GetString("ucaCashIBIH"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCashIBIH")));
				Utility.PrintSummary(RG, rm.GetString("ucaCashAdj"), RG.GetPrintOrderCalc(RG.GetCalc("ucaCashAdj")));
			}
            //underline & print even if 0
			Utility.UnderlineColumn(RG, 1, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			Utility.PrintSummary(RG, rm.GetString("ucaEndCash"), RG.GetPrintOrderCalc(RG.GetCalc("ucaEndCash")));


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
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(label) * RG.CONV_RATE_BS() - RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG));
					Utility.PrintDetail(RG, RG.GetDetailCalcs(label)- RG.GetDetailCalcs(laglabel));
				else if (flow == 1)
					//Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel)* RG.CONV_RATE_BS(RG.LAG) -RG.GetDetailCalcs(label) * RG.CONV_RATE_BS());
					Utility.PrintDetail(RG, RG.GetDetailCalcs(laglabel) - RG.GetDetailCalcs(label));
			}

		}


	}
}
