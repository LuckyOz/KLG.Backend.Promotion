using KLG.Backend.Promotion.Services.Entities;
using KLG.Library.Microservice.DataAccess;

namespace KLG.Backend.Promotion.Services.Business.Companies;

public class HeadcountManager
{
    private IKLGDbProvider<DefaultDbContext> _dbProvider;

    public HeadcountManager(IKLGDbProvider<DefaultDbContext> dbProvider)
    {
        _dbProvider = dbProvider;
    }

    /// <summary>
    /// Recruits a new employee and updates the total employee count.
    /// </summary>
    public async Task RecruitNewEmployee()
    {
        await UpdateTotalEmployee(1);
    }

    /// <summary>
    /// Handles an employee resignation and updates the total employee count.
    /// </summary>
    public async Task EmployeeResignation()
    {
        await UpdateTotalEmployee(-1);
    }

    private async Task UpdateTotalEmployee(int n)
    {
        // Retrieve the company from the database
        var company = _dbProvider.DbContext.Company.FirstOrDefault();

        if (company == null)
        {
            // Create a new company if it doesn't exist yet
            company = new Company() { Id = Guid.NewGuid().ToString(), Name = "MyCompany", TotalEmployee = 1 };
            _dbProvider.DbContext.Company.Add(company);
        }
        else
        {
            // Update the total employee count by adding or reducing
            company.TotalEmployee += n;
        }

        // Save the changes to the database
        await _dbProvider.DbContext.SaveChangesAsync();
    }
}
