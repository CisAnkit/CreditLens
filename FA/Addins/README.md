# CreditLens Financial Templates Addins Extension

This folder contains all customized Financial Templates Addin projects

## 1. Referencing to customize financial template assemblies
* When references needed from addin project to customized financial template assemblies. **DO NOT** check-in output DLLs to repository. You can add links to your tpl file and SystemData.sdata. Then add `Moodys.ERS.CreditLens.Compiler` as NuGet references. It would generate the assemblies and provided to you project automatically.
