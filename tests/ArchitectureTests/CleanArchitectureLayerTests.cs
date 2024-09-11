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

    [Fact]
    public void ArchitecturePlantUmlCheck()
    {
        IArchRule architecture = Types().Should().AdhereToPlantUmlDiagram("Architecture.puml");
        architecture.Check(Architecture);
    }
}