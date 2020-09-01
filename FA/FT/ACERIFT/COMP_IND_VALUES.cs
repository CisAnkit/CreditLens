using System.Collections;
using FinancialAnalyst;
using MKMV.RiskAnalyst.ReportAuthoring.PrintUtility;
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Resources;


namespace ACERIFT
{
	/// <summary>
	/// Summary description for COMP_IND_VALUES.
	/// </summary>
	public class COMP_IND_VALUES: FinancialAnalyst.IReport
	{
		public void Execute(ReportGenerator RG)
		{
			CALCULATIONS Calcs = new CALCULATIONS(RG);

			Calcs.BS_Calcs(RG);
			Calcs.IS_Calcs(RG);
			#region InitVar
			///***CPF 11/6/02 Load the resource manager.
			ResourceManager rm = FORMATCOMMANDS.GetResourceManager(Assembly.GetExecutingAssembly().GetName().Name);

			///This variable will store the stmt index of the base comparison stmt
			int PBaseId;
			int BaseId = RG.BaseStatementID;
			PBaseId = RG.Statements.IndexOf(RG.Context.Statements[BaseId.ToString()]);
			StatementConstant sc = (StatementConstant)RG.Customer.Model.StatementConstants[SCON.AuditOpinionStmtSource - 1];

			//int ind = 0;
			int intPeriod = (int)(RG.STMT_PERIODS()[PBaseId]);
			int intMonth = (int)(RG.STMT_MONTH()[PBaseId]);
			int intYear = (int)(RG.STMT_YEAR()[PBaseId]);
			int dbType = RG.DATABASE_TYPE();
			//int indCat = 0;

			//string header = "";
			string s0 = "";
			string s1 = "";
			string s2 = "";
			string s3 = "";
			string sSpace = " ";
			string sVersion = "";
			//string sRegion = "";
			string sTimePeriod = "";
			string auditMethod = "";
			string sDate = Convert.ToDateTime(RG.STMT_DATE()[PBaseId]).ToShortDateString();
            ///CPF 06/22/07 SCR 1986:  Change ToUpper to ToUpperInvariant.
			string sAdtMthVal = (RG.LANGSTMTCONSTANT(SCON.AuditOpinionStmtSource)[PBaseId].ToString()).ToUpperInvariant();
            //DE77176 - Commented out as it is not used
            //string sCurrencyTarget = RG.TARGETCURRENCY(0)[0].ToString();

            string[] Justif = new string[4];
			Justif[0] = Justif[1] = Justif[2] = Justif[3] = "Left";

			//Create and print numbers
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			#endregion InitVar

			//It has to be after initialization because it uses PBaseId from it.
			Calcs.Peer_Comp_Ind_Val(RG, PBaseId);

			#region Author's Settings
			///***CPF 3/11/02 This instantiates the Utility object.
			PRINTCOMMANDS Utility = new PRINTCOMMANDS();
			FORMATCOMMANDS FormatCommands = new FORMATCOMMANDS();

			FormatCommands.LoadFormatDefaults(RG);

			RG.SetAuthorSetting(FORMATCOMMANDS.SUPPRESS, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.COMMAS_ON_1, "True");
			RG.SetAuthorSetting(FORMATCOMMANDS.NEGATIVE_CHAR_1, "()");
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "-");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_LABEL, "2");
			RG.SetAuthorSetting(FORMATCOMMANDS.WIDTH_COLUMN, "1.5");

			///CPF 6/24/03  This is so that we don't round these items.
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "False");	
			#endregion Author's Settings

			///***CPF 3/11/02 This creates the standard page header for the report.  If
			///this as new report, make sure the NewReport parm is "True"
			Utility.CreatePageHeader(RG,true);

			//Get Audit Method from customer
            ///CPF 06/22/07 SCR 1986:  Change ToLower to ToLowerInvariant
			sAdtMthVal = (RG.LANGSTMTCONSTANT(SCON.AuditOpinionStmtSource)[PBaseId].ToString()).ToLowerInvariant();
			intPeriod = (int)(RG.STMT_PERIODS()[PBaseId]);
			if (intPeriod == 12)
				sTimePeriod = string.Format(rm.GetString("civAnnual"), intYear);
			else
				sTimePeriod = string.Format(rm.GetString("civInterim"), intPeriod, sDate);
			sDate = Convert.ToDateTime(RG.STMT_DATE()[PBaseId]).ToShortDateString();

			//  DETERMINE WHAT TYPE OF STATEMENT IS BEING ANALYZED
			#region Audit Type Compare			
			string sAdtMthValAfter = "";
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
			if (rm.GetString("civUnqUC").ToLowerInvariant().Equals(sAdtMthVal) )
			{
                sAdtMthVal = rm.GetString("civUnqLC").ToLowerInvariant() + sSpace;
				auditMethod = "unqualified";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civRevUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
				sAdtMthVal = rm.GetString("civRevUC").ToLowerInvariant() + sSpace;
				auditMethod = "reviewed";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civQualUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
                sAdtMthVal = rm.GetString("civQualUC").ToLowerInvariant() + sSpace;
				auditMethod = "qualified";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civCompUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
                sAdtMthVal = rm.GetString("civCompUC").ToLowerInvariant() + sSpace;
				auditMethod = "compiled";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civCoPrdUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
				sAdtMthVal = rm.GetString("civCoPrdLC") + sSpace;
				auditMethod = "companyprepared";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civProjUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
				sAdtMthVal = rm.GetString("civProjLC") + sSpace;
				auditMethod = "projection";
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civTaxRetUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
                sAdtMthVal = rm.GetString("civTaxRetUC").ToLowerInvariant() + sSpace;
				auditMethod = "taxreturn";
				sAdtMthValAfter = rm.GetString("civTaxRetLC") + sSpace;
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civDiscUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
                sAdtMthVal = rm.GetString("civDiscUC").ToLowerInvariant() + sSpace;
				auditMethod = "disclaimer";
				sAdtMthValAfter = rm.GetString("civDiscLC") + sSpace;
			}
            ///CPF 06/22/07 SCR 1986:  Changed ToLower to ToLowerInvariant
            else if (rm.GetString("civAdvUC").ToLowerInvariant().Equals(sAdtMthVal))
			{
                sAdtMthVal = rm.GetString("civAdvUC").ToLowerInvariant() + sSpace;
				auditMethod = "adverse";
				sAdtMthValAfter = rm.GetString("civAdvLC") + sSpace;
			}
			else if ((sAdtMthVal == " ") || (sAdtMthVal == ""))
			{
				sAdtMthVal = "";
				auditMethod = "";
			}
			else
			{
                sAdtMthVal = sAdtMthVal.ToLowerInvariant() + " ";
                auditMethod = sAdtMthVal.ToLowerInvariant();
			}
			#endregion Audit Type Compare

			///This is the equivalent of skip when you don't have a table yet.
			Utility.PrintParagraph(RG, " ");
			//Create and print first sentence "Base on..."
			//s0 = the whole sentence, s1 = Audit Type, s2 = Time period, s3 = ending date/empty.
			if (sAdtMthValAfter != "")
				s0 = string.Format(rm.GetString("civBaseOn"), "", sAdtMthValAfter, sTimePeriod + rm.GetString("sDot"));
			else
				s0 = string.Format(rm.GetString("civBaseOn"), sAdtMthVal, "", sTimePeriod + rm.GetString("sDot"));
			Utility.PrintParagraph(RG, s0);
			s0 = s1 = s2 = s3 = "";
			//Skip the line.
			Utility.PrintParagraph(RG, " ");
			
			//Create and print the line "The analysis ..." with the detailed description of the size (s1, s2) and etc.
			if (RG.IND(94) > 0)
			{
				if ((RG.IND_Category == (int)ePeerCategory.Assets) || (RG.IND_Category == (int)ePeerCategory.Sales))
				{
					// For comparative purposes, the analysis of this company uses RG.IND(94)
					s0 = rm.GetString("civForCompPurp") + sSpace + RG.IND(94).ToString();
					// data for the industry code ..., ... sorted by
					if (((RG.PEER_CODE != "") || (RG.PEER_CODE != " ")) && ((RG.IND_DESC != "") || (RG.IND_DESC != " ")))
						s1 = sSpace + rm.GetString("sDash") + sSpace;
					if ((RG.IND_DESC == "") || (RG.IND_DESC == " "))
						s1 = "";
					s2 = rm.GetString("sComma") + sSpace;
					
					s0 = s0 + sSpace + string.Format(rm.GetString("civDataFor"), sSpace + RG.PEER_CODE + s1, RG.IND_DESC + s2);
					//assets size or sales size: 
					if (RG.IND_Category == (int)ePeerCategory.Assets)
						s0 = s0 + rm.GetString("civAssetsSize");
					else if (RG.IND_Category == (int)ePeerCategory.Sales)
						s0 = s0 + rm.GetString("civSalesSize");
					else
						s0 = s0 + "";
					// range.
					s0 = s0 + sSpace + string.Format(rm.GetString("civRange"), (RG.IND_SIZE).ToLower());
				}
				else if (RG.IND_Category ==(int)ePeerCategory.Totals)
				{
					//For all companies in the industry:
					s0 = string.Format(rm.GetString("civCompAn") + sSpace + rm.GetString("civUse"), RG.IND(94));
					s0 = s0 + sSpace + string.Format(rm.GetString("civAllCompIn"), RG.PEER_CODE, RG.IND_DESC);
				}
				else
					s0 = rm.GetString("civCompAn") + sSpace + rm.GetString("civDoesntUse");

				//The peer ...
				if (s0 != "")
					s0 = s0 + sSpace;
				if (RG.IND(7) == 0)
					s1 = rm.GetString("civPeer") + sSpace + rm.GetString("civDoNotHave") + sSpace;
				else
					s1 = rm.GetString("civPeer") + sSpace + string.Format(rm.GetString("civConsists"), RG.IND(7)) + sSpace;

				if (RG.IND(7) == 1)
					s2 = rm.GetString("civSing") + rm.GetString("sDot");
				else
					s2 = rm.GetString("civPlur") + rm.GetString("sDot");
			}
			else
				//if peer group is not recognised
			{
				if (RG.PEER_TYPE() == ePeerType.None)
					s0 = rm.GetString("civCompAn") + sSpace + rm.GetString("civDoNotHave");
				else if ((RG.IND_Category == (int)ePeerCategory.Assets) || (RG.IND_Category == (int)ePeerCategory.Sales) || (RG.IND_Category ==(int)ePeerCategory.Totals))
					s0 = rm.GetString("civForCompPurp") + sSpace + RG.IND_DESC.ToString() + rm.GetString("sDot");
			}

			Utility.PrintParagraph(RG, s0 + s1 + s2 + s3);
			s0 = s1 = s2 = s3 = "";
			Utility.PrintParagraph(RG, " ");
			
			#region BSD
			//Create the table for the Balance Sheet Data
			Utility.CreateTable(RG, 4);
			Utility.mT.AddStartRow(Utility.nRow + 1);
			
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			string[] RCLabels = new string[4];
			//Create and print headers
			RCLabels[0] = rm.GetString("civBSD");
			RCLabels[1] = rm.GetString("civCustVal");
			RCLabels[2] = rm.GetString("civPeerVal");
			RCLabels[3] = rm.GetString("civVarSC");
			Justif[0] = "Left";
			Justif[1] = Justif[2] = Justif[3] = "Right";
			Utility.PrintColumnLabels(RG, RCLabels, true, Justif);
			Utility.mT.LastRowAsHeader(0);
			
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintSummary(RG, rm.GetString("civNetFixAsts"), RG.GetCalc("NetFixAsts"));
			Utility.PrintSummary(RG, rm.GetString("civNetInt"), RG.GetCalc("NetInt"));
			Utility.PrintSummary(RG, rm.GetString("civAllOtherNonCurAsts"), RG.GetCalc("AllOtherNonCurAsts"));
			Utility.PrintSummary(RG, rm.GetString("civTotNonCurAsts"), RG.GetCalc("TotNonCurAsts"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);

			Utility.PrintSummary(RG, rm.GetString("civCashEq"), RG.GetCalc("CashEq"));
			Utility.PrintSummary(RG, rm.GetString("civNetTrRec"), RG.GetCalc("NetTrRec"));
			Utility.PrintSummary(RG, rm.GetString("civInvent"), RG.GetCalc("Invent"));
			Utility.PrintSummary(RG, rm.GetString("civAllOtherCurAsts"), RG.GetCalc("AllOtherCurAsts"));
			Utility.PrintSummary(RG, rm.GetString("civTotCurAsts"), RG.GetCalc("TotCurAsts"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);

			Utility.PrintSummary(RG, rm.GetString("civTotEqRes"), RG.GetCalc("TotEqRes"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);

			Utility.PrintSummary(RG, rm.GetString("civLTD"), RG.GetCalc("LTD"));
			Utility.PrintSummary(RG, rm.GetString("civDefTaxLiab"), RG.GetCalc("DefTaxLiab"));
			Utility.PrintSummary(RG, rm.GetString("civAllOthNonCurLiab"), RG.GetCalc("AllOthNonCurLiab"));
			Utility.PrintSummary(RG, rm.GetString("civTotNonCurLiab"), RG.GetCalc("TotNonCurLiab"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);

			Utility.PrintSummary(RG, rm.GetString("civOverdrft"), RG.GetCalc("Overdrft"));
			Utility.PrintSummary(RG, rm.GetString("civCPLTD"), RG.GetCalc("CPLTD"));
			Utility.PrintSummary(RG, rm.GetString("civTrade_Pay"), RG.GetCalc("Trade_Pay"));
			Utility.PrintSummary(RG, rm.GetString("civCurTaxPay"), RG.GetCalc("CurTaxPay"));
			Utility.PrintSummary(RG, rm.GetString("civAllOthCurLiab"), RG.GetCalc("AllOthCurLiab"));
			Utility.PrintSummary(RG, rm.GetString("civTotCurLiab"), RG.GetCalc("TotCurLiab"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			#endregion BSD
			
			#region IncData
			//Print Income Data
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			RCLabels[0] = rm.GetString("civIncData");
			RCLabels[1] = "";
			RCLabels[2] = "";
			RCLabels[3] = "";
			Justif[0] = "Left";
			Justif[1] = Justif[2] = Justif[3] = "Right";
			Utility.PrintColumnLabels(RG, RCLabels, true, Justif);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.mT.LastRowAsHeader(0);

			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			Utility.PrintSummary(RG, rm.GetString("civGrProf"), RG.GetCalc("GrProf"));
			Utility.PrintSummary(RG, rm.GetString("civOthOperIncExp"), RG.GetCalc("OthOperIncExp"));
			Utility.PrintSummary(RG, rm.GetString("civEBIT"), RG.GetCalc("EBIT_To_TA"));
			Utility.PrintSummary(RG, rm.GetString("civOthExp"), RG.GetCalc("OthExp"));
			Utility.PrintSummary(RG, rm.GetString("civProfLossB4Tax"), RG.GetCalc("ProfLossB4Tax"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			//Utility.CloseTable(RG);
			Utility.mT.AddEndRow(Utility.nRow);
			#endregion IncData

			#region Ratios
			//Utility.CreateTable(RG, 4);
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RCLabels[0] = rm.GetString("civRatios");
			RCLabels[1] = rm.GetString("civCustVal");
			RCLabels[2] = rm.GetString("civPeerVal");
			RCLabels[3] = rm.GetString("civVarSC");
			Justif[0] = "Left";
			Justif[1] = Justif[2] = Justif[3] = "Right";
			Utility.PrintColumnLabels(RG, RCLabels, true, Justif);
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "N/A");
			///CPF 07/13/06 Log 1716:  Changed zero string to print "" instead of "-"
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING_1, "");

			//Current Ratio
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civCurRatio"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("CURRENT_RATIO_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("CURRENT_RATIO_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("CURRENT_RATIO_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Quick Ratio
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civQuickRatio"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("QUICK_RATIO_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("QUICK_RATIO_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("QUICK_RATIO_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "0");
			//Net Trade Receivable Days
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civNetTrRecDays"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("NetTrRecDays_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("NetTrRecDays_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("NetTrRecDays_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Inventory Days
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civInvDays"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("InvDays_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("InvDays_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("InvDays_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Trade Payable Days
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civTrPayDays"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("TrPayDays_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("TrPayDays_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("TrPayDays_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			//Sales to Working Capital
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civSalesToWorkCap"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("SalesToWorkCap_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("SalesToWorkCap_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("SalesToWorkCap_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);

			//Interest Coverage
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civIntCov"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("IntCov_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("IntCov_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("IntCov_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//(Net P&L + D&A - Dvds)/CPLTD
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civNetPLtoCPLTD"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("civNetPLtoCPLTD_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("civNetPLtoCPLTD_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("civNetPLtoCPLTD_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Net Fixed Asset / TNW
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civNetFixAstsToTNW"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("NetFixAstsToTNW_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("NetFixAstsToTNW_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("NetFixAstsToTNW_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//% PBT / TNW
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civPBTtoTNW"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("PBTtoTNW_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("PBTtoTNW_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("PBTtoTNW_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//% PBT / Total Assets
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civPBTtoTotAsts"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("PBTtoTotAsts_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("PBTtoTotAsts_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("PBTtoTotAsts_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Sales to Net Fixed Assets
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civSalesToNetFAsts"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("SalesToNetFAsts_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("SalesToNetFAsts_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("SalesToNetFAsts_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Sales to Total Assets
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civSalesToTotAsts"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("SalesToTotAsts_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("SalesToTotAsts_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("SalesToTotAsts_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Depr & Amort / Sales(%)
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civDeprAmrtToSales"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("DeprAmrtToSales_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("DeprAmrtToSales_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("DeprAmrtToSales_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			
			//Officer Comp / Sales(%)
			Utility.mT.AddStartRow(Utility.nRow + 1);
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civOfCompToSales"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("OfCompToSales_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("OfCompToSales_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("OfCompToSales_Low"));
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			Utility.mT.AddEndRow(Utility.nRow);
			Utility.mT.AddEndRow(Utility.nRow);
			#endregion Ratios

			#region RC labels
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "0");
			Utility.CloseTable(RG);

			//Print underlined RISKCALC label
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			s0 = rm.GetString("civRCLabelUC");
			Utility.PrintParagraph(RG, s0);
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			s0 = "";
			//Skip a line
			Utility.PrintParagraph(RG, " ");

			// paragraph of text giving RiskCalc version, sample size
			//basically, we have added a call to a field in the RMA database that has not been created (123).  This
			//field will hopefully be used to store the RC Version in the future.  So here we check to see if the call
			//to RG.IND(123) returns anything other than 0.  If not, sub in 1.50.
			if (RG.IND(123) == 0)
				sVersion = "1.5";
			else
				sVersion = RG.IND(123).ToString();

			//Manage the end: company-companies depending on numbers
			if (RG.IND(113) == 1)
				s1 = rm.GetString("civSing");
			else
				s1 = rm.GetString("civPlur");

			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING, "0");
			s0 = string.Format(rm.GetString("civRC0"), sVersion, RG.IND(113), s1, rm.GetString("civRCLabelSC"))
				+ " " + string.Format(rm.GetString("civRC1"), rm.GetString("civRCLabelSC"), rm.GetString("civMenuRC"));
			Utility.PrintParagraph(RG, s0);
			s0 = "";

			//amit: Testing it out with Print Cell
			Utility.PrintParagraph(RG, "  ");
			Utility.PrintParagraph(RG, "  ");

            //0=left, 1=center 2= left
			//TO DO: format NaN to N/A
			
			///CPF 07/17/06 Log 1300:  Create new table to use print cell to space out the RC info.
			///The old code is commented out below.
			Utility.CreateTable(RG, 3, new float[]{2.3f, 2.03f, 2.3f});
			Utility.mT.LastRowAsHeader(Utility.nRow);

			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING, "-");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "");

			string s1YrUpp;
			if (double.IsNaN(RG.GetCalc("RC1Yr_Upp")[1])) 
				s1YrUpp = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC1Yr_Upp")[1] == 0)
				s1YrUpp = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s1YrUpp = RG.GetCalc("RC1Yr_Upp")[1].ToString("N1");

			string s1YrMed;
			if (double.IsNaN(RG.GetCalc("RC1Yr_Med")[1])) 
				s1YrMed = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC1Yr_Med")[1] == 0)
				s1YrMed = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s1YrMed = RG.GetCalc("RC1Yr_Med")[1].ToString("N1");

			string s1YrLow;
			if (double.IsNaN(RG.GetCalc("RC1Yr_Low")[1])) 
				s1YrLow = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC1Yr_Low")[1] == 0)
				s1YrLow = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s1YrLow = RG.GetCalc("RC1Yr_Low")[1].ToString("N1");

			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 1, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, rm.GetString("civPeerVal"), false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, rm.GetString("civ1Yr"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 0, false, "", false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civUpp"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s1YrUpp, false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civMed"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s1YrMed, false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civLow"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s1YrLow, false);
			Utility.nRow++;

			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 1, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 0, false, "", false);
			Utility.nRow++;

			string s5YrUpp;
			if (double.IsNaN(RG.GetCalc("RC5Yr_Upp")[1])) 
				s5YrUpp = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC5Yr_Upp")[1] == 0)
				s5YrUpp = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s5YrUpp = RG.GetCalc("RC5Yr_Upp")[1].ToString("N1");

			string s5YrMed;
			if (double.IsNaN(RG.GetCalc("RC5Yr_Med")[1])) 
				s5YrMed = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC5Yr_Med")[1] == 0)
				s5YrMed = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s5YrMed = RG.GetCalc("RC5Yr_Med")[1].ToString("N1");

			string s5YrLow;
			if (double.IsNaN(RG.GetCalc("RC5Yr_Low")[1])) 
				s5YrLow = RG.GetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1);
			else if (RG.GetCalc("RC5Yr_Low")[1] == 0)
				s5YrLow = RG.GetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING);
			else
				s5YrLow = RG.GetCalc("RC5Yr_Low")[1].ToString("N1");

			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, rm.GetString("civ5Yr"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 0, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 0, false, "", false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civUpp"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s5YrUpp, false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civMed"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s5YrMed, false);
			Utility.nRow++;
			Utility.PrintCell(RG, 0, Utility.nRow, 0, false, "   " + rm.GetString("civLow"), false);
			Utility.PrintCell(RG, 1, Utility.nRow, 2, false, "", false);
			Utility.PrintCell(RG, 2, Utility.nRow, 2, false, s5YrLow, false);
			Utility.nRow++;
			

			
			
			/*
			Utility.CreateTable(RG,4);
			//Section 3 Page Break Start: 1 - Yr. EDF
			Utility.mT.AddStartRow(Utility.nRow + 1);
			
			RG.SetAuthorSetting(FORMATCOMMANDS.ZEROS_STRING, "-");
			RG.SetAuthorSetting(FORMATCOMMANDS.ERR_STRING_1, "");
			RG.SetAuthorSetting(FORMATCOMMANDS.ROUND_FOR_REPORTS, "True");

			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "True");
			RCLabels[0] = "";
			RCLabels[1] = "";
			RCLabels[2] = rm.GetString("civPeerVal");
			RCLabels[3] = "";
			Justif[0] = Justif[1] = Justif[3] = Justif[2] = "Right";
			Utility.PrintColumnLabels(RG, RCLabels, false, Justif);
			RG.SetAuthorSetting(FORMATCOMMANDS.UNDERLINEON, "False");
			
			RG.SetAuthorSetting(FORMATCOMMANDS.DECIMAL_PRCSN_1, "1");
			// 1 Year EDF (no customer value, just peer values)
			//Print rows with values
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civ1Yr"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("RC1Yr_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("RC1Yr_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("RC1Yr_Low"));
			//Simply skip the line
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			//Section 3 Page Break End 
			Utility.mT.AddEndRow(Utility.nRow);


			//Section 4 Page Break Start: 5 - Yr. EDF
			Utility.mT.AddStartRow(Utility.nRow + 1);
			// 5 Year EDF (no customer value, just peer values)
			//Print rows with values
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "3");
			Utility.PrintLabel(RG, 3, rm.GetString("civ5Yr"));
			RG.SetAuthorSetting(FORMATCOMMANDS.INDENT, "7");
			Utility.PrintSummary(RG, rm.GetString("civUpp"), RG.GetCalc("RC5Yr_Upp"));
			Utility.PrintSummary(RG, rm.GetString("civMed"), RG.GetCalc("RC5Yr_Med"));
			Utility.PrintSummary(RG, rm.GetString("civLow"), RG.GetCalc("RC5Yr_Low"));
			//Simply skip the line
			Utility.Skip(RG, RG.GetCalc("ProfLossB4Tax").Count, 1);
			//Section 4 Page Break End
			Utility.mT.AddEndRow(Utility.nRow);
			*/
			#endregion RC labels

			Utility.CloseReport(RG);

		}
	}
}
