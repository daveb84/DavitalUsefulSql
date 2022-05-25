DECLARE @xml NVARCHAR(MAX) = '<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="accessLockOverride" value="12,0397d7a5-1511-436a-bf40-914927aeb70e;2920,fbafef0c-36b4-46b9-bcd7-3b6a758d8055;3028,43e55193-5102-4c01-941e-4ee87a9d3a3d;3157,b40bedee-19d5-414b-ba7c-6e18f8197048;3157,86ee2794-5994-4a0c-aa4e-1d0a111f5e87;3157,f97f226c-3b44-4338-b4a7-a9b6f270ed2e;3157,7fb7751b-066d-4fe3-9b08-9890a1d2b40c;3157,92c02f28-fd11-4c58-ad28-806b3fb2ab9a;3028,f414c214-83f4-4626-ada3-78a16b2c6394;4298,3bdc1ea6-119e-4b40-ae7b-7a52e8d5698a;4298,2ae93fd5-d0e6-46c7-b28a-6940f6db6462;4298,24413689-6feb-445f-868e-b9a3aaeec54f;4155,fd70dfd9-7737-434a-b5a2-168388f01a96;4155,0cbce4ae-e820-49ff-b2e8-bcc72ec5b20d;4298,de27b1f5-3a7e-4df5-8c02-776947c20075;3028,7082d40d-e443-476a-86de-cec077c96ac0;746,c4ac2167-14d2-4eab-9019-18b24b14b706;746,cbc3be7f-0ac6-44da-94fb-70e301257d35"/>
  </appSettings>
</configuration>'

SELECT dbo.GetXPathValue(@xml, '//configuration/appSettings/add[@key="accessLockOverride"]/@value')