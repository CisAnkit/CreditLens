# CreditLens Financial Templates extension 

This folder contains all customized Financial Templates for Financial Analysis module

## 1. System Date
* `SystemData.sdata` This file is the export system data from `CreditLens Studio` => `System Data Author`. Customization project might have some changes in this file. If not, just use the default one.

## 2. Financial Templates
* Each folder in this directory represents on customized financial template, with its .tpl file please directlly in the folder. DO NOT check all original release assemblies (`constants?.dll`/`macro?.dll`/`proxyMappings?.dll`/`CHBIFT.dll`). These files are generated automatically once you compile the project. And zip it as one `CHBIFT.zip` under upper level folder.
* For better working with new financial template compiler, customized report projects use new SDK format of csproj. For how to convert your existing csproj into new format, please check [CreditLens Financial Template Guide](https://erswiki.analytics.moodys.net/display/CAO/CreditLens+Financial+Template+Compiler+Guide)
* **DO NOT** check in output binaries into Git. Such as:
  * `constants2014.dll`
  * `macros2014.dll`
  * `proxyMappings2014.dll`
  * `CHBIFT.dll`
  * `zh-Hans\CHBIFT.resources.dll`
* **DO NOT** Generate data loads from `CreditLens Studio` and check-in here. Start from 19.31. As long as the Tenant zip contans a tpl file. CreditLens would install the financial template from tpl file.
