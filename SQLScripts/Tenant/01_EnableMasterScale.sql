UPDATE tenant.riskgradingsystemsetting
SET jsondoc_ = jsonb_set(jsondoc_, '{EnableMasterScale}', '"true"')
WHERE islatestversion_ = true;