namespace rec JetBrains.ReSharper.Plugins.FSharp.Tests.Features

open System
open System.IO
open System.Linq
open JetBrains.Application.Components
open JetBrains.ProjectModel
open JetBrains.ReSharper.Feature.Services.Daemon
open JetBrains.ReSharper.FeaturesTestFramework.Daemon
open JetBrains.ReSharper.Plugins.FSharp.Common.Checker
open JetBrains.ReSharper.Plugins.FSharp.Common.Shim.AssemblyReader
open JetBrains.ReSharper.Plugins.FSharp.Daemon.Highlightings
open JetBrains.ReSharper.Plugins.FSharp.ProjectModelBase
open JetBrains.ReSharper.Plugins.FSharp.Tests
open JetBrains.ReSharper.Psi
open JetBrains.ReSharper.Psi.Modules
open JetBrains.Util
open NUnit.Framework

[<FSharpTest>]
type AssemblyReaderTest() =
    inherit TestWithTwoProjects()

    override x.RelativeTestDataPath = "common/assemblyReaderShim"

    override x.MainFileExtension = FSharpProjectFileType.FsExtension
    override x.SecondFileExtension = CSharpProjectFileType.CS_EXTENSION

    member x.SecondProjectOutput = x.SecondProject.Location / (x.SecondProjectName + ".dll")

    [<Test>] member x.``Types 01 - Global namespace``() = x.DoNamedTest()
    [<Test>] member x.``Types 02 - Namespace``() = x.DoNamedTest()
    [<Test>] member x.``Types 03 - Enum``() = x.DoNamedTest()
    [<Test>] member x.``Types 04 - Struct``() = x.DoNamedTest()
    [<Test>] member x.``Types 05 - Interface``() = x.DoNamedTest()
    [<Test>] member x.``Types 06 - Nested``() = x.DoNamedTest()
    [<Test>] member x.``Types 07 - Nested, namespace``() = x.DoNamedTest()
    [<Test>] member x.``Types 08 - Generic``() = x.DoNamedTest()
    [<Test>] member x.``Types 09 - Generic nested``() = x.DoNamedTest()
    [<Test>] member x.``Types 10 - Abstract``() = x.DoNamedTest()
    [<Test>] member x.``Types 11 - Sealed``() = x.DoNamedTest()
    [<Test>] member x.``Types 12 - Static``() = x.DoNamedTest()

    [<Test>] member x.``Constructors 01 - Default``() = x.DoNamedTest()
    [<Test>] member x.``Constructors 02 - Predefined param type``() = x.DoNamedTest()
    [<Test>] member x.``Constructors 03 - Overloads``() = x.DoNamedTest()

    [<Test>] member x.``Fields 01 - Simple Field``() = x.DoNamedTest()
    [<Test>] member x.``Fields 02 - Self type``() = x.DoNamedTest()
    [<Test>] member x.``Fields 03 - Static``() = x.DoNamedTest()
    [<Test>] member x.``Fields 04 - Readonly``() = x.DoNamedTest()
    [<Test>] member x.``Fields 05 - Const``() = x.DoNamedTest()

    override x.DoTest(project: IProject, secondProject: IProject) =
        let shouldHighlight (highlighting: IHighlighting) =
            highlighting :? FSharpErrorHighlightingBase

        x.ShellInstance.GetComponent<FSharpCheckerService>().Checker.StopBackgroundCompile()
        x.Solution.GetPsiServices().Files.CommitAllDocuments()

        x.ExecuteWithGold(fun writer ->
            let projectFile = project.GetAllProjectFiles() |> Seq.exactlyOne
            let sourceFile = projectFile.ToSourceFiles().Single()

            let daemon = TestHighlightingDumper(sourceFile, writer, null, (fun hl _ _ -> shouldHighlight hl))
            daemon.DoHighlighting(DaemonProcessKind.VISIBLE_DOCUMENT)
            daemon.Dump()) |> ignore

    member x.ExecuteWithGold(action: Action<TextWriter>) =
        base.ExecuteWithGold(action) |> ignore

    override x.DoTest(project: IProject) =
        let projectByOutput = x.Solution.GetComponent<IProjectsByOutput>() :?> ProjectsByOutputStub
        projectByOutput.ProjectOutput <- x.SecondProjectOutput
        projectByOutput.Project <- x.SecondProject

        let referencesProvider = x.Solution.GetComponent<ITestReferencesProvider>()
        referencesProvider.References <- [| x.SecondProjectOutput |]

        base.DoTest(project)

        projectByOutput.ProjectOutput <- null
        projectByOutput.Project <- null
        referencesProvider.References <- [| |]


[<SolutionComponent>]
type ProjectsByOutputStub() =
    member val ProjectOutput: FileSystemPath = null with get, set
    member val Project: IProject = null with get, set

    interface IHideImplementation<ProjectsByOutput>

    interface IProjectsByOutput with
        member x.GetProjectPsiModuleByOutputAssembly(path) =
            if path <> x.ProjectOutput then null else
            x.Project.GetPsiModules().First()