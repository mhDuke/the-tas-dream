using Ardalis.SmartEnum;

namespace TAS;
public class SeadWorkflow
{
    //public ApplicationWorkflowBlueprint ProjectPlanWorkflowBuilder()
    //{
    //    return new ApplicationWorkflowBlueprint
    //    {
    //        ApplicationType = ApplicationType.ModifyMainCommunityRequest, 

    //    }
    //}
    public ApplicationWorkflowBlueprint CommunityModificationWorkflowBuilder()
    {
        return new ApplicationWorkflowBlueprint
        {
            ApplicationType = ApplicationType.ModifyMainCommunityRequest, // add ServiceId to ApplicationType for example 1562 for Modify Main Community For Jointly Owned Properties. all OA services under GroupId 1017. these are dari user permission specific information.
            InitiationRequirement = new ApplicationInitiationRequirement
            {
                RequiredCompanyActivities = [PredefinedCompanyActivity.FacilitiesManagementServices, PredefinedCompanyActivity.RealEstateLeaseAndManagementService],
                RequiredPermissions = [new ApplicationRequirement("6510"/*, "Initiate", "البدء"*/)]
            },
            Stages =
            [
                new Stage { ReferenceNumber = 1, NameEn = "Draft", NameAr = "تحت الإنشاء", OnlineId = "DRAFT", AdminId = "DRAFT", ActorType = ApplicationActorType.CMC,
                    Actions =
                    [
                        StageAction.Submit(2, [ApplicationRequirement.Permission(11122)]),
                        StageAction.Submit(3, [ApplicationRequirement.POA], "submit_skipping_cmc_approval"),
                        StageAction.Submit(5, [ApplicationRequirement.POA], "submit_skipping_cmc_and_surveyor_approvals"),
                    ]},
                new Stage { ReferenceNumber = 2, NameEn = "Community Management Company Approval", NameAr = "إعتماد شركة إدارة المجمع", OnlineId = "CMC_APPROVAL", AdminId = "DRAFT", ActorType = ApplicationActorType.CMC,
                    Actions =
                    [
                        StageAction.Approve(3, [ApplicationRequirement.POA]),
                        StageAction.Approve(5, [ApplicationRequirement.POA], "approve_skipping_surveyor"),
                        StageAction.Return(ApplicationRequirement.POA),
                        StageAction.Reject(ApplicationRequirement.POA),
                    ]},
                new Stage { ReferenceNumber = 3, NameEn = "Surveyor Approval", NameAr = "إعتماد شركة المساحة", OnlineId = "SURVEYOR_APPROVAL", AdminId = "DRAFT", ActorType = ApplicationActorType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(4, [ApplicationRequirement.Permission(44553)]),
                        StageAction.Approve(5, [ApplicationRequirement.POA], "approve_skipping_surveyor_final_approval"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 4, NameEn = "Surveyor Final Approval", NameAr = "إعتماد شركة المساحة النهائي", OnlineId = "SURVEYOR_FINAL_APPROVAL", AdminId = "DRAFT", ActorType = ApplicationActorType.SURVEYOR,
                    Actions =
                    [
                        StageAction.Approve(5, [ApplicationRequirement.POA]),
                        StageAction.Return(ApplicationRequirement.POA),
                        StageAction.Reject(ApplicationRequirement.POA),
                    ]},
                new Stage { ReferenceNumber = 5, NameEn = "ADREC Review", NameAr = "مراجعة البلدية", OnlineId = "DMT_REVIEW", AdminId = "SUBMITTED", ActorType = ApplicationActorType.ADREC},
                new Stage { ReferenceNumber = 6, NameEn = "ADREC Audit", NameAr = "تحت تدقيق البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_AUDIT", ActorType = ApplicationActorType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(7),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 7, NameEn = "Audited", NameAr = "تمت المراجعة", OnlineId = "DMT_REVIEW", AdminId = "AUDITED", ActorType = ApplicationActorType.ADREC},
                new Stage { ReferenceNumber = 8, NameEn = "ADREC Approval", NameAr = "اعتماد البلدية", OnlineId = "DMT_REVIEW", AdminId = "UNDER_APPROVAL", ActorType = ApplicationActorType.ADREC,
                    Actions =
                    [
                        StageAction.Approve(9),
                        StageAction.Approve(10, id: "approve_skipping_fees"),
                        StageAction.Return(),
                        StageAction.Reject(),
                    ]},
                new Stage { ReferenceNumber = 9, NameEn = "Pending Payment", NameAr = "في انتظار الدفع", OnlineId = "DARI_PENDING_PAYMENT", AdminId = "APPROVED", ActorType = ApplicationActorType.CMC},
                new Stage { ReferenceNumber = 10, NameEn = "Certificate Generation", NameAr = "استخراج الشهادة", OnlineId = "GENERATE_CERTIFICATE", AdminId = "APPROVED", ActorType = ApplicationActorType.SYSTEM},
                new Stage { ReferenceNumber = 11, NameEn = "Completed", NameAr = "مكتمل", OnlineId = "COMPLETED", AdminId = "COMPLETED", ActorType = ApplicationActorType.SYSTEM, IsFinal = true},
            ],
        };
    }
}

public sealed class ApplicationWorkflowBlueprint
{
    public ApplicationType ApplicationType { get; set; }

    public ApplicationInitiationRequirement InitiationRequirement { get; set; }
    public Stage[] Stages { get; set; }
}

public sealed record ApplicationInitiationRequirement
{
    public ApplicationRequirement[] RequiredPermissions { get; set; }
    public ApplicationRequirement[] RequiredCompanyActivities { get; set; }
}

public sealed record ApplicationRequirement
{
    public ApplicationRequirement(string id)
    {
        Id = id;
        //NameEn = nameEn;
        //NameAr = nameAr;
    }
    public static ApplicationRequirement Permission(int permissionId) => new ApplicationRequirement(permissionId.ToString());
    public string Id { get; }
    //public string NameEn { get; }
    //public string NameAr { get; }

    public static readonly ApplicationRequirement POA = new("POA");
}

public sealed class PredefinedCompanyActivity : SmartEnum<PredefinedCompanyActivity, string>
{
    private PredefinedCompanyActivity(string nameEn, string nameAr, string value) : base(nameEn, value)
    {
        NameAr = nameAr;
    }
    public static readonly PredefinedCompanyActivity RealEstateLeaseAndManagementService = new("6820001", "Real estate lease and management services", "خدمات تاجير العقارات وادارتها");
    public static readonly PredefinedCompanyActivity FacilitiesManagementServices = new("8211010", "Facilities management services", "خدمات ادارة المنشأت");

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

    public ApplicationActorType ActorType { get; set; }
    public bool IsFinal { get; set; }
}
public sealed record StageAction
{
    public StageActionType Type { get; set; } //submit, approve, pay, reject, return
    public string Id { get; set; } //approve_skipping_cmc_signatory, approve_skipping_fee_payment, return_to_initiator, return_to_auditor
    public ApplicationRequirement[] RequiredPermissions { get; set; }

    public int TargetStage { get; set; } //the stage that the application should transition to when this action is executed

    public static StageAction Submit(int targetStage, ApplicationRequirement[]? requirements = null, string id = "submit") => new() { Type = StageActionType.Submit, Id = id, RequiredPermissions = requirements ?? [], TargetStage = targetStage };
    public static StageAction Approve(int targetStage, ApplicationRequirement[]? requirements = null, string id = "approve") => new() { Type = StageActionType.Approve, Id = id, RequiredPermissions = requirements ?? [], TargetStage = targetStage };
    public static StageAction Reject(ApplicationRequirement? requirement = null) => new() { Type = StageActionType.Reject, Id = "reject", RequiredPermissions = requirement is null ? [] : [requirement], TargetStage = 0 };
    public static StageAction Return(ApplicationRequirement? requirement = null) => new() { Type = StageActionType.Return, Id = "return", RequiredPermissions = requirement is null ? [] : [requirement], TargetStage = 1 };
}

public sealed class ApplicationStagePlatform : SmartEnum<ApplicationStagePlatform, string>
{
    private ApplicationStagePlatform(string value) : base(value, value) { }

    public static readonly ApplicationStagePlatform PUBLIC = new(nameof(PUBLIC));
    public static readonly ApplicationStagePlatform ADREC = new(nameof(ADREC));
    public static readonly ApplicationStagePlatform SYSTEM = new(nameof(SYSTEM));
}

public sealed class ApplicationActorType : SmartEnum<ApplicationActorType, string>
{
    private ApplicationActorType(string id, string nameEn, string nameAr, ApplicationStagePlatform platform) : base(nameEn, id)
    {
        NameAr = nameAr;
        Platform = platform;
    }

    public static readonly ApplicationActorType CMC = new(nameof(CMC), "Community Management Company", "شركة ادارة مجمعات", ApplicationStagePlatform.PUBLIC);
    public static readonly ApplicationActorType SURVEYOR = new(nameof(SURVEYOR), "Surveying Company", "شركة مساحة", ApplicationStagePlatform.PUBLIC);
    public static readonly ApplicationActorType FinancialAuditor = new("FINANCIAL_AUDITOR", "Financial Auditor", "مدقق مالي", ApplicationStagePlatform.PUBLIC);
    public static readonly ApplicationActorType ADREC = new(nameof(ADREC), "Abu Dhabi Real Estate Centre", "مركز أبوظبي العقاري", ApplicationStagePlatform.ADREC);
    public static readonly ApplicationActorType SYSTEM = new(nameof(SYSTEM), "System", "النظام", ApplicationStagePlatform.SYSTEM);

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
    private ApplicationType(string name, string value, string dariServiceName, string nameAr, string tammServiceNameEn, string tammServiceNameAr) : base(name, value)
    {
        DariAppType = dariServiceName;
        NameAr = nameAr;
        TammSericeNameEn = tammServiceNameEn;
        TammSericeNameAr = tammServiceNameAr;
    }

    public static readonly ApplicationType MasterCommunityRegistrationRequest = new("Registration of Main Community for Jointly Owned Property",
        nameof(MasterCommunityRegistrationRequest), "JOINT_OWNED_MAIN_COMMUNITY_REGISTRATION", "تسجيل مجمع عقاري رئيسي ذا ملكية مشتركة", "Registration of a Main Real Estate Community with Jointly Owned", "تسجيل مجمع عقاري رئيسي ذا ملكية مشتركة");

    public static readonly ApplicationType SubCommunityRegistrationRequest = new("Registration of Sub Community for Jointly Owned Property",
        nameof(SubCommunityRegistrationRequest), "JOINT_OWNED_SUB_COMMUNITY_REGISTRATION", "تسجيل مجمع عقاري فرعي ذا ملكية مشتركة", "Registration of a Sub Real Estate Community with Jointly Owned", "تسجيل مجمع عقاري فرعي ذا ملكية مشتركة");

    public static readonly ApplicationType BudgetApprovalRequest = new("Approval Of Service Charge Budget for Jointly Owned Property",
        nameof(BudgetApprovalRequest), "APPROVAL_OF_SERVICE_CHARGE_BUDGET_FOR_JOINTLY_OWNED_PROPERTY", string.Empty, "Approval of Service Charges Budget for Real Estate Community with Jointly Owned", "اعتماد ميزانية رسوم خدمات لمجمع عقاري ذا ملكية مشتركة");

    public static readonly ApplicationType IssuingServiceChargesInvoices = new("Issuing Service Charge and Community Charge Invoices",
        nameof(IssuingServiceChargesInvoices), "ISSUING_SERVICE_CHARGE_AND_COMMUNITY_CHARGE_INVOICES", string.Empty, "Issuing Invoices for Real Estate Community with Jointly Owned", "إصدار فواتير خدمات لمجمع عقاري ذا ملكية مشتركة");

    public static readonly ApplicationType PaymentOfServiceChargesInvoice = new("Payment of Service Charges and Community Charges Invoice",
         nameof(PaymentOfServiceChargesInvoice), "PAYMENT_OF_SERVICE_CHARGES_AND_COMMUNITY_CHARGES_INVOICE", string.Empty, "Payment of Invoices for Real Estate Community with Jointly Owned", "دفع فواتير خدمات لمجمع عقاري ذا ملكية مشتركة");

    public static readonly ApplicationType ModifyMainCommunityRequest = new("Modify Main Community For Jointly Owned Properties",
        nameof(ModifyMainCommunityRequest), "MODIFY_MAIN_COMMUNITY_FOR_JOINTLY_OWNED_PROPERTIES", "تعديل مجمع رئيسي للعقارات ذا ملكية مشتركة", "Modify Main Community For Jointly Owned Properties", "تعديل مجمع رئيسي للعقارات ذا ملكية مشتركة");

    public static readonly ApplicationType ModifySubCommunityRequest = new("Modify Sub Community For Jointly Owned Properties",
        nameof(ModifySubCommunityRequest), "MODIFY_SUB_COMMUNITY_FOR_JOINTLY_OWNED_PROPERTIES", "تعديل مجمع فرعي للعقارات ذا ملكية مشتركة", "Modify Sub Community For Jointly Owned Properties", "تعديل مجمع فرعي للعقارات ذا ملكية مشتركة");

    public static readonly ApplicationType CommunityManagementCompanyLicenseApprovalRequest = new("Approval of an Administrative Supervision Company for Jointly Owned Properties Services",
        nameof(CommunityManagementCompanyLicenseApprovalRequest), "COMMUNITY_MANAGEMENT_COMPANY_LICENSE_APPROVAL_FOR_JOINTLY_OWNED_PROPERTIES_SERVICES", "اعتماد شركة إدارة مجمعات عقارية ذات ملكية مشتركة", "Approval of an Administrative Supervision Company for Jointly Owned Properties Services", "اعتماد شركة إدارة مجمعات عقارية ذات ملكية مشتركة");

    public static readonly ApplicationType FinancialAuditorCompanyRegistrationRequest = new("Approval of The Financial Auditing Company for Jointly Owned Properties Service Charges",
        nameof(FinancialAuditorCompanyRegistrationRequest), "FINANCIAL_AUDITING_COMPANY_LICENSE_APPROVAL_FOR_JOINTLY_OWNED_PROPERTIES", "اعتماد شركة مدقق مالي لمجمعات عقارية ذات ملكية مشتركة", "Approval Of The Financial Auditing Company For Jointly Owned Properties Service Charges", "اعتماد شركة مدقق مالي لمجمعات عقارية ذات ملكية مشتركة");

    public string DariAppType { get; }
    public string NameAr { get; }
    public string TammSericeNameEn { get; }
    public string TammSericeNameAr { get; }


    private static readonly Dictionary<string, ApplicationType> _dariAppTypeMap =
        List.ToDictionary(
            type => type.DariAppType,
            type => type,
            StringComparer.OrdinalIgnoreCase);

    public static ApplicationType FromDariAppType(string dariAppType)
    {
        return _dariAppTypeMap.TryGetValue(dariAppType, out var appType)
            ? appType
            : throw new ArgumentException($"Unknown application type: {dariAppType}", nameof(dariAppType));
    }
}