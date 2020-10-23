using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECWebApp.Domain.Constant
{
    public class RoleAssignment
    {
        public static Guid ROLE_ADMIN = new Guid("0B07FF51-E008-4A90-8C7F-6CEDF37E7B23");
        public static Guid ROLE_MEMBER= new Guid("a61bdc00-a6b4-435d-ad88-c54f353999a7");
        public static Guid ROLE_TAILOR = new Guid("6866fc5d-6fdc-4bf3-a330-9045c99caf44");
        public const String ROLE_MEMBER_NAME = "Customer";
        public const String ROLE_ADMIN_NAME = "Administrator";
        public const String ROLE_TAILOR_NAME = "Tailor";
    }
}
