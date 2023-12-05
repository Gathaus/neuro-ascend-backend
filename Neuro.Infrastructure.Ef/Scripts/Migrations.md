# Database Migrations

## Applying Migrations

### ApplicationDbContext Migrations
1. **Creating Migrations**

   ```bash
   dotnet ef migrations add InitialCreate --project .\Neuro.Infrastructure.Ef --startup-project .\Neuro.Api --context ApplicationDbContext -v
2. **Updating the Database**
    
   ```bash
    dotnet ef database update --project .\Neuro.Infrastructure.Ef --startup-project .\Neuro.Api --context ApplicationDbContext -v
