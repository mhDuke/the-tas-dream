using Ardalis.SmartEnum;

namespace TAS;
public class Worflows
{
    public ApplicationWorkflowBlueprint ProjectPlan()
    {
        return new ApplicationWorkflowBlueprint
        {
            ApplicationType = ApplicationType.ProjectPlanRequest,
            InitiationRequirements = [ApplicationRequirement.CompanyActivity(PredefinedCompanyActivity.FacilitiesManagementServices), ApplicationRequirement.CompanyActivity(PredefinedCompanyActivity.RealEstateLeaseAndManagementService), ApplicationRequirement.Permission()],
            Stages =
            [
                new Stage { ReferenceNumber = 1, NameEn = "Draft", NameAr = "تحت الإنشاء", OnlineId = "DRAFT", AdminId = "DRAFT", ActorType = TheApplicationPartyType.DEVELOPER, Requirements = [ApplicationRequirement.Permission()],
                    Actions =
                    [
                        StageAction.Submit(2),
                        StageAction.Submit(3, [ApplicationRequirement.Permission()], "submit_skipping_checker_approval"),
                    ]},
                new Stage { ReferenceNumber = 2, NameEn = "Checker Approval", NameAr = "إعتماد المراجع", OnlineId = "DEVELOPER_SIGNATORY_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.DEVELOPER, Requirements = [ApplicationRequirement.Permission()],
                    Actions =
                    [
                        StageAction.Approve(3),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 3, NameEn = "Surveyor Approval", NameAr = "إعتماد شركة المساحة", OnlineId = "SURVEYOR_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(4, [ApplicationRequirement.Permission()]),
                        StageAction.Approve(5, [ApplicationRequirement.POA()], "approve_skipping_surveyor_final_approval"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 4, NameEn = "Surveyor Final Approval", NameAr = "إعتماد شركة المساحة النهائي", OnlineId = "SURVEYOR_FINAL_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(5, [ApplicationRequirement.POA()]),
                        StageAction.Return(ApplicationRequirement.POA()),
                        StageAction.Reject(ApplicationRequirement.POA()),
                    ]},
                new Stage { ReferenceNumber = 5, NameEn = "ADREC Review", NameAr = "مراجعة البلدية", OnlineId = "DMT_REVIEW", AdminId = "SUBMITTED", ActorType = TheApplicationPartyType.ADREC },
                new Stage { ReferenceNumber = 6, NameEn = "ADREC Audit", NameAr = "تحت تدقيق البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_AUDIT", ActorType = TheApplicationPartyType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(7),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 7, NameEn = "Audited", NameAr = "تمت المراجعة", OnlineId = "DMT_REVIEW", AdminId = "AUDITED", ActorType = TheApplicationPartyType.ADREC},
                new Stage { ReferenceNumber = 8, NameEn = "ADREC Approval", NameAr = "اعتماد البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_APPROVAL", ActorType = TheApplicationPartyType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(9),
                        StageAction.Approve(10, id: "approve_skipping_fees"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 9, NameEn = "Pending Payment", NameAr = "في انتظار الدفع", OnlineId = "DARI_PENDING_PAYMENT", AdminId = "APPROVED", ActorType = TheApplicationPartyType.CMC},
                new Stage { ReferenceNumber = 10, NameEn = "Certificate Generation", NameAr = "استخراج الشهادة", OnlineId = "GENERATE_CERTIFICATE", AdminId = "APPROVED", ActorType = TheApplicationPartyType.SYSTEM},
                new Stage { ReferenceNumber = 11, NameEn = "Completed", NameAr = "مكتمل", OnlineId = "COMPLETED", AdminId = "COMPLETED", ActorType = TheApplicationPartyType.SYSTEM, IsFinal = true},
            ],
        };
    }
    public ApplicationWorkflowBlueprint CommunityModification()
    {
        return new ApplicationWorkflowBlueprint
        {
            ApplicationType = ApplicationType.ModifyMainCommunityRequest, // add ServiceId to ApplicationType for example 1562 for Modify Main Community For Jointly Owned Properties. all OA services under GroupId 1017. these are dari user permission specific information.
            InitiationRequirements = [PredefinedCompanyActivity.FacilitiesManagementServices, PredefinedCompanyActivity.RealEstateLeaseAndManagementService, ApplicationRequirement.Permission(6510)],
            Stages =
            [
                new Stage { ReferenceNumber = 1, NameEn = "Draft", NameAr = "تحت الإنشاء", OnlineId = "DRAFT", AdminId = "DRAFT", ActorType = TheApplicationPartyType.CMC,
                    Actions =
                    [
                        StageAction.Submit(2, [ApplicationRequirement.Permission(11122)]),
                        StageAction.Submit(3, [ApplicationRequirement.POA()], "submit_skipping_cmc_approval"),
                        StageAction.Submit(5, [ApplicationRequirement.POA()], "submit_skipping_cmc_and_surveyor_approvals"),
                    ]},
                new Stage { ReferenceNumber = 2, NameEn = "Community Management Company Approval", NameAr = "إعتماد شركة إدارة المجمع", OnlineId = "CMC_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.CMC,
                    Actions =
                    [
                        StageAction.Approve(3, [ApplicationRequirement.POA()]),
                        StageAction.Approve(5, [ApplicationRequirement.POA()], "approve_skipping_surveyor"),
                        StageAction.Return(ApplicationRequirement.POA()),
                        StageAction.Reject(ApplicationRequirement.POA()),
                    ]},
                new Stage { ReferenceNumber = 3, NameEn = "Surveyor Approval", NameAr = "إعتماد شركة المساحة", OnlineId = "SURVEYOR_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(4, [ApplicationRequirement.Permission(44553)]),
                        StageAction.Approve(5, [ApplicationRequirement.POA()], "approve_skipping_surveyor_final_approval"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 4, NameEn = "Surveyor Final Approval", NameAr = "إعتماد شركة المساحة النهائي", OnlineId = "SURVEYOR_FINAL_APPROVAL", AdminId = "DRAFT", ActorType = TheApplicationPartyType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(5, [ApplicationRequirement.POA()]),
                        StageAction.Return(ApplicationRequirement.POA()),
                        StageAction.Reject(ApplicationRequirement.POA()),
                    ]},
                new Stage { ReferenceNumber = 5, NameEn = "ADREC Review", NameAr = "مراجعة البلدية", OnlineId = "DMT_REVIEW", AdminId = "SUBMITTED", ActorType = TheApplicationPartyType.ADREC},
                new Stage { ReferenceNumber = 6, NameEn = "ADREC Audit", NameAr = "تحت تدقيق البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_AUDIT", ActorType = TheApplicationPartyType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(7),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 7, NameEn = "Audited", NameAr = "تمت المراجعة", OnlineId = "DMT_REVIEW", AdminId = "AUDITED", ActorType = TheApplicationPartyType.ADREC},
                new Stage { ReferenceNumber = 8, NameEn = "ADREC Approval", NameAr = "اعتماد البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_APPROVAL", ActorType = TheApplicationPartyType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(9),
                        StageAction.Approve(10, id: "approve_skipping_fees"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 9, NameEn = "Pending Payment", NameAr = "في انتظار الدفع", OnlineId = "DARI_PENDING_PAYMENT", AdminId = "APPROVED", ActorType = TheApplicationPartyType.CMC},
                new Stage { ReferenceNumber = 10, NameEn = "Certificate Generation", NameAr = "استخراج الشهادة", OnlineId = "GENERATE_CERTIFICATE", AdminId = "APPROVED", ActorType = TheApplicationPartyType.SYSTEM},
                new Stage { ReferenceNumber = 11, NameEn = "Completed", NameAr = "مكتمل", OnlineId = "COMPLETED", AdminId = "COMPLETED", ActorType = TheApplicationPartyType.SYSTEM, IsFinal = true},
            ],
        };
    }
}

public sealed class ApplicationWorkflowBlueprint
{
    public ApplicationType ApplicationType { get; set; }

    public ApplicationRequirement[] InitiationRequirements { get; set; }
    public Stage[] Stages { get; set; }
}


public sealed record ApplicationRequirement
{
    public ApplicationRequirement(string requirementType)
    {
        RequirementType = requirementType;
        Id = requirementType;
    }
    public ApplicationRequirement(string requirementType, string id)
    {
        RequirementType = requirementType;
        Id = id;
    }
    public static ApplicationRequirement Permission(int permissionId) => new(permissionId.ToString());
    public static ApplicationRequirement Permission() => new("permission");
    public static ApplicationRequirement CompanyActivity(PredefinedCompanyActivity activity) => new("company-activity", activity);
    public static ApplicationRequirement POA() => new("poa");
    
    public string Id { get; }
    public string RequirementType { get; set; }
}

public sealed class PredefinedCompanyActivity : SmartEnum<PredefinedCompanyActivity, string>
{
    private PredefinedCompanyActivity(string nameEn, string nameAr, string value) : base(nameEn, value)
    {
        NameAr = nameAr;
    }
    public static readonly PredefinedCompanyActivity RealEstateLeaseAndManagementService = new("6820001", "Real estate lease and management services", "خدمات تاجير العقارات وادارتها");
    public static readonly PredefinedCompanyActivity RealEstatePurchaseAndSaleBrokerage = new("6820004", "Real estate purchase and sale brokerage", "الوساطة في بيع العقارات وشرائها");
    public static readonly PredefinedCompanyActivity FacilitiesManagementServices = new("8211010", "Facilities management services", "خدمات ادارة المنشأت");
    public static readonly PredefinedCompanyActivity OrganizationAndEventManagement = new("8230009", "Organization And Event Management", "تنظيم وإدارة الفعاليات");
    public static readonly PredefinedCompanyActivity BuildingsMaintenance = new("4329901", "Buildings  maintenance", "صيانة المباني");
    public static readonly PredefinedCompanyActivity BuildingsCleaningServices = new("8121001", "Buildings Cleaning Services", "خدمات التنظيف الداخلية للمباني");
    public static readonly PredefinedCompanyActivity DetectionServicesForRealEstate = new("6820007", "Detection Services For Real Estate", "خدمات الكشف على العقارات");
    public static readonly PredefinedCompanyActivity TaxConsultancy = new("6920002", "Tax Consultancy", "الاستشارات الضريبية");
    public static readonly PredefinedCompanyActivity AccountsAuditing = new("6920001", "Accounts auditing", "تدقيق ومراجعة الحسابات");
    public static readonly PredefinedCompanyActivity AdministrativeConsultancyAndStudies = new("7020003", "Administrative Consultancy And Studies", "استشارات ودراسات ادارية");

    public string NameAr { get; }

    public static implicit operator ApplicationRequirement(PredefinedCompanyActivity act) => new ApplicationRequirement(act.Value/*, act.Name, act.NameAr*/);
}
public sealed record Stage
{
    public int ReferenceNumber { get; set; }

    public string OnlineId { get; set; } //CMC_APPROVAL, SURVEYOR_APPROVAL, SURVEYOR_FINAL_APPROVAL, DMT_REVIEW
    public string AdminId { get; set; } //UNDER_AUDIT, UNDER_APPROVAL
    public string NameEn { get; set; }
    public string NameAr { get; set; }

    public StageAction[] Actions { get; set; }

    public TheApplicationPartyType ActorType { get; set; }
    public bool IsFinal { get; set; }
    public ApplicationRequirement[] Requirements { get; set; }
}
public sealed record StageAction
{
    public StageActionType Type { get; set; } //submit, approve, pay, reject, return
    public string Id { get; set; } //approve_skipping_cmc_signatory, approve_skipping_fee_payment, return_to_initiator, return_to_auditor
    public ApplicationRequirement[] Requirements { get; set; }

    public int TargetStage { get; set; } //the stage that the application should transition to when this action is executed

    public static StageAction Submit(int targetStage, ApplicationRequirement[]? requirements = null, string id = "submit") => new() { Type = StageActionType.Submit, Id = id, Requirements = requirements ?? [], TargetStage = targetStage };
    public static StageAction Approve(int targetStage, ApplicationRequirement[]? requirements = null, string id = "approve") => new() { Type = StageActionType.Approve, Id = id, Requirements = requirements ?? [], TargetStage = targetStage };
    public static StageAction Reject(ApplicationRequirement? requirement = null) => new() { Type = StageActionType.Reject, Id = "reject", Requirements = requirement is null ? [] : [requirement], TargetStage = 0 };
    public static StageAction Return(ApplicationRequirement? requirement = null) => new() { Type = StageActionType.Return, Id = "return", Requirements = requirement is null ? [] : [requirement], TargetStage = 1 };
    public static StageAction Return(int targetStage, ApplicationRequirement? requirement = null) => new() { Type = StageActionType.Return, Id = "return", Requirements = requirement is null ? [] : [requirement], TargetStage = targetStage };
}

public sealed class ApplicationStagePlatform : SmartEnum<ApplicationStagePlatform, string>
{
    private ApplicationStagePlatform(string value) : base(value, value) { }

    public static readonly ApplicationStagePlatform PUBLIC = new(nameof(PUBLIC));
    public static readonly ApplicationStagePlatform ADREC = new(nameof(ADREC));
    public static readonly ApplicationStagePlatform SYSTEM = new(nameof(SYSTEM));
}

public sealed class TheApplicationPartyType : SmartEnum<TheApplicationPartyType, string>
{
    private TheApplicationPartyType(string id, string nameEn, string nameAr, ApplicationStagePlatform platform) : base(nameEn, id)
    {
        NameAr = nameAr;
        Platform = platform;
    }

    public static readonly TheApplicationPartyType DEVELOPER = new(nameof(DEVELOPER), "Developer", "مطور عقاري", ApplicationStagePlatform.PUBLIC);
    public static readonly TheApplicationPartyType CMC = new(nameof(CMC), "Community Management Company", "شركة ادارة مجمعات", ApplicationStagePlatform.PUBLIC);
    public static readonly TheApplicationPartyType SURVEYOR = new(nameof(SURVEYOR), "Surveying Company", "شركة مساحة", ApplicationStagePlatform.PUBLIC);
    public static readonly TheApplicationPartyType FinancialAuditor = new("FINANCIAL_AUDITOR", "Financial Auditor", "مدقق مالي", ApplicationStagePlatform.PUBLIC);
    public static readonly TheApplicationPartyType ADREC = new(nameof(ADREC), "Abu Dhabi Real Estate Centre", "مركز أبوظبي العقاري", ApplicationStagePlatform.ADREC);
    public static readonly TheApplicationPartyType SYSTEM = new(nameof(SYSTEM), "System", "النظام", ApplicationStagePlatform.SYSTEM);

    public string NameAr { get; }
    public ApplicationStagePlatform Platform { get; }
}

public sealed class StageActionType : SmartEnum<StageActionType, string>
{
    private StageActionType(string value, string nameEn, string nameAr) : base(nameEn, value)
    {
        NameAr = nameAr;
    }

    public static readonly StageActionType Submit = new(nameof(Submit), nameof(Submit), "تقديم");
    public static readonly StageActionType Approve = new(nameof(Approve), nameof(Approve), "موافقة");
    public static readonly StageActionType Pay = new(nameof(Pay), "Pay Fees", "دفع الرسوم");
    public static readonly StageActionType GenerateOutputDocument = new(nameof(GenerateOutputDocument), "Generate Certificate", "إنشاء الشهادة");
    public static readonly StageActionType Reject = new(nameof(Reject), nameof(Reject), "رفض");
    public static readonly StageActionType Return = new(nameof(Return), nameof(Return), "إرجاع");
    public static readonly StageActionType Cancel = new(nameof(Cancel), nameof(Cancel), "إلغاء");

    public string NameAr { get; set; }
}
public sealed class ApplicationType : SmartEnum<ApplicationType, string>
{
    private ApplicationType(string name, string value, string dariServiceName, string nameAr) : base(name, value)
    {
        DariAppType = dariServiceName;
        NameAr = nameAr;
    }

    public static readonly ApplicationType ModifyMainCommunityRequest = new("Modify Main Community For Jointly Owned Properties", nameof(ModifyMainCommunityRequest), "MODIFY_MAIN_COMMUNITY_FOR_JOINTLY_OWNED_PROPERTIES", "تعديل مجمع رئيسي للعقارات ذا ملكية مشتركة");
    public static readonly ApplicationType ProjectPlanRequest = new("Submit/Modify Project Plan (feasibility study)", nameof(ProjectPlanRequest), "PROJECT_PLAN", "تسليم / تعديل خطة تنفيذ المشروع");
    public static readonly ApplicationType DepositInquiryRequest = new("Inquiry about deposits", nameof(DepositInquiryRequest), "DEPOSITS_INQUIRY", "الاستعلام عن المبالغ المودعة في حساب الضمان");
    public static readonly ApplicationType BankGuaranteeApproval = new("Request Bank Guarantee Approval", nameof(BankGuaranteeApproval), "BANK_GUARANTEE_APPROVAL", "طلب الموافقة على الضمان البنكي للصرف من حساب الضمان");

    public string DariAppType { get; }
    public string NameAr { get; }
}