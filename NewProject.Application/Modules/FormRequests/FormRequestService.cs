using Microsoft.EntityFrameworkCore;
using MultipleFormat.Application.Modules.FormRequests.DTOs;
using MultipleFormat.Domain.Common.Enums;
using NewProject.Application.Common.CurrentUser;
using NewProject.Common.Results;
using NewProject.Infrastructure.Persistence;

namespace MultipleFormat.Application.Modules.FormRequests
{
    public class FormRequestService
    {
        private readonly MultipleFormatDbContext _ctx;

        public FormRequestService(MultipleFormatDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<ServiceResult<List<FormRequestSummaryDto>>> GetVisibleAsync(
            CurrentUserDto currentUser,
            CancellationToken cancellationToken = default)
        {
            var roleNames = await GetRoleNamesAsync(currentUser, cancellationToken);

            var isAdmin = roleNames.Any(x => x == nameof(Roles.Admin));
            var isSupervisor = roleNames.Any(x => x == nameof(Roles.Supervisor));
            var isGeneralUser = roleNames.Any(x => x == nameof(Roles.GeneralUser));

            if (!isAdmin && !isSupervisor && !isGeneralUser)
            {
                return ServiceResult<List<FormRequestSummaryDto>>.NotFound("El usuario no tiene un rol habilitado.");
            }

            var query = from formRequest in _ctx.FormRequests.AsNoTracking()
                        join employee in _ctx.Employees.AsNoTracking()
                            on formRequest.EmployeeId equals employee.Id
                        select new
                        {
                            FormRequest = formRequest,
                            Employee = employee
                        };

            if (!isAdmin)
            {
                if (isSupervisor && currentUser.SupervisorId.HasValue)
                {
                    query = query.Where(x =>
                        (currentUser.AccountId.HasValue &&
                         x.FormRequest.CreatedById == currentUser.AccountId.Value) ||
                        x.Employee.SupervisorId == currentUser.SupervisorId.Value);
                }
                else if (isGeneralUser && currentUser.Id > 0)
                {
                    query = query.Where(x => x.FormRequest.EmployeeId == currentUser.Id);
                }
                else
                {
                    return ServiceResult<List<FormRequestSummaryDto>>.Success(new List<FormRequestSummaryDto>());
                }
            }

            var requests = await query
                .OrderByDescending(x => x.FormRequest.CreatedDate)
                .Select(x => new FormRequestSummaryDto
                {
                    Id = x.FormRequest.Id,
                    EmployeeId = x.FormRequest.EmployeeId,
                    EmployeeNumber = x.Employee.EmployeeNumber,
                    EmployeeFullName = x.Employee.FullName,
                    FormType = x.FormRequest.FormType,
                    Status = x.FormRequest.Status,
                    CreatedDate = x.FormRequest.CreatedDate,
                    SubmittedDate = x.FormRequest.SubmittedDate,
                    Comments = x.FormRequest.Comments
                })
                .ToListAsync(cancellationToken);

            return ServiceResult<List<FormRequestSummaryDto>>.Success(requests);
        }

        public async Task<ServiceResult<FormRequestDetailDto>> GetVisibleDetailAsync(
            int id,
            CurrentUserDto currentUser,
            CancellationToken cancellationToken = default)
        {
            var roleNames = await GetRoleNamesAsync(currentUser, cancellationToken);
            var isAdmin = roleNames.Any(x => x == nameof(Roles.Admin));
            var isSupervisor = roleNames.Any(x => x == nameof(Roles.Supervisor));
            var isGeneralUser = roleNames.Any(x => x == nameof(Roles.GeneralUser));

            if (!isAdmin && !isSupervisor && !isGeneralUser)
            {
                return ServiceResult<FormRequestDetailDto>.NotFound("El usuario no tiene un rol habilitado.");
            }

            var request = await _ctx.FormRequests
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Join(
                    _ctx.Employees.AsNoTracking(),
                    formRequest => formRequest.EmployeeId,
                    employee => employee.Id,
                    (formRequest, employee) => new { FormRequest = formRequest, Employee = employee })
                .FirstOrDefaultAsync(cancellationToken);

            if (request == null)
            {
                return ServiceResult<FormRequestDetailDto>.NotFound("Formulario no encontrado.");
            }

            var canView = isAdmin ||
                (isSupervisor &&
                 ((currentUser.AccountId.HasValue && request.FormRequest.CreatedById == currentUser.AccountId.Value) ||
                  (currentUser.SupervisorId.HasValue && request.Employee.SupervisorId == currentUser.SupervisorId.Value))) ||
                (isGeneralUser && currentUser.Id > 0 && request.FormRequest.EmployeeId == currentUser.Id);

            if (!canView)
            {
                return ServiceResult<FormRequestDetailDto>.NotFound("Formulario no encontrado.");
            }

            ApproverDto? approver = null;
            if (request.FormRequest.ApproverId.HasValue)
            {
                var approverAccount = await _ctx.IDMAccounts
                    .AsNoTracking()
                    .Where(x => x.Id == request.FormRequest.ApproverId.Value && x.IsActive)
                    .Select(x => new { x.Id, x.FullName, x.DomainUserName })
                    .FirstOrDefaultAsync(cancellationToken);

                if (approverAccount != null)
                {
                    var position = await _ctx.Employees
                        .AsNoTracking()
                        .Where(x => x.DomainUserName == approverAccount.DomainUserName)
                        .Select(x => x.Position)
                        .FirstOrDefaultAsync(cancellationToken);

                    approver = new ApproverDto
                    {
                        Id = approverAccount.Id,
                        Name = approverAccount.FullName,
                        Position = position
                    };
                }
            }

            var detail = new FormRequestDetailDto
            {
                Id = request.FormRequest.Id,
                EmployeeId = request.FormRequest.EmployeeId,
                EmployeeNumber = request.Employee.EmployeeNumber,
                EmployeeFullName = request.Employee.FullName,
                DomainUserName = request.Employee.DomainUserName,
                FormType = request.FormRequest.FormType,
                Status = request.FormRequest.Status,
                CreatedDate = request.FormRequest.CreatedDate,
                SubmittedDate = request.FormRequest.SubmittedDate,
                Approver = approver,
                Comments = request.FormRequest.Comments
            };

            switch (request.FormRequest.FormType)
            {
                case FormType.Vacation:
                    var vacation = await _ctx.VacationRequests
                        .AsNoTracking()
                        .Where(x => x.FormRequestId == id)
                        .Select(x => new
                        {
                            x.VacationDays,
                            x.StartDate,
                            x.EndDate,
                            x.ReturnDate
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (vacation != null)
                    {
                        detail.VacationDays = vacation.VacationDays;
                        detail.StartDate = vacation.StartDate;
                        detail.EndDate = vacation.EndDate;
                        detail.ReturnDate = vacation.ReturnDate;
                    }
                    break;

                case FormType.Permission:
                    var permission = await _ctx.PermissionRequests
                        .AsNoTracking()
                        .Where(x => x.FormRequestId == id)
                        .Select(x => new
                        {
                            x.PermissionType,
                            x.PermissionDays,
                            x.StartDate,
                            x.EndDate,
                            x.ReturnDate,
                            x.SwapDetail
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (permission != null)
                    {
                        detail.PermissionType = permission.PermissionType;
                        detail.PermissionDays = permission.PermissionDays;
                        detail.StartDate = permission.StartDate;
                        detail.EndDate = permission.EndDate;
                        detail.ReturnDate = permission.ReturnDate;
                        detail.SwapDetail = permission.SwapDetail;
                    }
                    break;

                case FormType.TimeAdjustment:
                    var adjustment = await _ctx.TimeAdjustmentRequests
                        .AsNoTracking()
                        .Where(x => x.FormRequestId == id)
                        .Select(x => new
                        {
                            x.AdjustmentDate,
                            x.MissingPunchType,
                            x.EntryTime,
                            x.ExitTime,
                            x.Justification
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (adjustment != null)
                    {
                        detail.AdjustmentDate = adjustment.AdjustmentDate;
                        detail.MissingPunchType = adjustment.MissingPunchType;
                        detail.EntryTime = adjustment.EntryTime;
                        detail.ExitTime = adjustment.ExitTime;
                        detail.Justification = adjustment.Justification;
                    }
                    break;

                case FormType.DepartmentShiftChange:
                    var change = await _ctx.DepartmentShiftChangeRequests
                        .AsNoTracking()
                        .Where(x => x.FormRequestId == id)
                        .Select(x => new
                        {
                            x.EffectiveDate,
                            x.NewPlant,
                            x.NewShift,
                            x.NewDepartment,
                            x.NewSupervisor
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    if (change != null)
                    {
                        detail.EffectiveDate = change.EffectiveDate;
                        detail.NewPlant = change.NewPlant;
                        detail.NewShift = change.NewShift;
                        detail.NewDepartment = change.NewDepartment;
                        detail.NewSupervisor = change.NewSupervisor;
                    }
                    break;
            }

            return ServiceResult<FormRequestDetailDto>.Success(detail);
        }

        private async Task<List<string>> GetRoleNamesAsync(
            CurrentUserDto currentUser,
            CancellationToken cancellationToken)
        {
            var userName = currentUser.DomainUserName.Trim().ToUpperInvariant();

            return await (from account in _ctx.IDMAccounts.AsNoTracking()
                          join accountRole in _ctx.IDMAccountRoles.AsNoTracking()
                              on account.Id equals accountRole.AccountId
                          join role in _ctx.IDMRoles.AsNoTracking()
                              on accountRole.RoleId equals role.Id
                          where account.IsActive &&
                                role.IsActive &&
                                account.DomainUserName == userName
                          select role.RoleName)
                .ToListAsync(cancellationToken);
        }
    }
}
