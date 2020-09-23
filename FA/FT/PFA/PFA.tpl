﻿<?xml version="1.0" encoding="utf-8"?>
<MODEL>
  <SAVED_IN_CULTURE>en-US</SAVED_IN_CULTURE>
  <MODEL_FILE_VERSION>4.1</MODEL_FILE_VERSION>
  <MODEL_VERSION MAJOR="6" MINOR="5" BUILD="14" PATCH="0" />
  <ALL_LANGUAGES />
  <GLOBAL_DATA_KEY>00000000-0000-0000-0000-000000000000</GLOBAL_DATA_KEY>
  <MODEL_INFO>
    <ID>11</ID>
    <CHECKINKEY>00000000-0000-0000-0000-000000000000</CHECKINKEY>
    <QAC>False</QAC>
    <CLASSHIDING>True</CLASSHIDING>
    <SPREADINGGUIDE>True</SPREADINGGUIDE>
    <SUPPORTSRATINGS>False</SUPPORTSRATINGS>
    <DISABLE_ST_PROJECTION>True</DISABLE_ST_PROJECTION>
    <DISABLE_LT_PROJECTION>False</DISABLE_LT_PROJECTION>
    <DISABLE_PEER_COMPARISON>True</DISABLE_PEER_COMPARISON>
    <DISABLE_CONSOLIDATIONS>True</DISABLE_CONSOLIDATIONS>
    <DEFAULTLANG>1</DEFAULTLANG>
    <STMTTYPE_ANNUAL_ID />
    <STMTTYPE_YEARTODATE_ID />
    <LANGUAGES>
      <LANG ID="1" />
    </LANGUAGES>
    <NAMES>
      <NAME ID="1">PFA</NAME>
    </NAMES>
    <DESCRIPTIONS>
      <DESCRIPTION ID="1">Personal Financial Analysis</DESCRIPTION>
    </DESCRIPTIONS>
  </MODEL_INFO>
  <CURRENCIES />
  <MODEL_ADDINS>
    <MODEL_ADDIN ID="1" NAME="RiskCalc" MAPPED="False" ENABLED="False" TYPE="4" />
    <MODEL_ADDIN ID="2" NAME="Reporting Table" MAPPED="True" ENABLED="True" TYPE="1" />
    <MODEL_ADDIN ID="3" NAME="Batch Reporting Table" MAPPED="True" ENABLED="True" TYPE="3" />
    <MODEL_ADDIN ID="4" NAME="Public EDF" MAPPED="False" ENABLED="False" TYPE="4" />
    <MODEL_ADDIN ID="5" NAME="Batch PFP Export" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="6" NAME="HOSP117 New" MAPPED="False" ENABLED="False" TYPE="5" />
    <MODEL_ADDIN ID="7" NAME="PFAAddIn" MAPPED="True" ENABLED="True" TYPE="1" />
    <MODEL_ADDIN ID="8" NAME="PFAAddIn Loans" MAPPED="True" ENABLED="True" TYPE="2" />
    <MODEL_ADDIN ID="9" NAME="PFAAddIn Schedules" MAPPED="True" ENABLED="True" TYPE="2" />
    <MODEL_ADDIN ID="11" NAME="Batch ArchiveRetrieval" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="12" NAME="Batch RMA Submission" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="13" NAME="BatchRatingsUpdate" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="14" NAME="HOSP117 Startup" MAPPED="False" ENABLED="False" TYPE="1" />
    <MODEL_ADDIN ID="20" NAME="Batch Archive" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="21" NAME="Entity Copy" MAPPED="True" ENABLED="True" TYPE="3" />
    <MODEL_ADDIN ID="22" NAME="Archive Conversion" MAPPED="False" ENABLED="False" TYPE="3" />
    <MODEL_ADDIN ID="23" NAME="ETL Mapping" MAPPED="False" ENABLED="False" TYPE="1" />
    <MODEL_ADDIN ID="24" NAME="Batch ETL Mapping" MAPPED="True" ENABLED="True" TYPE="3" />
    <MODEL_ADDIN ID="25" NAME="LGD Archive AddIn" MAPPED="False" ENABLED="False" TYPE="1" />
    <MODEL_ADDIN ID="26" NAME="AG Startup" MAPPED="False" ENABLED="False" TYPE="1" />
    <MODEL_ADDIN ID="27" NAME="AG New" MAPPED="False" ENABLED="False" TYPE="5" />
    <MODEL_ADDIN ID="28" NAME="Batch Testing" MAPPED="False" ENABLED="False" TYPE="3" />
  </MODEL_ADDINS>
  <MACRO_GROUPINGS />
  <UNKNOWN_MODEL_PROPERTIES>
    <UNKNOWN_MODEL_PROPERTY ID="GlobalCashFlowEnabled"><![CDATA[True]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="FirmTypeConst"><![CDATA[21]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="HideGlobalConstant"><![CDATA[2,3,4]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="SaveByStatus"><![CDATA[Completed,Reviewed]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="CurrencyConstID"><![CDATA[0]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="ConfigPrintNullAccountEnabled"><![CDATA[False]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="CustomerClasses"><![CDATA[1]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="CustomerStateConst"><![CDATA[44]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="StmtStatusApproved"><![CDATA[Reviewed]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="RestatedConstID"><![CDATA[7]]></UNKNOWN_MODEL_PROPERTY>
    <UNKNOWN_MODEL_PROPERTY ID="StatusStmtConst"><![CDATA[6]]></UNKNOWN_MODEL_PROPERTY>
  </UNKNOWN_MODEL_PROPERTIES>
  <MODEL_CONFIG><![CDATA[]]></MODEL_CONFIG>
  <RC_MODELS />
  <GLOBAL_CONSTANTS />
  <STMT_CONSTANTS>
    <STMT_CONSTANT ID="1" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="True">
      <STMT_CONST_LABEL ID="1">Pers F/S Date</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1"></STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">PersFSDate</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="2" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="False">
      <STMT_CONST_LABEL ID="1">Accountant</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1"></STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">Accountant</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="3" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="False">
      <STMT_CONST_LABEL ID="1">Analyst</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1"></STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">Analyst</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="4" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="True">
      <STMT_CONST_LABEL ID="1">Stmt Type</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1">&lt;as&gt;&lt;a id="Annual" v="Annual"/&gt;&lt;a id="Tax Return" v="Tax Return"/&gt;&lt;a id="Projection" v="Projection"/&gt;&lt;a id="Monthly" v="Monthly"/&gt;&lt;a id="Quarterly" v="Quarterly"/&gt;&lt;a id="FY-To-Date" v="FY-To-Date"/&gt;&lt;a id="Rolling Stmt" v="Rolling Stmt"/&gt;&lt;a id="" v=""/&gt;&lt;hd v="Annual"/&gt;&lt;pd v="Annual"/&gt;&lt;rscd v="Rolling Stmt"/&gt;&lt;/as&gt;</STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">StmtType</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="5" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="False">
      <STMT_CONST_LABEL ID="1">Credit Score Date</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1"></STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">CreditScoreDate</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="6" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="True">
      <STMT_CONST_LABEL ID="1">Status</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1">&lt;as&gt;&lt;a id="Draft" v="Draft"/&gt;&lt;a id="Completed" v="Completed"/&gt;&lt;a id="Rework" v="Rework"/&gt;&lt;a id="Reviewed" v="Reviewed"/&gt;&lt;hd v="Draft"/&gt;&lt;pd v="Draft"/&gt;&lt;rscd v="Draft"/&gt;&lt;/as&gt;</STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">StatusStmtConst</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_CONSTANT ID="7" HIDDEN="False" READONLY="False" LENGTH="30" REQUIRED="False" DISPLAY="True">
      <STMT_CONST_LABEL ID="1">Restated</STMT_CONST_LABEL>
      <STMT_CONST_ATTRIBUTE ID="1">&lt;as&gt;&lt;a id="" v=""/&gt;&lt;a id="Restated" v="Restated"/&gt;&lt;a id="Restated2" v="Restated - 2"/&gt;&lt;a id="Restated3" v="Restated - 3"/&gt;&lt;a id="Restated4" v="Restated - 4"/&gt;&lt;a id="Restated5" v="Restated - 5"/&gt;&lt;hd v=""/&gt;&lt;pd v=""/&gt;&lt;rscd v=""/&gt;&lt;/as&gt;</STMT_CONST_ATTRIBUTE>
      <STMT_CONSTANT_MACRO_DESC ID="1">Restated</STMT_CONSTANT_MACRO_DESC>
    </STMT_CONSTANT>
    <STMT_TYPE_STMT_CONST_ID>4</STMT_TYPE_STMT_CONST_ID>
    <ANALYST_STMT_CONST_ID>3</ANALYST_STMT_CONST_ID>
    <AUDIT_METHOD_STMT_CONST_ID>-1</AUDIT_METHOD_STMT_CONST_ID>
  </STMT_CONSTANTS>
  <DIVISIONS>
    <DIVISION ID="A" MAPPED="True">
      <DIVISION_LABEL ID="18">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">農業、林業和漁業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การเกษตรการป่าไม้และการประมง</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Agricultura, silvicultura y pesca</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Agricultura, Silvicultura e Pesca</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Сельское, лесное и рыбное хозяйство</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Agricultura, silvicultura y pesca</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Agricoltura, pesca e silvicoltura</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">חקלאות, יערנות ודיג</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Tarım, Ormancılık ve Balıkçılık</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Agriculture, sylviculture et pisciculture</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">农业、林业和渔业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Agriculture, sylviculture et pêche</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Agriculture, Forestry and Fisheries</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="B" MAPPED="True">
      <DIVISION_LABEL ID="18">Mineral Industries</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Mineral Industries</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Mineral Industries</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Mineral Industries</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">礦業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">อุตสาหกรรมแร่</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Minería</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Indústrias Minerais</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Добывающая промышленность</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Minería</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Industrie minerali</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">תעשיות מחצבים</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Maden Sanayisi</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Industries minières</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">采矿业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Industries minières</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Mineral Industries</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Mineral Industries</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="C" MAPPED="True">
      <DIVISION_LABEL ID="18">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">建築業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การก่อสร้าง</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Construcción</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Construção</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Строительство</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Construcción</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Costruzione</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">בנייה</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">İnşaat</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">建筑业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Bâtiment</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Construction</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Construction</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="D" MAPPED="True">
      <DIVISION_LABEL ID="18">Manufacturing</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Manufacturing</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Manufacturing</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Manufacturing</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">製造業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การผลิต</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Manufacturación</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Fabricação</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Производство</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Manufacturación</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Produzione</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">ייצור</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">İmalat</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Fabrication</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">制造业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Fabrication</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Manufacturing</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Manufacturing</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="E" MAPPED="True">
      <DIVISION_LABEL ID="18">Transportation, Communication and Utilities</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Transportation, Communication and Utilities</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Transportation, Communication and Utilities</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Transportation, Communication and Utilities</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">運輸業、通訊業和公用事業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การคมนาคมและสาธารณูปโภค</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Transporte, comunicación y utilidades</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Transporte, Comunicação e Serviços Públicos</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Транспорт, связь и коммунальное хозяйство</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Transporte, comunicación y utilidades</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Trasporti, comunicazioni e servizi pubblici</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">תובלה, תקשורת ושירותים לציבור</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Taşımacılık, İletişim ve Altyapı Hizmetleri</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Transport, communications et services publics</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">运输、通信和公用事业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Transport, communications et services publics</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Transportation, Communication and Utilities</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Transportation, Communication and Utilities</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="F" MAPPED="True">
      <DIVISION_LABEL ID="18">Wholesale Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Wholesale Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Wholesale Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Wholesale Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">批發業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การค้าส่ง</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Comercio mayorista</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Comércio de Atacado</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Оптовая торговля</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Comercio mayorista</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Commercio all’ingrosso</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">מסחר סיטונאי</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Toptan Ticaret</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Commerce de gros</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">批发贸易</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Commerce de gros</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Wholesale Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Wholesale Trade</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="G" MAPPED="True">
      <DIVISION_LABEL ID="18">Retail Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Retail Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Retail Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Retail Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">零售業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การค้าปลีก</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Comercio minorista</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Comércio de Varejo</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Розничная торговля</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Comercio minorista</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Commercio al dettaglio</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">מסחר קימעונאי</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Perakende Ticaret</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Commerce de détail</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">零售贸易</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Commerce de détail</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Retail Trade</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Retail Trade</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="H" MAPPED="True">
      <DIVISION_LABEL ID="18">Finance, Insurance and Real Estate</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Finance, Insurance and Real Estate</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Finance, Insurance and Real Estate</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Finance, Insurance and Real Estate</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">金融業、保險業和房地產業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การเงินประกันภัยและอสังหาริมทรัพย์</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Finanzas, seguros e inmobiliarias</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Finanças, Seguros e Imóveis</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Финансы, страхование и недвижимость</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Finanzas, seguros e inmobiliarias</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Assicurazioni, finanze e immobiliare</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">כספים, ביטוח ונדל"ן</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Finansman, Sigorta ve Gayrımenkul</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Finances, assurances et immobilier</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">金融、保险和房地产</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Finances, assurances et immobilier</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Finance, Insurance and Real Estate</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Finance, Insurance and Real Estate</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="I" MAPPED="True">
      <DIVISION_LABEL ID="18">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">服務業</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">บริการ</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Servicios</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Serviços</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Услуги</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Servicios</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Servizi</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">שירותים</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Hizmetler</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">服务业</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Services</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Services</DIVISION_LABEL>
    </DIVISION>
    <DIVISION ID="Z" MAPPED="True">
      <DIVISION_LABEL ID="18">Public Administration</DIVISION_LABEL>
      <DIVISION_LABEL ID="17">Public Administration</DIVISION_LABEL>
      <DIVISION_LABEL ID="16">Public Administration</DIVISION_LABEL>
      <DIVISION_LABEL ID="15">Public Administration</DIVISION_LABEL>
      <DIVISION_LABEL ID="14">公共系統管理</DIVISION_LABEL>
      <DIVISION_LABEL ID="13">การบริหารรัฐกิจ</DIVISION_LABEL>
      <DIVISION_LABEL ID="12">Administración pública</DIVISION_LABEL>
      <DIVISION_LABEL ID="11">Administração Pública</DIVISION_LABEL>
      <DIVISION_LABEL ID="10">Госуд административные органы</DIVISION_LABEL>
      <DIVISION_LABEL ID="9">Administración pública</DIVISION_LABEL>
      <DIVISION_LABEL ID="8">Pubblica amministrazione</DIVISION_LABEL>
      <DIVISION_LABEL ID="7">מינהל ציבורי</DIVISION_LABEL>
      <DIVISION_LABEL ID="6">Kamu Yönetimi</DIVISION_LABEL>
      <DIVISION_LABEL ID="5">Administration publique</DIVISION_LABEL>
      <DIVISION_LABEL ID="4">公共行政部门</DIVISION_LABEL>
      <DIVISION_LABEL ID="3">Administration publique</DIVISION_LABEL>
      <DIVISION_LABEL ID="2">Public Administration</DIVISION_LABEL>
      <DIVISION_LABEL ID="1">Public Administration</DIVISION_LABEL>
    </DIVISION>
  </DIVISIONS>
  <ENTRY_METHODS />
  <TYPES />
  <FLOWS />
  <CLASSES />
  <CONSTANTS />
  <ACCOUNTS />
  <PROJ_TYPES />
  <PARAMS />
  <NAME_OBJECTS />
  <LITERALS />
  <VALUES />
  <REPORT_TYPES />
  <REPORTS />
  <GROUPS />
  <SUBCONTEXTS />
  <CONTEXTS />
  <SUBTOTALS />
  <SYSTEM_MACROS />
  <MACROS />
  <ASSIGNS />
  <UP_SETTINGS>
    <NUM_ANNUAL_STMTS>0</NUM_ANNUAL_STMTS>
    <NUM_INTERIM_STMTS>0</NUM_INTERIM_STMTS>
    <NUM_COMBINED_STMTS>0</NUM_COMBINED_STMTS>
    <NUM_LTP_STMTS>0</NUM_LTP_STMTS>
    <NUM_STP_STMTS>0</NUM_STP_STMTS>
    <NUM_COMBINED_PROJ_STMTS>0</NUM_COMBINED_PROJ_STMTS>
    <UP_ROUNDING>0</UP_ROUNDING>
    <UP_USE_HIDDEN_STMTS>False</UP_USE_HIDDEN_STMTS>
    <UP_INCLUDE_RC>False</UP_INCLUDE_RC>
    <UP_FORCE_RECALC>False</UP_FORCE_RECALC>
    <UP_RUNIFRCFAIL>False</UP_RUNIFRCFAIL>
    <UP_DELETEONFAILURE>False</UP_DELETEONFAILURE>
    <UP_INCLUDEPEERANALYSIS>False</UP_INCLUDEPEERANALYSIS>
    <UP_RUNONSAVE>False</UP_RUNONSAVE>
  </UP_SETTINGS>
  <UP_CONDITIONS />
  <UP_KEYS />
  <UP_MAPPINGS />
  <INDUSTRY_CLASSIFICATION_SYSTEMS />
  <SENSITIVITY_PARAMETERS />
  <CURRENCY_SETTINGS>
    <SOURCE_CURRENCY_CONSTANT_ID>3</SOURCE_CURRENCY_CONSTANT_ID>
    <TARGET_CURRENCY_CONSTANT_ID>4</TARGET_CURRENCY_CONSTANT_ID>
    <UP_CURRENCY_BASIS />
    <SOURCE_CURRENCY_REQUIREMENT>0</SOURCE_CURRENCY_REQUIREMENT>
    <SOURCE_CURRRENCY_DEFAULT />
  </CURRENCY_SETTINGS>
  <REQ_FIELDS />
  <PFPEXPORT_MAPPINGS />
  <PROJ_TYPE_HISTORY />
  <ROLLING_STMT_SETTINGS>
    <MULTIPLY_BS>False</MULTIPLY_BS>
    <SHOW_MULTIPLY_COLUMN>False</SHOW_MULTIPLY_COLUMN>
    <MULTIPLY_STATEMENTS>False</MULTIPLY_STATEMENTS>
    <RETAIN_PROJ_BS_BALANCES>True</RETAIN_PROJ_BS_BALANCES>
    <MULTIPLIER_IS_ACCT>False</MULTIPLIER_IS_ACCT>
    <MULTIPLIER>1</MULTIPLIER>
    <SUMMED_ITEMS>
      <SUMMED_ITEM TYPE="1" ID="60" />
      <SUMMED_ITEM TYPE="1" ID="61" />
      <SUMMED_ITEM TYPE="1" ID="62" />
      <SUMMED_ITEM TYPE="1" ID="63" />
      <SUMMED_ITEM TYPE="1" ID="64" />
      <SUMMED_ITEM TYPE="1" ID="65" />
      <SUMMED_ITEM TYPE="1" ID="66" />
      <SUMMED_ITEM TYPE="1" ID="67" />
      <SUMMED_ITEM TYPE="1" ID="68" />
      <SUMMED_ITEM TYPE="1" ID="69" />
      <SUMMED_ITEM TYPE="1" ID="70" />
      <SUMMED_ITEM TYPE="1" ID="71" />
      <SUMMED_ITEM TYPE="1" ID="72" />
      <SUMMED_ITEM TYPE="1" ID="73" />
      <SUMMED_ITEM TYPE="1" ID="74" />
      <SUMMED_ITEM TYPE="1" ID="75" />
      <SUMMED_ITEM TYPE="1" ID="76" />
      <SUMMED_ITEM TYPE="1" ID="77" />
      <SUMMED_ITEM TYPE="1" ID="78" />
      <SUMMED_ITEM TYPE="1" ID="79" />
      <SUMMED_ITEM TYPE="1" ID="82" />
      <SUMMED_ITEM TYPE="1" ID="83" />
      <SUMMED_ITEM TYPE="1" ID="85" />
      <SUMMED_ITEM TYPE="1" ID="10" />
      <SUMMED_ITEM TYPE="1" ID="11" />
      <SUMMED_ITEM TYPE="1" ID="12" />
      <SUMMED_ITEM TYPE="1" ID="15" />
      <SUMMED_ITEM TYPE="1" ID="16" />
      <SUMMED_ITEM TYPE="1" ID="17" />
      <SUMMED_ITEM TYPE="1" ID="18" />
      <SUMMED_ITEM TYPE="1" ID="19" />
      <SUMMED_ITEM TYPE="1" ID="20" />
      <SUMMED_ITEM TYPE="1" ID="21" />
      <SUMMED_ITEM TYPE="1" ID="22" />
      <SUMMED_ITEM TYPE="1" ID="23" />
      <SUMMED_ITEM TYPE="1" ID="24" />
      <SUMMED_ITEM TYPE="1" ID="25" />
      <SUMMED_ITEM TYPE="1" ID="35" />
      <SUMMED_ITEM TYPE="1" ID="36" />
      <SUMMED_ITEM TYPE="1" ID="37" />
      <SUMMED_ITEM TYPE="1" ID="38" />
      <SUMMED_ITEM TYPE="1" ID="39" />
      <SUMMED_ITEM TYPE="1" ID="40" />
      <SUMMED_ITEM TYPE="1" ID="41" />
      <SUMMED_ITEM TYPE="1" ID="42" />
      <SUMMED_ITEM TYPE="1" ID="43" />
      <SUMMED_ITEM TYPE="1" ID="44" />
      <SUMMED_ITEM TYPE="1" ID="45" />
      <SUMMED_ITEM TYPE="1" ID="46" />
      <SUMMED_ITEM TYPE="1" ID="47" />
      <SUMMED_ITEM TYPE="1" ID="48" />
      <SUMMED_ITEM TYPE="1" ID="49" />
      <SUMMED_ITEM TYPE="1" ID="50" />
      <SUMMED_ITEM TYPE="1" ID="51" />
      <SUMMED_ITEM TYPE="1" ID="52" />
      <SUMMED_ITEM TYPE="1" ID="53" />
      <SUMMED_ITEM TYPE="1" ID="54" />
      <SUMMED_ITEM TYPE="1" ID="55" />
      <SUMMED_ITEM TYPE="1" ID="56" />
      <SUMMED_ITEM TYPE="1" ID="57" />
      <SUMMED_ITEM TYPE="1" ID="58" />
      <SUMMED_ITEM TYPE="1" ID="59" />
    </SUMMED_ITEMS>
    <VALUE_ITEMS />
  </ROLLING_STMT_SETTINGS>
  <RMA_MAPPINGS />
  <RA_MAPPINGS />
  <RMA_CONDITIONS />
  <SYSTEM_RA_MAPPINGS />
  <ORG_MACROS />
</MODEL>