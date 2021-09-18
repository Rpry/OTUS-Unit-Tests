using System;
using Trading.StateMachine.DataAccess.Models;

namespace StateMachine.BusinessLogic.Managers.UnitTests_Demo
{
    public class LotBuilder
    {
        private Guid _createdOrganizationId;
        private string _createdUserId;

        public LotBuilder()
        {
            
        }

        public LotBuilder WithCreatedOrganizationId(Guid organizationId)
        {
            _createdOrganizationId = organizationId;
            return this;
        }

        public LotBuilder WithCreatedUserId(string userId)
        {
            _createdUserId = userId;
            return this;
        }

        public Lot Build()
        {
            return new Lot()
            {
                CreatedOrganizationId = _createdOrganizationId,
                CreatedUserId = _createdUserId
            };
        }
    }
}