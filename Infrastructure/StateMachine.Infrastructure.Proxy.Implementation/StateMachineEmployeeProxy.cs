using IT2.Common.BusManager.Abstraction;
using IT2.Common.Infrastructure.Proxy.Abstraction;
using IT2.Common.Infrastructure.Proxy.Abstraction.Exceptions;
using IT2.Common.Infrastructure.Proxy.Implementation.Employee;
using IT2.Employee.Contracts;
using IT2.Employee.Contracts.Requests;
using IT2.Employee.Contracts.Responses;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StateMachine.Infrastructure.Proxy.Implementation
{
    /// <summary>
    /// Имплементор Employee Proxy стейтмашины
    /// </summary>
    public class StateMachineEmployeeProxy : EmployeeProxy, IStateMachineEmployeeProxy
    {
        public StateMachineEmployeeProxy(HttpClient httpClient, IBusManager busManager) : base(httpClient, busManager)
        {
        }

        public async Task<OrganizationShortInfo> GetOrganizationShortInfoAsync(Guid organizationId)
        {
            return await BusManager.RequestAsync<EmployeeOrganizationGetShortInfoRequest, EmployeeOrganizationGetShortInfoResponse, OrganizationShortInfo>(
            new EmployeeOrganizationGetShortInfoRequest
            {
                Id = organizationId
            }, 
            (exception) =>
            {
                throw new ProxyException($"Ошибка получения краткой информация организации с идентификатором {organizationId}", exception);
            });
        }
    }
}
