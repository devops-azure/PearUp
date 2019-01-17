export class RouteTranslation {
    RouteTranslationId: number;
    VendorId: number;
    ExternalRouteName: string;
    IQRouteName: string;
    UseMap: boolean = true;
    IsInUse: string;
    IsBlocked: boolean = false;
    Blocked: string;
    CreatedDate: string;
    ModifiedDate: string;
    CreatedBy: string;
    ModifiedBy: string;
    CreatedById: number;
    ModifiedById: number;

    VendorName: string;
    IsActive: boolean;
}