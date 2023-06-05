
using Microsoft.EntityFrameworkCore;
using KLG.Library.Microservice.DataAccess;

namespace KLG.Backend.Promotion.Services.Entities;

public class DefaultDbContext : KLGDbContext
{
    public DefaultDbContext() { }

    public DefaultDbContext(DbContextOptions opt) : base(opt) { }

    public DbSet<PromoWorkflow> PromoWorkflow { get; set; }
    public DbSet<PromoWorkflowExpression> PromoWorkflowExpression { get; set; }
    public DbSet<PromoRule> PromoRule { get; set; }
    public DbSet<PromoRuleExpression> PromoRuleExpression { get; set; }
    public DbSet<PromoRuleResult> PromoRuleResult { get; set; }
    public DbSet<PromoRuleVariable> PromoRuleVariable { get; set; }
}
