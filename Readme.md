---MIT.ECSR.IDENTITY


##Powershell Infrastructure
dotnet ef dbcontext scaffold "Data Source=mediaindoteknologi.com,5031; Initial Catalog=MIT.CSR; User Id=sa;Password=antapani@1b" Microsoft.EntityFrameworkCore.SqlServer --output-dir "..\MIT.ECSR.Data\Model" -c ApplicationDBContext --context-dir "..\MIT.ECSR.Data" --namespace "MIT.ECSR.Data.Model" --context-namespace "MIT.ECSR.Data" --no-pluralize -f --no-onconfiguring --schema "dbo"
