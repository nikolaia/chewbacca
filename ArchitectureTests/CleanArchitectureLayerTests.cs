namespace ArchitectureTests;

public class CleanArchitectureLayerTests
{
    // TIP: load your architecture once at the start to maximize performance of your tests
    private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
        System.Reflection.Assembly.Load("ApplicationCore"),
        System.Reflection.Assembly.Load("Infrastructure"),
        System.Reflection.Assembly.Load("Web"),
        System.Reflection.Assembly.Load("Shared")
    ).Build();

    private readonly IObjectProvider<IType> ApplicationCoreLayer =
        Types().That().ResideInAssembly("ApplicationCore").As("Application Core Layer");

    private readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInNamespace("Infrastructure").As("Infrastructure Layer");

    private readonly IObjectProvider<IType> WebLayer =
        Types().That().ResideInNamespace("Web").As("Web Layer");
    
    private readonly IObjectProvider<IType> SharedLayer =
        Types().That().ResideInNamespace("Shared").As("Shared Layer");
    
    [Fact]
    public void ApplicationCoreLayerShouldNotAccessWebLayer()
    {
        IArchRule applicationCoreLayerShouldNotAccessWebLayer = Types().That().Are(ApplicationCoreLayer).Should()
            .NotDependOnAny(WebLayer).Because("The ApplicationCore project should not depend on the Web project.");
        applicationCoreLayerShouldNotAccessWebLayer.Check(Architecture);
    }
    
    [Fact]
    public void ApplicationCoreLayerShouldNotAccessInfrastructureLayer()
    {
        IArchRule applicationCoreLayerShouldNotAccessWebLayer = Types().That().Are(ApplicationCoreLayer).Should()
            .NotDependOnAny(InfrastructureLayer).Because("The ApplicationCore project should not depend on the Web project.");
        applicationCoreLayerShouldNotAccessWebLayer.Check(Architecture);
    }
    
    [Fact]
    public void InfrastructureLayerShouldNotAccessWebLayer()
    {
        IArchRule infrastructureLayerShouldNotAccessWebLayer = Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(WebLayer).Because("The Infrastructure project should not depend on the Web project.");
        infrastructureLayerShouldNotAccessWebLayer.Check(Architecture);
    }
    
    [Fact]
    public void SharedLayerShouldNotAccessAnyLayer()
    {
        IArchRule sharedLayerShouldNotAccessWebLayer = Types().That().Are(SharedLayer).Should()
            .NotDependOnAny(WebLayer).Because("The Shared project should not depend on the Web project.");
        sharedLayerShouldNotAccessWebLayer.Check(Architecture);
        
        IArchRule sharedLayerShouldNotAccessInfrastructureLayer = Types().That().Are(SharedLayer).Should()
            .NotDependOnAny(InfrastructureLayer).Because("The Shared project should not depend on the Web project.");
        sharedLayerShouldNotAccessInfrastructureLayer.Check(Architecture);
        
        IArchRule sharedLayerShouldNotAccessApplicationCoreLayer = Types().That().Are(SharedLayer).Should()
            .NotDependOnAny(ApplicationCoreLayer).Because("The Shared project should not depend on the Web project.");
        sharedLayerShouldNotAccessApplicationCoreLayer .Check(Architecture);
    }
}