using loadsheddingapp.Models;

namespace loadsheddingapp.Services
{
    public interface ISecretsManagerService
    {
         DbSecretModel getDatabaseCredential(string secretID);
    }
}
