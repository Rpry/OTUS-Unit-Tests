using IT2.Common.Infrastructure.Proxy.Abstraction.Employee;
using IT2.Employee.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.Infrastructure.Proxy.Implementation
{
    public interface IStateMachineEmployeeProxy: IEmployeeProxy
    {
        /// <summary>
        /// Получение краткой информации организации
        /// </summary>
        /// <param name="organizationId">Идентификатор организации</param>
        /// <returns>Краткая информация об организации</returns>
        Task<OrganizationShortInfo> GetOrganizationShortInfoAsync(Guid organizationId);
    }
}
