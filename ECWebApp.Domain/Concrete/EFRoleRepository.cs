using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ECWebApp.Domain.Concrete
{
    class EFRoleRepository : RoleProvider
    {
        private DIYCommerceV2Entities context = new DIYCommerceV2Entities();

        /// <summary>
        /// Add Users in to their respective role 
        /// </summary>
        /// <param name="CustomerIds"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] CustomerIds, string[] roleNames)
        {
            foreach (var CustomerId in CustomerIds)
            {
                foreach (var roleName in roleNames)
                {
                    var _RoleId = context.Roles.Where(x => x.RoleName.Equals(roleName)).Select(x => x.RoleId).FirstOrDefault();
                    RoleAssign roleAssign = new RoleAssign()
                    {
                        CustomerId = Guid.Parse(CustomerId),
                        RoleId = _RoleId,
                        RoleAssignCreatedBy = roleName,
                        RoleAssignCreatedOn = DateTime.UtcNow.Date,
                        RoleAssignUpdatedBy = roleName,
                        RoleAssignUpdatedOn = DateTime.UtcNow.Date
                    };
                    context.RoleAssigns.Add(roleAssign);
                    context.SaveChangesAsync();
                }
            }
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Create a new Role
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
            Role role = new Role()
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName,
                RoleStatus = 1,
                RoleCreatedBy = roleName,
                RoleCreatedOn = DateTime.UtcNow.Date,
                RoleUpdatedBy = roleName,
                RoleUpdatedOn = DateTime.UtcNow.Date
            };
            context.Roles.Add(role);
            context.SaveChangesAsync();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all existing roles in database
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            var UserRoles = context.Roles
                                  .Select(x => x.RoleName);

            return UserRoles.ToArray<string>();
        }

        /// <summary>
        /// Get list of roles for a user
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string CustomerID)
        {
            Guid _CustomerID = Guid.Parse(CustomerID);
            var RoleIds = context.RoleAssigns.Where(x => x.CustomerId.Equals(_CustomerID))
                                            .Select(x => x.RoleId);
           List<string> RoleNames = new List<string>(); 
            foreach (var RoleId in RoleIds)
            {
                RoleNames.Add(context.Roles.Where(x=> x.RoleId.Equals(RoleId)).Select(x => x.RoleName).FirstOrDefault());
            }
             string[] output = new string[RoleIds.Count()];
            output = RoleNames.ToArray<string>();
            return output;
        }


        /// <summary>
        /// Get a list of users in a role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override string[] GetUsersInRole(string roleName)
        {
            var roleId = context.Roles
                                   .Where(x => x.RoleName.Equals(roleName))
                                   .Select(x => x.RoleId).FirstOrDefault();
            var UsersInRole = context.RoleAssigns
                                   .Where(x => x.RoleId.Equals(roleId))
                                   .Select(x => x.CustomerId.ToString());
            return UsersInRole.ToArray<string>();
        }


        /// <summary>
        /// Check whether user is in the particular role 
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string CustomerId, string roleName)
        {
            bool output = false;
            var roleId = context.Roles
                                   .Where(x => x.RoleName.Equals(roleName))
                                   .Select(x => x.RoleId).FirstOrDefault();
            var result = context.RoleAssigns
                                   .Where(x => x.CustomerId.Equals(Guid.Parse(CustomerId)) && x.RoleId.Equals(roleId))
                                   .ToList();
            if (result.Count > 0)
            {
                output = true;
            }

            return output;
        }

        /// <summary>
        /// Remove User from selected Roles
        /// </summary>
        /// <param name="CustomerIds"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] CustomerIds, string[] roleNames)
        {
            foreach (var CustomerId in CustomerIds)
            {
                foreach (var roleName in roleNames)
                {
                    var _RoleId = context.Roles.Where(x => x.RoleName.Equals(roleName)).Select(x => x.RoleId).FirstOrDefault();
                    RoleAssign RoleTOBeRemoved = context.RoleAssigns.Where(x => x.RoleId.Equals(_RoleId) && x.CustomerId.Equals(CustomerId)).FirstOrDefault();
                    context.RoleAssigns.Remove(RoleTOBeRemoved);
                    context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Check whether the role existed or not
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            return context.Roles.Any(x => x.RoleName.Equals(roleName));
        }
    }
}
