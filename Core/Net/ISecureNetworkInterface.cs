using FaqatSafi.Core.AppUser;

namespace FaqatSafi.Core.Net
{
    public interface ISecureNetworkInterface : INetworkInterface
    {
        IAppUser User { get; }
    }
}
