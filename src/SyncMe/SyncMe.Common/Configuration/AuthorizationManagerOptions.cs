namespace SyncMe.Configuration;

public record AuthorizationManagerOptions
{
    public string IOSKeychainSecurityGroup { get; init; }
}
