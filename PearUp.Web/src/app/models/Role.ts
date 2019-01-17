export class Role {
    RoleID: number;
    RoleName: string;
    Description: string;
    CreatedDate: string;
    ModifiedDate: string;
    CreatedBy: string;
    ModifiedBy: string;
    CreatedByID: number;
    ModifiedByID: number;  
    isActive: boolean = true;
    Permissions: number[];
}