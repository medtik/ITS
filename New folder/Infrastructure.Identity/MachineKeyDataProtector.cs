namespace Infrastructure.Identity
{
    using Microsoft.Owin.Security.DataProtection;
    using System.Web.Security;

    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] purposes;

        public MachineKeyDataProtector(params string[] purposes)
        {
            this.purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, purposes);
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, purposes);
        }
    }
}