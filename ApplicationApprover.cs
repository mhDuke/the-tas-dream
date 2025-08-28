using Ardalis.SmartEnum;

namespace TAS;

public class ApplicationApprover(IRepo repo, IApplicationEventNotifier notifier, IServiceProvider sp)
{

    public async Task<ApprovalResponse> Approve(long applicationId)
    {
        var app = await repo.Get(applicationId);

        //validate approval permission requirements.
        //validate approval business requirements.
        var aprovalResponse = await ExecuteApprove(app);
        
        await notifier.Progressed(app);
        return aprovalResponse;
    }
    private Task<ApprovalResponse> ExecuteApprove(DariApp app)
    {
        // update application state in data store.
        //
        return Task.FromResult(new ApprovalResponse());
    }
}

public interface IRepo 
{
    Task<DariApp> Get(long applicationId);
}
public interface IApplicationEventNotifier
{
    Task Progressed(DariApp app); //moved a step forward
    Task FeesPaid(DariApp app);
    Task Completed(DariApp app);
    Task Picked(DariApp app);
    Task Rejected(DariApp app, Reason reason);
    Task Returned(DariApp app, Reason reason);
    Task Cancelled(DariApp app);
}
public class DariApp;
public sealed record ApprovalResponse;
public sealed record Reason(int Id, string NameEn, string NameAr, string? Remarks);
//public class ApplicationValidator

public class SubmitFinancialSummaryApplicationDefinition
{
    public string[] Stages = ["DRAFT", "ASSIGNED_TO_AUDITOR", ""];
}

public class ApplicationStage : SmartEnum<ApplicationStage, string>
{
    //each stage may require approval permission
    private ApplicationStage(string name, string value, DariPermission progressPermission) : base(name, value)
    {
        ProgressPermission = progressPermission;
    }

    public static readonly ApplicationStage Draft = new("DRAFT", "DRAFT", DariPermission.SubmitApplication);
    public static readonly ApplicationStage SubmittedToAuditor = new("DRAFT", "DRAFT", DariPermission.SubmitApplication);

    public DariPermission ProgressPermission { get; }
}

public class DariPermission : SmartEnum<DariPermission, string>
{
    private DariPermission(string name, string value) : base(name, value) { }
    public static readonly DariPermission SubmitApplication = new("SubmitApplication", "dari permission id i.e. 1142");
    public static readonly DariPermission POA = new("POA", "POA");
    //public static readonly ApplicationState Initiate = new("POA", "POA");
}
