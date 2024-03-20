using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;

namespace RiverBooks.OrderProcessing.Tests;

public class InfrastructureDependencyTests
{
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(typeof(AssemblyInfo).Assembly).Build();

    [Fact]
    public void DomainTypesShouldNotReferenceInfrastructure()
    {
        var domainTypes = ArchRuleDefinition.Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Domain.*", useRegularExpressions: true)
            .As("OrderProcessing Domain Types");
        var infrastructureTypes = ArchRuleDefinition.Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Infrastructure.*", useRegularExpressions: true)
            .As("OrderProcessing Infrastructure Types");

        var rule = domainTypes.Should().NotDependOnAny(infrastructureTypes);
        rule.Check(Architecture);
    }
}