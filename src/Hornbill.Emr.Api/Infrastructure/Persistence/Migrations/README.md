## Steps to create a new migration file
- Right click on the `HornBill.Emr.Api` project.
- From the context menu, select `Open in Terminal` option.
- Execute the below command in the terminal to create a new migration file. 
<br>
`dotnet ef migrations add <migration-name> --startup-project ..\HornBill.Emr.Api\HornBill.Emr.Api.csproj -o .\Infrastructure\Persistence\Migrations`

> Replace the `<migration-name>` placeholder with a meaningful migration name.

## Steps to apply the migrations to the database
- Right click on the `HornBill.Emr.Api` project.
- From the context menu, select `Open in Terminal` option.
- Execute the below command in the terminal to create a new migration file.
<br>
`dotnet ef database update --startup-project ..\HornBill.Emr.Api\HornBill.Emr.Api.csproj`